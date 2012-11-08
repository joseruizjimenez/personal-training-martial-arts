using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NMock2;
using Microsoft.Kinect;

namespace personal_training_martial_arts.Tests
{
    [TestFixture]
    public class TestGame1
    {
        Mockery mocks;
        KinectSensor kinectSensorMock;
        Game1 game;

        [SetUp]
        public void TestInit()
        {
            mocks = new Mockery();
            game = new Game1();
            game.InitializeToStub();
            kinectSensorMock = game.getKinectSensorToStub();
            //kinectSensorMock = mocks.NewMock<IKinectSensor>();            
        }

        [TearDown]
        public void TestEnd()
        {
            mocks = null;
            game = null;
            kinectSensorMock = null;
        }


        [Test]
        public void TestDiscoverKinectSensor_NotDetected()
        {
            //Expect.Once.On(kinectSensorMock).GetProperty("Status").Will(Return.Value(null));
            //mocks.VerifyAllExpectationsHaveBeenMet();
            //kinectSensorMock.Stop();
            Assert.AreEqual("KINECT NOT DETECTED", game.GetConnectedStatus());
        }

        // Para poder testear el Game1 correctamente tenemos que implementar una interfaz para el
        // Kinect y eso es lo que usará el Game1. Solo se pueden crear Mocks de interfaces, y el
        // framework de MS no nos lo da. [TODO]
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
