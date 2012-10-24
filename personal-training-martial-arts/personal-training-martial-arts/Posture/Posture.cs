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
using System.Xml;

namespace personal_training_martial_arts.Posture
{
    class Posture
    {
        public Vector3[] joints {get; set;}

        public Posture(){}

        public Posture(Skeleton skeleton): this(Posture.castSkeletonToJoints(skeleton)){}

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

        
        public Boolean compareTo(Posture p, float averageTolerance, float puntualTolerance)
        {
            /**
             * @TODO: Visualizamos el cumplimiento punto a punto y despues como media para asumir
             * la postura como valida.
             */

            int i = 0; 
            float avDistance=0; //Separacion promedio (20 puntos)

            foreach (Vector3 v in joints)
            {
                Vector3 pVector = p.joints[i]; i++;

                float distance = Math.Abs((Math.Abs(v.X - pVector.X)) + (Math.Abs(v.Y - pVector.Y)) 
                                 + (Math.Abs(v.Z - pVector.Z)));

                if (distance > puntualTolerance) return false;
                else avDistance += distance;
            }

            if (avDistance > averageTolerance) return false;
            else return true; //Postura correcta
        }
    }
}
