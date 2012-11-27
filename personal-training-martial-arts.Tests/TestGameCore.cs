using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NMock2;
using Microsoft.Kinect;
using personal_training_martial_arts.Core;
using XNACore = XNAGraphics.KernelBundle.Core;
using XNAGame = XNAGraphics.Game1;
using XNAGraphics.ComponentBundle.DrawableBundle;

namespace personal_training_martial_arts.Test
{
    [TestFixture]
    public class TestGameCore
    {
        GameCore gameCore;
        XNACore xnaCore;

        [SetUp]
        public void TestInit()
        {
            gameCore = new GameCore(null);
            xnaCore = new XNACore(new XNAGame());
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

        [Test]
        public void TestPostureTextFeedback_NoSkeleton()
        {
            float out_of_range_score = 2;
            String actual_feedback = XNACore.getPostureTextFeedback(out_of_range_score);

            String expected_feedback = "¡Situate en la pantalla!";
            Assert.AreEqual(expected_feedback, actual_feedback);
        }

    }
}