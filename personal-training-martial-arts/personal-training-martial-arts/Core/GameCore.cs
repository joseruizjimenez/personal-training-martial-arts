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
        private List<Posture.Posture> gamePostures;
        private Dictionary<Posture.Posture, Boolean> gameProgress;

        public GameCore()
        {
            gameScreen = new GameScreen();
            nextScreenState = screenState.INIT;
            nextPlayState = playState.INIT;
            userPosture = null;
            gamePostures = null;
            gamePosturesProgress = null;
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
                    // algo en inicio?
                    nextScreenState = screenState.MENU;
                    break;

                case screenState.MENU:
                    // ver si se ha pulsado una opcion y en ese caso pasar a PLAY (y el nextPlayState a INIT)
                    // o si END (se ha pulsado salir)
                    // sino se mantiene en ese estado (nextPlayState = playState.MENU)
                    break;

                case screenState.PLAY:
                    switch (currentPlayState)
                    {
                        case playState.INIT:
                            // algo en inicio?
                            nextPlayState = playState.SELECT_POSTURE;
                            break;

                        case playState.SELECT_POSTURE:
                            // Si selected_posture_index es -1:
                            // Se piden las posturas a PostureLibrary, se randomiza y se ejecuta la primera
                            // sino, se avanza a la siguiente...
                            if(gamePostures == null) 
                            {
                                gamePosturesProgress = new Dictionary<Posture.Posture, bool>();
                                gamePostures = new SortedSet<Posture.Posture>(PostureLibrary.getPostureList());
                                gamePostures.
                                foreach (Posture.Posture p in gamePostures)
                                {
                                    gamePostures
                                }
                            }
                            break;

                        case playState.DRAW_POSTURE:
                            break;

                        case playState.DETECT_POSTURE:
                            break;

                        case playState.SCORE:
                            // se comprueba si se ha pulsado repetir (nextPlayState = INIT)
                            // o menu (nextPlayState = END)
                            break;

                        default:
                        case playState.END:
                            gamePostures.Clear();
                            nextScreenState = screenState.MENU;
                            break;
                    }
                    break;

                default:
                case screenState.END:
                    return false; // Se avisa de que el programa dejara de actualizarse
            }

            return true; // Si no esta en estado de END se continua con la ejecucion
        }

        public void draw()
        {
            // movida
            
        }
    }
}
