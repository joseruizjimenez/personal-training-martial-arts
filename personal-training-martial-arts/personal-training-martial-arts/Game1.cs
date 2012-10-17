
/// Grabar posturas y trasladarlas a un fichero

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

namespace personal_training_martial_arts
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D kinectRGBVideo;
        Texture2D overlay;
        Texture2D hand;
        SpriteFont font;

        // Ejemplo con posicion de la mano. Se actualiza en KinectSensor_SkeletonFrameReady
        Vector2 handPosition = new Vector2();

        KinectSensor kinectSensor;

        string connectedStatus = "KINECT NOT DETECTED";

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Ajustamos la pantalla a la resolucion del kinect para copiar mapa 1:1
            graphics.PreferredBackBufferWidth = 640;
            graphics.PreferredBackBufferHeight = 480;
        }

        /// <summary>
        /// Detecta cuando se conecta o desconecta un sensor kinect. Lanza una busqueda si es necesario.
        /// Se emplea un eventHandler para lanzar la actualizacion, que se dirige con StatusChangedEventArgs
        /// </summary>
        void KinectSensors_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            if (this.kinectSensor == e.Sensor)
            {
                if (e.Status == KinectStatus.Disconnected ||
                    e.Status == KinectStatus.NotPowered)
                {
                    this.kinectSensor = null;
                    this.DiscoverKinectSensor();
                }
            }
        }

        /// <summary>
        /// Localiza nuevos dispositivos Kinect y lo asocia con nuestro kinectSensor
        /// </summary>
        private void DiscoverKinectSensor()
        {
            foreach (KinectSensor sensor in KinectSensor.KinectSensors)
            {
                if (sensor.Status == KinectStatus.Connected)
                {
                    // Encuentra un kinect y lo asigna a nuestro kinectSensor
                    kinectSensor = sensor;
                    break;
                }
            }

            // Si no ha encontrado ninguno:
            if (this.kinectSensor == null)
            {
                connectedStatus = "KINECT NOT DETECTED";
                return;
            }

            // Localizado el kinect, podemos especificar su estado concreto:
            switch (kinectSensor.Status)
            {
                case KinectStatus.Connected:
                    {
                        connectedStatus = "KINECT DETECTED";
                        break;
                    }
                case KinectStatus.Disconnected:
                    {
                        connectedStatus = "Status: Disconnected";
                        break;
                    }
                case KinectStatus.NotPowered:
                    {
                        connectedStatus = "Status: Connect the power";
                        break;
                    }
                default:
                    {
                        connectedStatus = "Status: Error";
                        break;
                    }
            }

            // Inicializa el kinect encontrado si esta operativo
            if (kinectSensor.Status == KinectStatus.Connected)
            {
                InitializeKinect();
            }
        }

        /// <summary>
        /// Activamos y configuramos la camara RGB y la deteccion del esqueleto en el Kinect
        /// </summary>
        private bool InitializeKinect()
        {
            // Color stream
            kinectSensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
            kinectSensor.ColorFrameReady += new EventHandler<ColorImageFrameReadyEventArgs>(kinectSensor_ColorFrameReady);

            // Skeleton Stream
            kinectSensor.SkeletonStream.Enable(new TransformSmoothParameters()
            {
                Smoothing = 0.5f,
                Correction = 0.5f,
                Prediction = 0.5f,
                JitterRadius = 0.05f,
                MaxDeviationRadius = 0.04f
            });
            kinectSensor.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(kinectSensor_SkeletonFrameReady);

            try
            {
                kinectSensor.Start();
            }
            catch
            {
                connectedStatus = "Unable to start the Kinect Sensor";
                return false;
            }
            return true;
        }

        /// <summary>
        /// Actualiza la posicion del esqueleto sobre nuestro vector
        /// </summary>
        void kinectSensor_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
            {
                if (skeletonFrame != null)
                {
                    Skeleton[] skeletonData = new Skeleton[skeletonFrame.SkeletonArrayLength];

                    skeletonFrame.CopySkeletonDataTo(skeletonData);
                    // selecciona el primer esqueleto detectado
                    Skeleton playerSkeleton = (from s in skeletonData where s.TrackingState == SkeletonTrackingState.Tracked select s).FirstOrDefault();
                    if (playerSkeleton != null)
                    {
                        // Aqui se seleccionan las articulaciones con las que trabajamos
                        // (Y podrias grabar el frame del esqueleto. Creo que playerSkeleton contiene el frame del esqueleto activo 1)
                        // y se actualizan las posiciones del punto concreto que necesitamos, por ejemplo la mano:
                        Joint rightHand = playerSkeleton.Joints[JointType.HandRight];
                        handPosition = new Vector2((((0.5f * rightHand.Position.X) + 0.5f) * (640)), (((-0.5f * rightHand.Position.Y) + 0.5f) * (480)));
                    }
                }
            }
        }

        /// <summary>
        /// Actualiza los datos recibidos de la camara sobre nuestro kinectRGBVideo
        /// </summary>
        void kinectSensor_ColorFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {
            using (ColorImageFrame colorImageFrame = e.OpenColorImageFrame())
            {
                if (colorImageFrame != null)
                {

                    byte[] pixelsFromFrame = new byte[colorImageFrame.PixelDataLength];

                    colorImageFrame.CopyPixelDataTo(pixelsFromFrame);

                    Color[] color = new Color[colorImageFrame.Height * colorImageFrame.Width];
                    kinectRGBVideo = new Texture2D(graphics.GraphicsDevice, colorImageFrame.Width, colorImageFrame.Height);

                    // Recorre los pixels y asigna el byte adecuado a cada punto
                    // El indice se incrementa de 4 en 4 porque hay 3 colores: red, green, blue
                    // El bytemap pixelsFromFrame se compone de un array unidimensional
                    int index = 0;
                    for (int y = 0; y < colorImageFrame.Height; y++)
                    {
                        for (int x = 0; x < colorImageFrame.Width; x++, index += 4)
                        {
                            color[y * colorImageFrame.Width + x] = new Color(pixelsFromFrame[index + 2], pixelsFromFrame[index + 1], pixelsFromFrame[index + 0]);
                        }
                    }

                    // Actualizamos los datos de los pixels del ColorImageFrame a nuestra Texture2D
                    kinectRGBVideo.SetData(color);
                }
            }
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // En la inicializacion del proyecto XNA lanzamos la busqueda del Kinect cuando
            // se produce un nuevo evento en StatusChangedEventArgs
            KinectSensor.KinectSensors.StatusChanged += new EventHandler<StatusChangedEventArgs>(KinectSensors_StatusChanged);
            DiscoverKinectSensor();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            kinectRGBVideo = new Texture2D(GraphicsDevice, 1337, 1337);

            overlay = Content.Load<Texture2D>("overlay");
            hand = Content.Load<Texture2D>("hand");
            font = Content.Load<SpriteFont>("SpriteFont1");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            kinectSensor.Stop();
            kinectSensor.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            // Por ahora no tenemos logica que actualizar en el juego.
            // La deteccion del kinect se realiza con su propio EventHandler y la pantalla se pinta con Draw.

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            // Pintamos el video de Kinect
            spriteBatch.Draw(kinectRGBVideo, new Rectangle(0, 0, 640, 480), Color.White);
            // En el ejemplo de la mano pintaremos un punto blanco:
            //spriteBatch.Draw(hand, handPosition, Color.White);
            // Capa overlay  intermedia sobre la que pintar las letras
            //spriteBatch.Draw(overlay, new Rectangle(0, 0, 640, 480), Color.White);
            // Pintamos el estado del Kinect
            spriteBatch.DrawString(font, connectedStatus, new Vector2(20, 80), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }


        // Metodos accesibles para los tests... probando el mocking
        // Hace publico el estado del dispositivo Kinect para emplearlo en los test
        public String GetConnectedStatus()
        {
            return connectedStatus;
        }

        // Stub para los test
        public void InitializeStub()
        {
            this.Initialize();
        }
    }
}
