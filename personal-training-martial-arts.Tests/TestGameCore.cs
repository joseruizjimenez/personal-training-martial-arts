using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NMock2;
using Microsoft.Kinect;
using personal_training_martial_arts.Core;

namespace personal_training_martial_arts.Test
{
    [TestFixture]
    public class TestGameCore
    {
        GameCore gameCore;

        [SetUp]
        public void TestInit()
        {
            gameCore = new GameCore(null);         
        }

        [TearDown]
        public void TestEnd()
        {
            gameCore = null;
        }

        [Test]
        public void TestIsTimedOut_True()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Assert.IsTrue(GameCore.isTimedOut(sw, 0));
        }

        [Test]
        public void TestIsTimedOut_False()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Assert.IsFalse(GameCore.isTimedOut(sw, 99));
        }

        [Test]
        public void TestUpdate_StartAtInit()
        {
            Assert.IsTrue(gameCore.update());
        }

    }
}