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
using personal_training_martial_arts.Graphics;

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

        private Skeleton playerSkeleton;
        private Posture.Posture[] gamePostures;
        private int gamePosturesIndex;
        private Dictionary<Posture.Posture, float> gameScores;

        private DateTime drawPostureTimeOut;

        public GameCore(KinectSensor sensor, GraphicsDevice graphicsDevice)
        {
            this.gameScreen = new GameScreen(sensor, graphicsDevice);
            this.nextScreenState = screenState.INIT;
            this.nextPlayState = playState.INIT;
            this.playerSkeleton = null;
            this.gamePostures = null;
            this.gameScores = new Dictionary<Posture.Posture, float>();
            this.drawPostureTimeOut = new DateTime(0);
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
                    // ver si se ha pulsado una opcion y en ese caso pasar a PLAY (y el nextPlayState a INIT)
                    // o si END (se ha pulsado salir)
                    // sino se mantiene en ese estado (nextPlayState = playState.MENU)
                    break;

                case screenState.PLAY:
                    switch (this.currentPlayState)
                    {
                        case playState.INIT:
                            // algo en inicio?
                            this.gamePostures = null;
                            this.nextPlayState = playState.SELECT_POSTURE;
                            break;

                        case playState.SELECT_POSTURE:
                            this.updateCurrentGamePosture();
                            /// @FIX001:
                            this.drawPostureTimeOut = DateTime.Now;
                            this.nextPlayState = playState.DRAW_POSTURE;
                            break;

                        case playState.DRAW_POSTURE:
                            // Esta fase es para presentarle al usuario la postura objetivo
                            // la pantalla se pintará en la llamada a Draw como siempre,
                            // en el update se mira que pase un tiempo determinado o se pulse un botón
                            // antes de pasar a la siguiente fase
                            
                            /// @FIX001: Estaba a null el this.drawPostureTimeOut, asique le he puesto valor en SELECT_POSTURE (Margen de error: 1frame).
                            if(this.isTimedOut(this.drawPostureTimeOut, 30))
                                this.nextPlayState = playState.DETECT_POSTURE;
                            break;

                        case playState.DETECT_POSTURE:
                            break;

                        case playState.SCORE:
                            // se comprueba si se ha pulsado SIGUIENTE (nextPlayState = INIT)
                            // o menu (nextPlayState = END)
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
        public void draw()
        {
            // movida           
        }
        
        /// <summary>
        /// Actualiza la postura actual. Si no hay posturas, las carga.
        /// </summary>
        private void updateCurrentGamePosture()
        {
            // Se piden las posturas a PostureLibrary, se randomiza y se selecciona la primera
            // sino, se avanza a la siguiente...
            if (this.gamePostures == null)
            {
                this.gamePostures = PostureLibrary.getPostureList();
                this.shufflePostures(gamePostures);
                this.gameScores.Clear();
                this.gamePosturesIndex = 0;
            }
            else
                this.gamePosturesIndex++;
        }

        /// ?? Esto iría mejor en Posture.Posture (static) ??
        /// <summary>
        /// Mezcla un array de <code>Posture</code>.
        /// </summary>
        /// <param name="postures">Posturas a mezclar</param>
        private void shufflePostures(Posture.Posture[] postures)
        {
            for (int t = 0; t < postures.Length; t++ )
            {
                Posture.Posture tmp = postures[t];
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
        private Boolean isTimedOut(DateTime startTime, int secondsToTimeOut)
        {
            DateTime endTime = startTime.AddSeconds((double) secondsToTimeOut);
            DateTime now = DateTime.Now;

            if (DateTime.Compare(endTime, now) >= 0)
                return true;

            return false;
        }
    }
}
