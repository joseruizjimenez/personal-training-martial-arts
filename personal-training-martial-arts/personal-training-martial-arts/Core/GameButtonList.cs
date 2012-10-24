using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace personal_training_martial_arts.Core
{
    static class GameButtonList
    {
        public static const int BUTTON_HEIGHT = 40;
        public static const int BUTTON_WIDTH = 88;

        public static enum menuButton
        {
            PLAY,
            EXIT
        }

        public static enum scoreButton
        {
            MENU,
            REPLAY,
            NEXT
        }

        public static enum gameButton
        {
            PAUSE
        }

        public static enum pauseButton
        {
            CONTINUE,
            REPLAY,
            EXIT
        }

        public static int getTotalNumber()
        {
            return Enum.GetNames(typeof(menuButton)).Length +
                Enum.GetNames(typeof(scoreButton)).Length +
                Enum.GetNames(typeof(gameButton)).Length +
                Enum.GetNames(typeof(pauseButton)).Length;
        }

        public static int getMenuNumber()
        {
            return Enum.GetNames(typeof(menuButton)).Length;
        }

        public static int getScoreNumber()
        {
            return Enum.GetNames(typeof(scoreButton)).Length;
        }

        public static int getGameNumber()
        {
            return Enum.GetNames(typeof(gameButton)).Length;
        }

        public static int getPauseNumber()
        {
            return Enum.GetNames(typeof(pauseButton)).Length;
        }

    }
}
