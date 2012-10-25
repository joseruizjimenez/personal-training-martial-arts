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

        public struct disVectors
        {
            public int index; //Marcador miembro
            public float[] Distance; //Distancias a los puntos (de index a cada parte)
        }

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

        private disVectors[] measurePosture(Vector3[] mv)
        {
            disVectors[] dvs = null;
            disVectors dv = new disVectors();
            int ii = 0, j=0;

            foreach (Vector3 v in mv)
            {
                dv.index=ii;

                foreach (Vector3 vv in mv)
                {
                    if (j == ii) dv.Distance[j] = 0; 
                    else dv.Distance[j] = Math.Abs((Math.Abs(v.X - vv.X)) + (Math.Abs(v.Y - vv.Y)) + (Math.Abs(v.Z - vv.Z)));         
                }
                dvs[ii] = dv;
                j = 0; ii++;
            }

            return dvs;
        }

        private float[] proportion(float[] norma, float[] dd)
        {
            //Aqui habria que normalizar y blablabla

            return dd;
        }

        public Boolean compareTo(Posture pos, float averageTolerance, float puntualTolerance)
        {
            float[] distance = null, distanceN =null;
            float average=0;
            int ii = 0;

            disVectors[] dvSaved = measurePosture(this.joints); // Postura Almacenada XML
            disVectors[] dvPos = measurePosture(pos.joints); // Postura Realizada

            foreach (disVectors dd in dvSaved) 
            {
                distance = dd.Distance;
                distanceN = dvPos[ii].Distance;

                distanceN = proportion(distance, distanceN); //¿PROPORCION?

                for (int k = 0; k < 20; k++)
                {
                    if ((Math.Abs(distance[k] - distanceN[k])) > puntualTolerance) return false; //Puntual
                    else average += Math.Abs(distance[k] - distanceN[k]);
                }

                if (average > averageTolerance) return false; //Media
                average = 0; ii++;
            }
            
            return true; //Postura correcta
        }



    } //END CLASS
}
