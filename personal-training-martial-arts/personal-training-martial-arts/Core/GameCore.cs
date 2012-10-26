using System;
using System.Diagnostics;
using System.Threading;
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
using personal_training_martial_arts.Graphics;

namespace personal_training_martial_arts.Core
{
    class GameCore
    {
        // ESTADOS DEL JUEGO Y PANTALLAS
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
            PAUSE,
            SCORE,
            FINAL_SCORE,
            END
        }

        private screenState currentScreenState, nextScreenState;
        private playState currentPlayState, nextPlayState;
        private const int WINDOW_WIDTH = 640;
        private const int WINDOW_HEIGHT = 480;
        public GameScreen gameScreen;

        // POSTURAS Y ESQUELTO
        private Skeleton playerSkeleton;
        private PostureInformation[] gamePostures;
        private int gamePosturesIndex;
        private Dictionary<PostureInformation, float> gameScores;

        // TOLERANCIAS Y NIVEL
        private const float averageTolerance = 0.5F;
        private const float puntualTolerance = 0.5F;

        // BOTONES
        private Button[] menuButtons;
        private Button[] scoreButtons;
        private Button[] gameButtons;
        private Button[] pauseButtons;

        // TEMPORIZADORES
        private Stopwatch drawPostureTimeOut;
        private Stopwatch scoreTimeOut;

        public ContentHandler ch { get; set; }

        public GameCore(KinectSensor sensor, GraphicsDevice graphicsDevice)
        {
            this.gameScreen = new GameScreen(sensor, graphicsDevice);
            this.nextScreenState = screenState.INIT;
            this.nextPlayState = playState.INIT;
            this.playerSkeleton = null;
            this.gamePostures = null;
            this.gameScores = new Dictionary<PostureInformation, float>();
            this.menuButtons = new Button[GameButtonList.getMenuNumber()];
            this.scoreButtons = new Button[GameButtonList.getScoreNumber()];
            this.gameButtons = new Button[GameButtonList.getGameNumber()];
            this.pauseButtons = new Button[GameButtonList.getPauseNumber()];
            this.drawPostureTimeOut = Stopwatch.StartNew();
            this.scoreTimeOut = Stopwatch.StartNew();

            // Crea los botones del juego
            // por ahora les asignamos su posicion en pantalla con esos metodos...
            initializeMenuButtons(this.menuButtons);
            initializeScoreButtons(this.scoreButtons);
            initializeGameButtons(this.gameButtons);
            initializePauseButtons(this.pauseButtons);
        }

        /// <summary>
        /// Actualiza la postura actual del jugador.
        /// </summary>
        /// <param name="playerSkeleton">Postura con la que actualizar</param>
        public void updatePlayerSkeleton(Skeleton playerSkeleton)
        {
            this.playerSkeleton = playerSkeleton;
        }

        /// <summary>
        /// Lógica del programa.
        /// </summary>
        /// <returns>Si se sale del programa o no</returns>
        public Boolean update()
        {
            this.currentScreenState = this.nextScreenState;
            this.currentPlayState   = this.nextPlayState;

            switch (this.currentScreenState)
            {
                case screenState.INIT:
                    // algo en inicio?
                    this.nextScreenState = screenState.MENU;
                    break;

                case screenState.MENU:
                    updateButtonsState(menuButtons);
                    if (menuButtons[(int) GameButtonList.menuButton.PLAY].justPushed())
                    {
                        this.nextScreenState = screenState.PLAY;
                        this.nextPlayState = playState.INIT;
                    }
                    else if (menuButtons[(int) GameButtonList.menuButton.EXIT].justPushed())
                    {
                        // termina el juego
                        return false;
                    }
                    break;

                case screenState.PLAY:
                    switch (this.currentPlayState)
                    {
                        case playState.INIT:
                            // algo en inicio?
                            this.gamePostures = null;
                            this.gameScores.Clear();
                            this.nextPlayState = playState.SELECT_POSTURE;
                            break;

                        case playState.SELECT_POSTURE:
                            // actualiza la postura objetivo
                            if (updateCurrentGamePosture())
                            {
                                /// @FIX001:
                                this.drawPostureTimeOut = Stopwatch.StartNew();
                                this.nextPlayState = playState.DRAW_POSTURE;
                            }
                            // si no quedan mas posturas va a la puntuacion final
                            else
                            {
                                this.nextPlayState = playState.FINAL_SCORE;
                            }
                            break;

                        case playState.DRAW_POSTURE:
                            // Esta fase es para presentarle al usuario la postura objetivo
                            // TIMEOUT de 10 segundos o pulsar CONTINUE
                            
                            /// @FIX001: Estaba a null el this.drawPostureTimeOut, asique le he puesto valor en SELECT_POSTURE (Margen de error: 1frame).
                            updateButtonsState(pauseButtons);
                            if (pauseButtons[(int) GameButtonList.pauseButton.CONTINUE].justPushed() || isTimedOut(this.drawPostureTimeOut, 10))
                                this.nextPlayState = playState.DETECT_POSTURE;
                            break;

                        case playState.DETECT_POSTURE:
                            updateButtonsState(gameButtons);
                            if (gameButtons[(int)GameButtonList.gameButton.PAUSE].justPushed())
                                this.nextPlayState = playState.PAUSE;
                            else
                            {
                                float score = 0F;
                                if (playerSkeleton != null)
                                    score = ratePosture(new Posture.Posture(playerSkeleton), gamePostures[gamePosturesIndex]);

                                if (score >= 5F)
                                {
                                    gameScores.Add(gamePostures[gamePosturesIndex], score);
                                    this.scoreTimeOut = Stopwatch.StartNew();
                                    this.nextPlayState = playState.SCORE;
                                }
                            }
                            break;

                        case playState.PAUSE:
                            updateButtonsState(pauseButtons);
                            if (pauseButtons[(int) GameButtonList.pauseButton.CONTINUE].justPushed())
                                this.nextPlayState = playState.DETECT_POSTURE;
                            else if (pauseButtons[(int) GameButtonList.pauseButton.REPLAY].justPushed())
                                this.nextPlayState = playState.INIT;
                            else if (pauseButtons[(int) GameButtonList.pauseButton.EXIT].justPushed())
                                this.nextPlayState = playState.END;
                            break;

                        case playState.SCORE:
                            updateButtonsState(scoreButtons);
                            // TIMEOUT de 10 segundos a la siguiente postura o se pulsa alguna opcion
                            if (scoreButtons[(int)GameButtonList.scoreButton.NEXT].justPushed() ||
                                isTimedOut(this.scoreTimeOut, 10))
                                this.nextPlayState = playState.DETECT_POSTURE;
                            else if (scoreButtons[(int)GameButtonList.scoreButton.MENU].justPushed())
                                this.nextPlayState = playState.END;
                            else if (scoreButtons[(int)GameButtonList.scoreButton.REPLAY].justPushed())
                                this.nextPlayState = playState.INIT;
                            break;

                        case playState.FINAL_SCORE:
                            updateButtonsState(scoreButtons);
                            if (scoreButtons[(int)GameButtonList.scoreButton.MENU].justPushed())
                                this.nextPlayState = playState.END;
                            else if (scoreButtons[(int)GameButtonList.scoreButton.REPLAY].justPushed())
                                this.nextPlayState = playState.INIT;
                            break;

                        default:
                        case playState.END:
                            this.nextScreenState = screenState.MENU;
                            break;
                    }
                    break;

                default:
                case screenState.END:
                    return false; // Se avisa de que el programa dejara de actualizarse
            }

            return true; // Si no esta en estado de END se continua con la ejecucion
        }

        /// <summary>
        /// Pinta el resultado de la lógica por pantalla.
        /// </summary>
        public void draw(Texture2D kinectRGBVideo)
        {
            this.gameScreen.clearAll();
            // movida
            if (this.currentScreenState == screenState.MENU)
            {
                foreach (Button b in menuButtons)
                {
                    this.gameScreen.layerAdd((Texture2D) this.ch.get(b.name), b.rectangle, Color.White);
                }
            }
            else if (this.currentScreenState == screenState.PLAY)
            {
                if (this.currentPlayState == playState.DRAW_POSTURE)
                {
                    this.gameScreen.postureAdd(this.gamePostures[this.gamePosturesIndex], (Texture2D) this.ch.get("joint"));
                    Button b = pauseButtons[(int) GameButtonList.pauseButton.CONTINUE];
                    this.gameScreen.layerAdd((Texture2D)this.ch.get(b.name), b.rectangle, Color.White);
                }
                else if (this.currentPlayState == playState.DETECT_POSTURE)
                {
                    this.gameScreen.layerAdd(kinectRGBVideo, new Rectangle(0, 0, 640, 480), Color.White);
                    if(playerSkeleton != null)
                        this.gameScreen.skeletonAdd(playerSkeleton, (Texture2D)this.ch.get("joint"));

                    foreach (Button b in gameButtons)
                    {
                        this.gameScreen.layerAdd((Texture2D)this.ch.get(b.name), b.rectangle, Color.White);
                    }
                }
                else if (this.currentPlayState == playState.PAUSE)
                {
                }
                else if (this.currentPlayState == playState.SCORE)
                {
                }
                else if (this.currentPlayState == playState.FINAL_SCORE)
                {
                }
                else
                {
                    // Loading...
                } // End if playState
            } // End if screenState

            this.gameScreen.drawAll();
        }
        
        /// <summary>
        /// Actualiza la postura actual. Si no hay posturas, las carga.
        /// False si no hay mas posturas que actualizar
        /// </summary>
        private Boolean updateCurrentGamePosture()
        {
            // Se piden las posturas a PostureLibrary, se randomiza y se selecciona la primera
            // sino, se avanza a la siguiente...
            if (this.gamePostures == null)
            {
                this.gamePostures = PostureLibrary.getPostureList();
                this.shufflePostures(gamePostures);
                this.gamePosturesIndex = 0;
                return true;
            }
            // si no hay mas posturas que sacar se termina el juego
            else if (this.gamePosturesIndex == gamePostures.Length - 1)
            {
                this.gamePostures = null;
                return false;
            }
            // sino se avanza la postura
            else
            {
                this.gamePosturesIndex++;
                return true;
            }
        }

        /// ?? Esto iría mejor en Posture.Posture (static) ??
        /// <summary>
        /// Mezcla un array de <code>Posture</code>.
        /// </summary>
        /// <param name="postures">Posturas a mezclar</param>
        private void shufflePostures(PostureInformation[] postures)
        {
            for (int t = 0; t < postures.Length; t++ )
            {
                PostureInformation tmp = postures[t];
                Random rr = new Random();
                int r = rr.Next(t, postures.Length);
                postures[t] = postures[r];
                postures[r] = tmp;
            }
        }

        /// <summary>
        /// Comprueba si se ha cumplido un timeout.
        /// </summary>
        /// <param name="startTime">DateTime de inicio</param>
        /// <param name="secondsToTimeOut">Segundos para timeout</param>
        /// <returns>Si los segundos se han pasado o no</returns>
        private Boolean isTimedOut(Stopwatch sw, int secondsToTimeOut)
        {
            /*DateTime endTime = startTime.AddSeconds((double) secondsToTimeOut);
            DateTime now = DateTime.Now;

            if (DateTime.Compare(endTime, now) >= 0)
                return true;

            return false;*/
            TimeSpan maxDuration = TimeSpan.FromSeconds(secondsToTimeOut);

            if (sw.Elapsed < maxDuration)
            {
                return false;
            }
            return true;
        }

        // Los pinta centrados en vertical
        private static void initializeMenuButtons(Button[] buttons)
        {
            int x = WINDOW_WIDTH / 2 - GameButtonList.BUTTON_WIDTH / 2;
            int y = WINDOW_HEIGHT / 2 - GameButtonList.getMenuNumber() / 2 * GameButtonList.BUTTON_HEIGHT -
                (GameButtonList.getMenuNumber() % 2) * GameButtonList.BUTTON_HEIGHT / 2;
            foreach (GameButtonList.menuButton b in Enum.GetValues(typeof(GameButtonList.menuButton)))
            {
                buttons[(int)b] = new Button(b.ToString(), x, y, GameButtonList.BUTTON_WIDTH, GameButtonList.BUTTON_HEIGHT);
                y += GameButtonList.BUTTON_HEIGHT;
            }
        }

        // Los pinta abajo centrados en linea
        private static void initializeScoreButtons(Button[] buttons)
        {
            int x = WINDOW_WIDTH / 2 - GameButtonList.getScoreNumber() / 2 * GameButtonList.BUTTON_WIDTH -
                (GameButtonList.getScoreNumber() % 2) * GameButtonList.BUTTON_WIDTH / 2;
            int y = WINDOW_HEIGHT / 2 - GameButtonList.BUTTON_HEIGHT / 2;
            foreach (GameButtonList.scoreButton b in Enum.GetValues(typeof(GameButtonList.scoreButton)))
            {
                buttons[(int)b] = new Button(b.ToString(), x, y, GameButtonList.BUTTON_WIDTH, GameButtonList.BUTTON_HEIGHT);
                x += GameButtonList.BUTTON_WIDTH;
            }
        }

        // Los pinta abajo a la derecha en vertical
        private static void initializeGameButtons(Button[] buttons)
        {
            int x = WINDOW_WIDTH - GameButtonList.BUTTON_WIDTH - 20;
            int y = WINDOW_HEIGHT - GameButtonList.getGameNumber() / 2 * GameButtonList.BUTTON_HEIGHT -
                (GameButtonList.getGameNumber() % 2) * GameButtonList.BUTTON_HEIGHT / 2 - 20;
            foreach (GameButtonList.gameButton b in Enum.GetValues(typeof(GameButtonList.gameButton)))
            {
                buttons[(int) b] = new Button(b.ToString(), x, y, GameButtonList.BUTTON_WIDTH, GameButtonList.BUTTON_HEIGHT);
                y += GameButtonList.BUTTON_HEIGHT;
            }
        }

        // Los pinta abajo centrados en linea
        private static void initializePauseButtons(Button[] buttons)
        {
            int x = WINDOW_WIDTH / 2 - GameButtonList.getPauseNumber() / 2 * GameButtonList.BUTTON_WIDTH -
                (GameButtonList.getPauseNumber() % 2) * GameButtonList.BUTTON_WIDTH / 2;
            int y = WINDOW_HEIGHT - GameButtonList.BUTTON_HEIGHT - 20;
            foreach (GameButtonList.pauseButton b in Enum.GetValues(typeof(GameButtonList.pauseButton)))
            {
                buttons[(int)b] = new Button(b.ToString(), x, y, GameButtonList.BUTTON_WIDTH, GameButtonList.BUTTON_HEIGHT);
                x += GameButtonList.BUTTON_WIDTH;
            }
        }

        private void updateButtonsState(Button[] buttons)
        {
            foreach (Button button in buttons)
                button.updateState();
        }

        // el compare to devolvera un float con una especie de puntuacion (de 0 a 10) en el futuro
        // estas se devuelven para registrarlas en el dicionario con las puntuaciones de cada ejercicio
        private float ratePosture(Posture.Posture player, Posture.Posture goal)
        {
            return 0F;
            if (player.compareTo(goal, averageTolerance, puntualTolerance))
                return 10F;
            else
                return 0F;
        }

        public void loadContentHandler(ContentHandler ch)
        {
            this.ch = ch;
            gameScreen.ch = ch;
        }

    }
}
