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
        XNAGame game;
        GameCore gameCore;
        XNACore xnaCore;

        [SetUp]
        public void TestInit()
        {
            game = new XNAGame();
            gameCore = new GameCore(null);
            xnaCore = new XNACore(new XNAGame());

            game.Run();
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
            setNotPlayerOnScreen();
            game.core.Update(null);

            //((Skeleton)this.r.get("Detectar postura").get("Profesor").drawable).posture = this.gamePostures[this.gamePosturesIndex];

            String actual_feedback = ((BorderedText)game.core.r.get("Detectar postura").get("Texto feedback").drawable).text;
            String expected_feedback = "¡Situate en la pantalla!";
            Assert.AreEqual(expected_feedback, actual_feedback);
        }

        private void setNotPlayerOnScreen()
        {
            game.core.currentPlayState = XNACore.playState.DETECT_POSTURE;
            game.core.nextPlayState = XNACore.playState.DETECT_POSTURE;
            game.core.currentScreenState = XNACore.screenState.PLAY;
            game.core.nextScreenState = XNACore.screenState.PLAY;

            game.core.kinect.skeleton = null;
        }

    }
}