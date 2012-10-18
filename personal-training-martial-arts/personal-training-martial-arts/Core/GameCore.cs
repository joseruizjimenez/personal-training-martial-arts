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
using personal_training_martial_arts.Posture;

namespace personal_training_martial_arts.Core
{
    class GameCore
    {
        private enum screenState
        {
            INIT,
            MENU,
            PLAY,
            END
        }

        private enum playState
        {
            INIT,
            SELECT_POSTURE,
            DRAW_POSTURE,
            DETECT_POSTURE,
            SCORE,
            END
        }

        private screenState currentScreenState, nextScreenState;
        private playState currentPlayState, nextPlayState;
        private GameScreen gameScreen;

        private Posture.Posture userPosture;

        public GameCore()
        {
            gameScreen = new GameScreen();
            nextScreenState = screenState.INIT;
            nextPlayState = playState.INIT;
            userPosture = new Posture.Posture();
        }

        public void updateUserPosture(Posture.Posture userPosture)
        {
            this.userPosture = userPosture;
        }

        public Boolean update()
        {
            currentScreenState = nextScreenState;
            currentPlayState = nextPlayState;

            switch (currentScreenState)
            {
                case screenState.INIT:
                    break;

                case screenState.MENU:
                    break;

                case screenState.PLAY:
                    switch (currentPlayState)
                    {
                        case playState.INIT:
                            break;

                        case playState.SELECT_POSTURE:
                            break;

                        case playState.DRAW_POSTURE:
                            break;

                        case playState.DETECT_POSTURE:
                            break;

                        case playState.SCORE:
                            break;

                        default:
                        case playState.END:
                            break;
                    }
                    break;

                default:
                case screenState.END:                
                    return false;
            }

            return false; // finaliza el programa
        }

        public void draw()
        {
            // movida
            
        }
    }
}
