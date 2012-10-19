using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Kinect;

namespace personal_training_martial_arts.Posture
{
    class Posture
    {
        protected Vector3[] joints {get; set;}

        public Posture()
        {
        }

        public Posture(Skeleton skeleton)
            : this(Posture.castSkeletonToJoints(skeleton))
        {
        }

        public Posture(Vector3[] joints)
        {
            this.joints = joints;
        }

        public static Vector3[] castSkeletonToJoints(Skeleton s)
        {
            Vector3[] joints = new Vector3[20];
            int index = 0;

            foreach (Joint j in s.Joints)
            {
                joints[index] = new Vector3(j.Position.X, j.Position.Y, j.Position.Z);
                index++;
            }

            return joints;
        }

        public Boolean compareTo(Posture p, float tolerance)
        {
            /**
             * @TODO: Normalizar primero y despues comparamos
             */

            return false;
        }
    }
}
