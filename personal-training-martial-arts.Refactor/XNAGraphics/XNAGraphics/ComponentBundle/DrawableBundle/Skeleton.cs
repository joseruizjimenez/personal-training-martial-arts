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
using XNAGraphics.ComponentBundle.PostureBundle;

namespace XNAGraphics.ComponentBundle.DrawableBundle
{
    class Skeleton : XNAGraphics.KernelBundle.BasicsBundle.BasicDrawable
    {
        private double[] accuracy;
        private KinectSensor sensor;
        private KinectSkeleton skeleton = null;
        public Posture postureToCompare = null;
        private Kinect kinect;
        private PostureBundle.Posture posture;
        // Textura del hueso
        private Texture2D boneTexture;
        // Textura de la articulacion
        private Texture2D jointTexture;
        private CoordinateMapper mapper;

        public Skeleton(int x, int y, Kinect kinect, KinectSkeleton skeleton)
            : this(x, y, kinect, new Posture(skeleton))
        {
            this.skeleton = skeleton;
        }

        public Skeleton(int x, int y, Kinect kinect, Posture posture)
            : base(x, y)
        {
            this.posture = posture;
            this.sensor = kinect.kinectSensor;
            this.kinect = kinect;
            this.mapper = new CoordinateMapper(this.sensor);
        }

        public override void load(Game game)
        { }

        public override void update(GameTime gameTime)
        {
            this.skeleton = kinect.skeleton;
            this.posture = new Posture(this.skeleton);
            // IMPORTANT:
            // FIX: ARREGLAR ESTO PARA QUE CALCULE LOS ERRORES Y LA DIFICULTAD LA DE DESDE UNA CAPA MAS ARRIBA...
            double fix = 0.6;
            if (postureToCompare != null) this.posture.compareTo(this.postureToCompare, ref accuracy, fix, fix);
        }


        public override void draw(SpriteBatch spriteBatch)
        {              
             // PINTA HUESOS
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
                // *BETA* - LO COMENTO PARA SACAR LA BETA
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
                    1.0f,
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
                return new Vector2(colorPt.X, colorPt.Y);
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
            Color color = Color.LightGreen;

            spriteBatch.Draw(this.boneTexture, start, null, color, angle, new Vector2(0.5f, 0.0f),
                scale, SpriteEffects.None, 1.0f);
        }
    }
}