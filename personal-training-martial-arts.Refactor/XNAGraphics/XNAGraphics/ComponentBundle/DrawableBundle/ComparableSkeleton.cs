using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Kinect;
using KinectSkeleton = Microsoft.Kinect.Skeleton;
using XNAGraphics.KinectBundle;
using XNAGraphics.KinectBundle.PostureBundle;

namespace XNAGraphics.ComponentBundle.DrawableBundle
{
    public class ComparableSkeleton : XNAGraphics.KernelBundle.BasicsBundle.BasicDrawable
    {
        private double[] accuracy;
        private KinectSensor sensor;
        private KinectSkeleton skeleton = null;
        public Posture postureToCompare = null;
        private Kinect kinect;
        private Posture posture;
        // Textura del hueso
        private Texture2D boneTexture;
        // Textura de la articulacion
        private Texture2D jointTexture;
        private CoordinateMapper mapper;
   
        public ComparableSkeleton(int x, int y, Kinect kinect, Object jointTexture)
            : base(x, y)
        {
            this.sensor = kinect.kinectSensor;
            this.kinect = kinect;
            this.accuracy = new double[20];
            if(this.sensor != null)
                this.mapper = new CoordinateMapper(this.sensor);
            this.jointTexture = (Texture2D)jointTexture;
        }

        protected override void onLoad(Game game)
        {
            this.boneTexture = new Texture2D(game.GraphicsDevice, 5, 1);
            this.boneTexture.SetData(new Color[] { Color.White * 0.75f, Color.White * 0.75f, Color.White * 0.75f, Color.White * 0.75f, Color.White * 0.75f });
        }

        protected override void onUpdate(GameTime gameTime)
        {
            this.skeleton = this.kinect.skeleton;
            if (this.skeleton != null)
            {

                if (this.posture != null) this.posture.joints = Posture.castSkeletonToJoints(this.skeleton);
                else this.posture = new Posture(this.skeleton);
            }
            // IMPORTANT:
            // FIX: ARREGLAR ESTO PARA QUE CALCULE LOS ERRORES Y LA DIFICULTAD LA DE DESDE UNA CAPA MAS ARRIBA...
            if (postureToCompare != null && this.posture != null)
                this.posture.compareTo(this.postureToCompare, ref accuracy, 0.07f, 0.058f);
            else
                if (this.kinect.skeleton != null)
                    accuracy = new double[20] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        }


        protected override void onDraw(SpriteBatch spriteBatch)
        {              
             // PINTA HUESOS
            if (this.posture == null) return;
            this.DrawBone(this.posture.joints, JointType.Head, JointType.ShoulderCenter,spriteBatch);
            this.DrawBone(this.posture.joints, JointType.ShoulderCenter, JointType.ShoulderLeft,spriteBatch);
            this.DrawBone(this.posture.joints, JointType.ShoulderCenter, JointType.ShoulderRight,spriteBatch);
            this.DrawBone(this.posture.joints, JointType.ShoulderCenter, JointType.Spine,spriteBatch);
            this.DrawBone(this.posture.joints, JointType.Spine, JointType.HipCenter,spriteBatch);
            this.DrawBone(this.posture.joints, JointType.HipCenter, JointType.HipLeft,spriteBatch);
            this.DrawBone(this.posture.joints, JointType.HipCenter, JointType.HipRight,spriteBatch);

            this.DrawBone(this.posture.joints, JointType.ShoulderLeft, JointType.ElbowLeft,spriteBatch);
            this.DrawBone(this.posture.joints, JointType.ElbowLeft, JointType.WristLeft, spriteBatch);
            this.DrawBone(this.posture.joints, JointType.WristLeft, JointType.HandLeft, spriteBatch);

            this.DrawBone(this.posture.joints, JointType.ShoulderRight, JointType.ElbowRight, spriteBatch);
            this.DrawBone(this.posture.joints, JointType.ElbowRight, JointType.WristRight, spriteBatch);
            this.DrawBone(this.posture.joints, JointType.WristRight, JointType.HandRight, spriteBatch);

            this.DrawBone(this.posture.joints, JointType.HipLeft, JointType.KneeLeft, spriteBatch);
            this.DrawBone(this.posture.joints, JointType.KneeLeft, JointType.AnkleLeft, spriteBatch);
            this.DrawBone(this.posture.joints, JointType.AnkleLeft, JointType.FootLeft, spriteBatch);

            this.DrawBone(this.posture.joints, JointType.HipRight, JointType.KneeRight, spriteBatch);
            this.DrawBone(this.posture.joints, JointType.KneeRight, JointType.AnkleRight, spriteBatch);
            this.DrawBone(this.posture.joints, JointType.AnkleRight, JointType.FootRight, spriteBatch);

            // PINTA ARTICULACIONES
            Vector2 jointOrigin = new Vector2(jointTexture.Width / 2, jointTexture.Height / 2);
            for ( int i = 0; i < this.posture.joints.Length; i++)
            {
                Vector3 joint = this.posture.joints[i];
                
                float colorValue = float.Parse(""+accuracy[i]);
                Color jointColor = new Color(new Vector3(colorValue, (1 - colorValue), 0.0F));

                jointColor = (this.postureToCompare == null) ? Color.White : jointColor;

                spriteBatch.Draw(
                    jointTexture,
                    this.SkeletonToColorMap(joint),
                    null,
                    jointColor,
                    0.0f,
                    jointOrigin,
                    1.15f,
                    SpriteEffects.None,
                    0.0f);
            }
        }

        private Vector2 SkeletonToColorMap(Vector3 joint)
        {
            SkeletonPoint point = new SkeletonPoint();
            point.X = joint.X;
            point.Y = joint.Y;
            point.Z = joint.Z;
            if ((null != sensor) && (null != sensor.ColorStream))
            {
                // This is used to map a skeleton point to the color image location
                ColorImagePoint colorPt = mapper.MapSkeletonPointToColorPoint(point, sensor.ColorStream.Format);
                return new Vector2(colorPt.X + this.x, colorPt.Y + this.y);
            }

            return Vector2.Zero;
        }

        /// <summary>
        /// This method draws a bone.
        /// </summary>
        /// <param name="joints">The joint data.</param>
        /// <param name="startJoint">The starting joint.</param>
        /// <param name="endJoint">The ending joint.</param>
        private void DrawBone(Vector3[] joints, JointType startJoint, JointType endJoint, SpriteBatch spriteBatch)
        {
            Vector2 start = this.SkeletonToColorMap(joints[(int)startJoint]);
            Vector2 end = this.SkeletonToColorMap(joints[(int)endJoint]);
            Vector2 diff = end - start;
            Vector2 scale = new Vector2(1.0f, diff.Length() / this.boneTexture.Height);

            float angle = (float)Math.Atan2(diff.Y, diff.X) - MathHelper.PiOver2;
            
            //Añadiendo media de colores entre 2 puntos
            float colorValue = (float.Parse(""+accuracy[(int)startJoint]) + float.Parse(""+accuracy[(int)endJoint])) / 2;
            Color jointColor = new Color(new Vector3(colorValue, (1 - colorValue), 0.0F));
            Color color = jointColor;

            spriteBatch.Draw(this.boneTexture, start, null, color, angle, new Vector2(0.5f, 0.0f),
                scale, SpriteEffects.None, 1.0f);
        }
    }
}