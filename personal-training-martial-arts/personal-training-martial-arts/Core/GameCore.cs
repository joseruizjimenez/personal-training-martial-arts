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
            HOLD_POSTURE,
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
        private Dictionary<PostureInformation, double> gameScores;

        // NIVEL NORMAL para modificar usar metodo chDificultyLevel(int);
        private float averageTolerance = 0.056F;
        private float puntualTolerance = 0.06F;

        private double[] jointScore = new double[20];
        private double score;

        // BOTONES
        private Button[] menuButtons;
        private Button[] scoreButtons;
        private Button[] gameButtons;
        private Button[] pauseButtons;

        // TEMPORIZADORES (tiempos en segundos)
        private Stopwatch drawPostureTimeOut;
        private Stopwatch holdPostureTimeOut;
        private Stopwatch scoreTimeOut;
        private const int DRAW_POSTURE_TIME = 10;
        private const int HOLD_POSTURE_TIME = 3;
        private const int SCORE_TIME = 10;

        public ContentHandler ch { get; set; }

        public GameCore(GraphicsDevice graphicsDevice)
        {
            this.gameScreen = new GameScreen(graphicsDevice);
            this.nextScreenState = screenState.INIT;
            this.nextPlayState = playState.INIT;
            this.playerSkeleton = null;
            this.gamePostures = null;
            this.gameScores = new Dictionary<PostureInformation, double>();
            this.menuButtons = new Button[GameButtonList.getMenuNumber()];
            this.scoreButtons = new Button[GameButtonList.getScoreNumber()];
            this.gameButtons = new Button[GameButtonList.getGameNumber()];
            this.pauseButtons = new Button[GameButtonList.getPauseNumber()];
            this.drawPostureTimeOut = Stopwatch.StartNew();
            this.holdPostureTimeOut = Stopwatch.StartNew();
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

        private void chDificultyLevel(int level)
        {
            switch (level){
                case 0:         
                    // NIVEL MUY FACIL.
                    this.averageTolerance = 0.1F;
                    this.puntualTolerance = 0.1F;
                    break;
                case 1:
                    // NIVEL FACIL.
                    this.averageTolerance = 0.05F;
                    this.puntualTolerance = 0.08F;
                    break;
                case 2:
                    // NIVEL MEDIO
                    this.averageTolerance = 0.05F;
                    this.puntualTolerance = 0.06F;
                    break;
                case 3:
                    // NIVEL DIFICIL
                    this.averageTolerance = 0.05F;
                    this.puntualTolerance = 0.05F;
                    break;
                case 4:
                    // NIVEL KATANA (Like a boss)
                    this.averageTolerance = 0.04F;
                    this.puntualTolerance = 0.05F;
                    break;
                default:
                    this.averageTolerance = 0.05F;
                    this.puntualTolerance = 0.06F;
                    break;
            }
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
                    updateButtonsState(this.menuButtons);
                    if (this.menuButtons[(int)GameButtonList.menuButton.PLAY].justPushed())
                    {
                        this.nextScreenState = screenState.PLAY;
                        this.nextPlayState = playState.INIT;
                    }
                    else if (this.menuButtons[(int)GameButtonList.menuButton.EXIT].justPushed())
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
                            updateButtonsState(this.pauseButtons);
                            if (this.pauseButtons[(int)GameButtonList.pauseButton.CONTINUE].justPushed() ||
                                isTimedOut(this.drawPostureTimeOut, DRAW_POSTURE_TIME))
                            {
                                this.drawPostureTimeOut.Reset();
                                this.nextPlayState = playState.DETECT_POSTURE;
                            }
                            break;

                        case playState.DETECT_POSTURE:
                            updateButtonsState(this.gameButtons);
                            if (this.gameButtons[(int)GameButtonList.gameButton.PAUSE].justPushed())
                                this.nextPlayState = playState.PAUSE;
                            else
                            {
                                if (playerSkeleton != null)
                                {
                                    Posture.Posture p = new Posture.Posture(playerSkeleton);
                                    score = p.compareTo(gamePostures[gamePosturesIndex], ref jointScore, averageTolerance, puntualTolerance);
                                    if (score < 1.0)
                                    {
                                        this.holdPostureTimeOut = Stopwatch.StartNew();
                                        this.nextPlayState = playState.HOLD_POSTURE;
                                    }
                                }
                            }
                            break;

                        case playState.HOLD_POSTURE:
                            updateButtonsState(this.gameButtons);
                            if (this.gameButtons[(int)GameButtonList.gameButton.PAUSE].justPushed())
                                this.nextPlayState = playState.PAUSE;
                            else
                            {
                                if (playerSkeleton != null)
                                {
                                    Posture.Posture p = new Posture.Posture(playerSkeleton);
                                    score = p.compareTo(gamePostures[gamePosturesIndex], ref jointScore, averageTolerance, puntualTolerance);                                   
                                    if (score < 1.0)
                                    {
                                        // La postura hay que mantenerla 2 segundos (HOLD_POSTURE_TIME)
                                        if (isTimedOut(this.holdPostureTimeOut, HOLD_POSTURE_TIME))
                                        {
                                            gameScores.Add(gamePostures[gamePosturesIndex], score);
                                            this.holdPostureTimeOut.Reset();
                                            this.scoreTimeOut = Stopwatch.StartNew();
                                            this.nextPlayState = playState.SCORE;
                                        }
                                    }
                                    else
                                    {
                                        this.holdPostureTimeOut.Reset();
                                        this.nextPlayState = playState.DETECT_POSTURE;
                                    }
                                }
                            }
                            break;

                        case playState.PAUSE:
                            updateButtonsState(this.pauseButtons);
                            if (this.pauseButtons[(int)GameButtonList.pauseButton.CONTINUE].justPushed())
                                this.nextPlayState = playState.DETECT_POSTURE;
                            else if (this.pauseButtons[(int)GameButtonList.pauseButton.REPLAY].justPushed())
                                this.nextPlayState = playState.INIT;
                            else if (this.pauseButtons[(int)GameButtonList.pauseButton.EXIT].justPushed())
                                this.nextPlayState = playState.END;
                            break;

                        case playState.SCORE:
                            updateButtonsState(this.scoreButtons);
                            // TIMEOUT de 10 segundos a la siguiente postura o se pulsa alguna opcion
                            if (this.scoreButtons[(int)GameButtonList.scoreButton.NEXT].justPushed() ||
                                isTimedOut(this.scoreTimeOut, SCORE_TIME))
                            {
                                this.scoreTimeOut.Reset();
                                this.nextPlayState = playState.DETECT_POSTURE;
                            }
                            else if (this.scoreButtons[(int)GameButtonList.scoreButton.MENU].justPushed())
                            {
                                this.scoreTimeOut.Reset();
                                this.nextPlayState = playState.END;
                            }
                            else if (this.scoreButtons[(int)GameButtonList.scoreButton.REPLAY].justPushed())
                            {
                                this.scoreTimeOut.Reset();
                                this.nextPlayState = playState.INIT;
                            }
                            break;

                        case playState.FINAL_SCORE:
                            updateButtonsState(this.scoreButtons);
                            if (this.scoreButtons[(int)GameButtonList.scoreButton.MENU].justPushed())
                                this.nextPlayState = playState.END;
                            else if (this.scoreButtons[(int)GameButtonList.scoreButton.REPLAY].justPushed())
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
        public void draw(Texture2D kinectRGBVideo, String connectedStatus)
        {
            this.gameScreen.clearAll();
            if (connectedStatus == "KINECT NOT DETECTED")
                this.gameScreen.textAdd((SpriteFont)this.ch.get("defaultFont"), connectedStatus, new Vector2(150, 80), Color.Tomato);
            if (this.currentScreenState == screenState.MENU)
            {
                if (connectedStatus == "KINECT DETECTED")
                    this.gameScreen.textAdd((SpriteFont)this.ch.get("defaultFont"), connectedStatus, new Vector2(150, 80), Color.LightGreen);
                foreach (Button b in this.menuButtons)
                {
                    this.gameScreen.layerAdd((Texture2D) this.ch.get(b.name), b.rectangle, Color.White);
                }
            }
            else if (this.currentScreenState == screenState.PLAY)
            {
                if (this.currentPlayState == playState.DRAW_POSTURE)
                {
                    this.gameScreen.textAdd((SpriteFont) this.ch.get("defaultFont"), this.gamePostures[this.gamePosturesIndex].name,
                        new Vector2(150, 80), Color.SandyBrown);
                    this.gameScreen.postureAdd(this.gamePostures[this.gamePosturesIndex], (Texture2D) this.ch.get("joint"));
                    Button b = this.pauseButtons[(int)GameButtonList.pauseButton.CONTINUE];
                    this.gameScreen.layerAdd((Texture2D)this.ch.get(b.name), b.rectangle, Color.White);
                }
                else if (this.currentPlayState == playState.DETECT_POSTURE)
                {
                    //**Update**
                    this.gameScreen.backgroundAdd(kinectRGBVideo, new Rectangle(0, 0, 640, 480), Color.White);
                    // *BETA* TO-DO
                    // Postura de ayuda para complir la postura... mas adelante la pondremos escalada en peque
                    this.gameScreen.postureAdd(this.gamePostures[this.gamePosturesIndex], (Texture2D)this.ch.get("joint"));
                    if (playerSkeleton != null)
                    {
                        this.gameScreen.skeletonAdd(playerSkeleton, (Texture2D)this.ch.get("joint"), jointScore);
                    }

                    foreach (Button b in this.gameButtons)
                    {
                        this.gameScreen.layerAdd((Texture2D)this.ch.get(b.name), b.rectangle, Color.White);
                    }
                }
                else if (this.currentPlayState == playState.HOLD_POSTURE)
                {
                    this.gameScreen.backgroundAdd(kinectRGBVideo, new Rectangle(0, 0, 640, 480), Color.White);
                    // *BETA* TO-DO
                    // Postura de ayuda para complir la postura... mas adelante la pondremos escalada en peque
                    this.gameScreen.postureAdd(this.gamePostures[this.gamePosturesIndex], (Texture2D)this.ch.get("joint"));
                    this.gameScreen.textAdd((SpriteFont)this.ch.get("defaultFont"),
                        "GOOD! DON'T MOVE... " + (HOLD_POSTURE_TIME - this.holdPostureTimeOut.Elapsed.Seconds).ToString(),
                        new Vector2(150, 80), Color.LawnGreen);
                    if (playerSkeleton != null)
                    {
                        this.gameScreen.skeletonAdd(playerSkeleton, (Texture2D)this.ch.get("joint"), jointScore);
                    }

                    foreach (Button b in this.gameButtons)
                    {
                        this.gameScreen.layerAdd((Texture2D)this.ch.get(b.name), b.rectangle, Color.White);
                    }
                }
                else if (this.currentPlayState == playState.PAUSE)
                {
                    foreach (Button b in this.pauseButtons)
                    {
                        this.gameScreen.layerAdd((Texture2D)this.ch.get(b.name), b.rectangle, Color.White);
                    }
                }
                else if (this.currentPlayState == playState.SCORE)
                {
                    this.gameScreen.textAdd((SpriteFont)this.ch.get("defaultFont"), "POSTURE COMPLETED!",
                        new Vector2(150, 80), Color.LawnGreen);
                    foreach (Button b in this.scoreButtons)
                    {
                        this.gameScreen.layerAdd((Texture2D)this.ch.get(b.name), b.rectangle, Color.White);
                    }
                }
                else if (this.currentPlayState == playState.FINAL_SCORE)
                {
                    this.gameScreen.textAdd((SpriteFont)this.ch.get("defaultFont"), "CHALLENGE COMPLETED!",
                        new Vector2(150, 80), Color.LawnGreen);
                    foreach (Button b in this.scoreButtons)
                    {
                        if (GameButtonList.scoreButton.NEXT.ToString() != b.name)
                            this.gameScreen.layerAdd((Texture2D)this.ch.get(b.name), b.rectangle, Color.White);
                    }
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

        public void loadContentHandler(ContentHandler ch)
        {
            this.ch = ch;
            gameScreen.updateContentHandler(ch);
        }

        public void loadKinectSensor(KinectSensor sensor)
        {
            this.gameScreen.updateKinectSensor(sensor);
        }

    }
}
