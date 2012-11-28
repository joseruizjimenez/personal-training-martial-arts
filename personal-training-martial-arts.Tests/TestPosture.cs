using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NMock2;
using Microsoft.Kinect;
using Microsoft.Xna.Framework;
using personal_training_martial_arts.Posture;

namespace personal_training_martial_arts.Test
{
    [TestFixture]
    public class TestPosture
    {
        Posture.Posture[] postures;
        const double averageTolerance = 0.056F;
        const double puntualTolerance = 0.06F;
        private double[] jointScore = new double[20];

        [SetUp]
        public void TestInit()
        {
            postures = PostureLibrary.getPostureList();
        }

        [Test]
        public void TestPostureCompareTo_SamePosture()
        {
            Assert.AreEqual(postures[0].compareTo(postures[0], ref jointScore, puntualTolerance, averageTolerance), 0);
        }

        [Test]
        public void TestPostureCompareTo_DiffPosture()
        {
            Assert.GreaterOrEqual(postures[0].compareTo(postures[1], ref jointScore, puntualTolerance, averageTolerance), 1);
        }

        [Test]
        public void TestPostureCompareTo_ClosePosture()
        {
            Posture.Posture p1 = PostureLibrary.loadPosture("./postures/T_JOSE");
            Posture.Posture p2 = PostureLibrary.loadPosture("./postures/T_JOSE2");
            Assert.Less(p1.compareTo(p2, ref jointScore, puntualTolerance, averageTolerance), 1);
        }
        
        [Test]
        public void TestCastSkeletonToJoins()
        {
            Skeleton skeleton = new Skeleton();
            Vector3[] joints = Posture.Posture.castSkeletonToJoints(skeleton);
            Boolean areEquals = true;
            for (int i = 0; i < 20; i++)
            {
                if ((joints[i].X != skeleton.Joints[(JointType)i].Position.X) ||
                    (joints[i].Y != skeleton.Joints[(JointType)i].Position.Y) ||
                    (joints[i].Z != skeleton.Joints[(JointType)i].Position.Z))
                {
                    areEquals = false;
                    break;
                }
            }

            Assert.IsTrue(areEquals);
        }

        [Test]
        public void Todo()
        {
            Skeleton skeleton = new Skeleton();
            Vector3[] joints = Posture.Posture.castSkeletonToJoints(skeleton);
            Boolean areEquals = true;
            for (int i = 0; i < 20; i++)
            {
                if ((joints[i].X != skeleton.Joints[(JointType)i].Position.X) ||
                    (joints[i].Y != skeleton.Joints[(JointType)i].Position.Y) ||
                    (joints[i].Z != skeleton.Joints[(JointType)i].Position.Z))
                {
                    areEquals = false;
                    break;
                }
            }

            Assert.IsTrue(areEquals);
        }

    }
}