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

        public float compareTo(Posture pos1, ref double[] result, double errorPuntual, double errorGeneral)
        {
            Vector3[] e1 = pos1.joints;
            Vector3[] e2 = this.joints;
            double media = 0;
            double difPuntual = 0;
            double mediaDif = 0;
            result = new double[20];


            for (int j1 = 0; j1 < 20; j1++)
            {
                for (int j2 = (j1 + 1); j2 < 20; j2++)
                {
                    media += distancia(e1[j1], e1[j2]) / distancia(e2[j1], e2[j2]);
                }
            }
            media = media / 190;

            for (int j1 = 0; j1 < 20; j1++)
            {
                for (int j2 = (j1 + 1); j2 < 20; j2++)
                {
                    difPuntual = Math.Abs((distancia(e1[j1], e1[j2]) / distancia(e2[j1], e2[j2])) - media);
                    mediaDif += difPuntual;
                    if (difPuntual > errorPuntual)
                    {
                        result[j1] += 0.05;
                        result[j2] += 0.05;
                    }
                }
            }

            Console.Write(" " + ((mediaDif / 190) / errorPuntual) + " \n");
            return float.Parse("" + ((mediaDif / 190) / errorGeneral));
        }

        private double distancia(Vector3 punto1, Vector3 punto2)
        {

            return Math.Sqrt(Math.Pow(punto1.X - punto2.X, 2) + Math.Pow(punto1.Y - punto2.Y, 2) + Math.Pow(punto1.Z - punto2.Z, 2));

        }

    } //END CLASS
}
