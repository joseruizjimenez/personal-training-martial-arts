using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NUnit.Mocks;
using Microsoft.Kinect;

namespace personal_training_martial_arts.Tests
{
    [TestFixture]
    public class TestGame1
    {
        private DynamicMock kinectSensorMock;
        Game1 game;

        [SetUp]
        public void TestInit()
        {
            // Mock de los sensores Kinect
            kinectSensorMock = new DynamicMock(typeof(KinectSensor));
            game = new Game1();
        }

        [Test]
        public void TestDiscoverKinectSensor_NotDetected()
        {           
            kinectSensorMock.ExpectAndReturn("Status", KinectStatus.Disconnected);
            game.InitializeStub((KinectSensor) kinectSensorMock.MockInstance);

            Assert.AreEqual("KINECT NOT DETECTED", game.GetConnectedStatus());
        }

        /*
        [Test]
        public void TestDiscoverKinectSensor_Detected()
        {
            kinectSensorMock.ExpectAndReturn("Status", KinectStatus.Connected);
            game.InitializeStub((KinectSensor) kinectSensorMock.MockInstance);

            Assert.AreEqual("KINECT DETECTED", game.GetConnectedStatus());
        }
        */
    }
}
