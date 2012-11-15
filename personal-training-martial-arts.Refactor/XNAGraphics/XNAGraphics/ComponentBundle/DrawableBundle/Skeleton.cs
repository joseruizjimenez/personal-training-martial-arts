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
using XNAGraphics.KinectBundle;

namespace XNAGraphics.ComponentBundle.DrawableBundle
{
    class Skeleton //: XNAGraphics.KernelBundle.BasicsBundle.BasicDrawable
    {
        /*Kinect kinect;
        Microsoft.Kinect.Skeleton skeleton;

        public Skeleton(int x, int y, Kinect k)
            : base(x, y)
        {
            this.kinect = k;
        }

        private Vector2 SkeletonToColorMap(SkeletonPoint point)
        {
            if ((null != sensor) && (null != sensor.ColorStream))
            {
                // This is used to map a skeleton point to the color image location
                ColorImagePoint colorPt = mapper.MapSkeletonPointToColorPoint(point, sensor.ColorStream.Format);
                return new Vector2(colorPt.X, colorPt.Y);
            }

            return Vector2.Zero;
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
        private void DrawBone(Vector3[] joints, JointType startJoint, JointType endJoint)
        {
            Vector2 start = this.SkeletonToColorMap(joints[(int)startJoint]);
            Vector2 end = this.SkeletonToColorMap(joints[(int)endJoint]);
            Vector2 diff = end - start;
            Vector2 scale = new Vector2(1.0f, diff.Length() / this.boneTexture.Height);

            float angle = (float)Math.Atan2(diff.Y, diff.X) - MathHelper.PiOver2;
            Color color = Color.LightGreen;

            this.spriteBatch.Draw(this.boneTexture, start, null, color, angle, new Vector2(0.5f, 0.0f),
                scale, SpriteEffects.None, 1.0f);
        }

        /// <summary>
        /// This method draws a bone.
        /// </summary>
        /// <param name="joints">The joint data.</param>
        /// <param name="startJoint">The starting joint.</param>
        /// <param name="endJoint">The ending joint.</param>
        private void DrawBone(JointCollection joints, JointType startJoint, JointType endJoint)
        {
            Vector2 start = this.SkeletonToColorMap(joints[startJoint].Position);
            Vector2 end = this.SkeletonToColorMap(joints[endJoint].Position);
            Vector2 diff = end - start;
            Vector2 scale = new Vector2(1.0f, diff.Length() / this.boneTexture.Height);

            float angle = (float)Math.Atan2(diff.Y, diff.X) - MathHelper.PiOver2;

            Color color = Color.LightGreen;
            if (joints[startJoint].TrackingState != JointTrackingState.Tracked ||
                joints[endJoint].TrackingState != JointTrackingState.Tracked)
            {
                color = Color.Gray;
            }

            this.spriteBatch.Draw(this.boneTexture, start, null, color, angle,
                new Vector2(0.5f, 0.0f), scale, SpriteEffects.None, 1.0f);
        }

        public override void load(Game game)
        { }

        public override void update(GameTime gameTime)
        {
            this.skeleton = kinect.skeleton;
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            // PINTA HUESOS
            this.DrawBone(this.skeleton.Joints, JointType.Head, JointType.ShoulderCenter);
            this.DrawBone(this.skeleton.Joints, JointType.ShoulderCenter, JointType.ShoulderLeft);
            this.DrawBone(this.skeleton.Joints, JointType.ShoulderCenter, JointType.ShoulderRight);
            this.DrawBone(this.skeleton.Joints, JointType.ShoulderCenter, JointType.Spine);
            this.DrawBone(this.skeleton.Joints, JointType.Spine, JointType.HipCenter);
            this.DrawBone(this.skeleton.Joints, JointType.HipCenter, JointType.HipLeft);
            this.DrawBone(this.skeleton.Joints, JointType.HipCenter, JointType.HipRight);

            this.DrawBone(this.skeleton.Joints, JointType.ShoulderLeft, JointType.ElbowLeft);
            this.DrawBone(this.skeleton.Joints, JointType.ElbowLeft, JointType.WristLeft);
            this.DrawBone(this.skeleton.Joints, JointType.WristLeft, JointType.HandLeft);

            this.DrawBone(this.skeleton.Joints, JointType.ShoulderRight, JointType.ElbowRight);
            this.DrawBone(this.skeleton.Joints, JointType.ElbowRight, JointType.WristRight);
            this.DrawBone(this.skeleton.Joints, JointType.WristRight, JointType.HandRight);

            this.DrawBone(this.skeleton.Joints, JointType.HipLeft, JointType.KneeLeft);
            this.DrawBone(this.skeleton.Joints, JointType.KneeLeft, JointType.AnkleLeft);
            this.DrawBone(this.skeleton.Joints, JointType.AnkleLeft, JointType.FootLeft);

            this.DrawBone(this.skeleton.Joints, JointType.HipRight, JointType.KneeRight);
            this.DrawBone(this.skeleton.Joints, JointType.KneeRight, JointType.AnkleRight);
            this.DrawBone(this.skeleton.Joints, JointType.AnkleRight, JointType.FootRight);

            // PINTA ARTICULACIONES
            Vector2 jointOrigin = new Vector2(spriteGraphic.Width / 2, spriteGraphic.Height / 2);
            int i = 0;
            foreach (Joint joint in this.skeleton.Joints)
            {
                // *BETA* - LO COMENTO PARA SACAR LA BETA
                //float colorValue = float.Parse(""+pointData[i]);
                //Color jointColor = new Color(new Vector3(colorValue,(1-colorValue),0.0F));
                i++;
                Color jointColor = Color.SlateBlue;
                if (joint.TrackingState != JointTrackingState.Tracked)
                {
                    jointColor = Color.Red;
                } //Tracked state, no los keremos

                //Es posible que se pueda usar otro override con menos parámetros.
                //Pero mola mas asi :P xD
                this.spriteBatch.Draw(
                    spriteGraphic,
                    this.SkeletonToColorMap(joint.Position),
                    null,
                    jointColor,
                    0.0f,
                    jointOrigin,
                    1.0f,
                    SpriteEffects.None,
                    0.0f);
            }
        }*/
    }
}
