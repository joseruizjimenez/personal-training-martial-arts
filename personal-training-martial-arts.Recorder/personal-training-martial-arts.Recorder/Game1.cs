
/// Creación del interfaz y detección de conexión y desconexión

using System;
using System.IO;
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

        // Global variables para menu
        enum BState
        {
            HOVER,
            UP,
            JUST_RELEASED,
            DOWN
        }
        const int NUMBER_OF_BUTTONS = 1,
            REC_BUTTON_INDEX = 0,
            BUTTON_HEIGHT = 480,
            BUTTON_WIDTH = 640;
        Color background_color;
        Color[] button_color = new Color[NUMBER_OF_BUTTONS];
        Rectangle[] button_rectangle = new Rectangle[NUMBER_OF_BUTTONS];
        BState[] button_state = new BState[NUMBER_OF_BUTTONS];
        Texture2D[] button_texture = new Texture2D[NUMBER_OF_BUTTONS];
        double[] button_timer = new double[NUMBER_OF_BUTTONS];
        //mouse pressed and mouse just pressed
        bool mpressed, prev_mpressed = false;
        //mouse location in window
        int mx, my;
        double frame_time;
        // fin menu

        Vector2[] jointPositions = new Vector2[20];

        Skeleton skeletonToRecord;

        KinectSensor kinectSensor;

        string connectedStatus = "KINECT NOT DETECTED";
        string recordStatus = "PUSH TO RECORD";

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
                kinectSensor.ElevationAngle = -5;
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
                        // Posicion para grabar
                        if (recordStatus == "PUSH TO RECORD")
                            skeletonToRecord = playerSkeleton;

                        // Aqui se seleccionan las articulaciones con las que trabajamos
                        // (Y podrias grabar el frame del esqueleto. Creo que playerSkeleton contiene el frame del esqueleto activo 1)
                        // y se actualizan las posiciones del punto concreto que necesitamos, por ejemplo la mano:
                        //Joint rightHand = playerSkeleton.Joints[JointType.HandRight];
                        int jointIndex = 0;
                        foreach(Joint joint in playerSkeleton.Joints) {
                            jointPositions[jointIndex] = new Vector2((((0.5f * joint.Position.X) + 0.5f) * (640)), (((-0.5f * joint.Position.Y) + 0.5f) * (480)));
                            jointIndex += 1;
                        }
                    }
                }
            }
        }
<<<<<<< HEAD

        /// POSTURAS (algunas)
        //Listado de posturas
        public enum Posture
        {
            None,
            Reverence,
            HandsOnHead,
            Joi
        }

        bool PostureDetector(Posture posture)
        {
            if (postureInDetection != posture) return false;
            else return true;
        }

        // Reverencia (Saludo Marcial)
        // - Inclinación del tronco hasta la altura de la cadera y correcta alineación
        //   (cabeza.XY = cadera.XY)
        private bool Reverence(Vector2 hipCPosition, Vector2 headPosition)
        {
            float distance = (hipCPosition.X - headPosition.X) + (hipCPosition.Y - headPosition.Y);
            //Separación minima aprox.
            if (Math.Abs(distance) > 0.1f)
                return false;
            else
                return true;
        }
        
        // Cobertura (Ambas manos en la cabeza, cubriendose la cara)
        // - Manos sobre la cabeza con independencia del angulo de los codos
        //   (manoIzq.XY = manoDer.XY = cabeza.XY)
        private bool HandsOnHead(Vector2 handLPosition, Vector2 handRPosition, Vector2 headPosition)
        {
            float distanceL = (handLPosition.X - headPosition.X) + (handLPosition.Y - headPosition.Y);
            float distanceR = (handRPosition.X - headPosition.X) + (handRPosition.Y - headPosition.Y);
            //Separación minima aprox.
            if (Math.Abs(distanceL) > 0.05f || Math.Abs(distanceR) > 0.05f)
                return false;
            else
                return true;
        }

        // Posición inicial (Joi)
        // -  El tronco totalmente recto (cabeza.Y = cadera.Y = hombroCentro.Y)
        // -  Manos a la altura de la cadera y adelantadas (Z) (manoDer.X = manoIzq.X = cadera.X) 
        // -  Pies separados a la altura de la cadera (pieIzq.Y = caderaIzq.Y // pieDer.Y = caderaDer.Y)
        private bool Joi(Vector2 headPosition, Vector2 hipCPosition, Vector2 handLPosition, Vector2 handRPosition,
            Vector2 shoulderPosition, Vector2 hipLPosition, Vector2 hipRPosition, Vector2 footLPosition, Vector2 footRPosition)
        {
            float distanceA = ((2*hipCPosition.Y) - (headPosition.Y + shoulderPosition.Y));
            float distanceB = ((2*hipCPosition.X) - (handLPosition.X + handRPosition.X));
            float distanceC1 = (footLPosition.Y - hipLPosition.Y);
            float distanceC2 = (footRPosition.Y - hipRPosition.Y);

            // Cuerpo torcido o con manos levantadas
            if (Math.Abs(distanceA) > 0.05f || Math.Abs(distanceB) > 0.05f) return false;
            // Pies o cerrados o muy abiertos (mayor grado de libertad)
            else if (Math.Abs(distanceC1) > 0.1f || Math.Abs(distanceC2) > 0.1f) return false;
            else return true;
        }
=======
>>>>>>> RefactorXpress
        
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
            initializeMenu();

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

            button_texture[REC_BUTTON_INDEX] = Content.Load<Texture2D>("rec");
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

            // MENU
            // get elapsed frame time in seconds
            frame_time = gameTime.ElapsedGameTime.Milliseconds / 1000.0;

            // update mouse variables
            MouseState mouse_state = Mouse.GetState();
            mx = mouse_state.X;
            my = mouse_state.Y;
            prev_mpressed = mpressed;
            mpressed = mouse_state.LeftButton == ButtonState.Pressed;

            update_buttons();
            // FIN MENU

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
            if (recordStatus == "RECORDED")
            {
                spriteBatch.DrawString(font, recordStatus, new Vector2(20, 80), Color.Red);
                foreach (Joint joint in skeletonToRecord.Joints)
                {
                    Vector2 v = new Vector2((((0.5f * joint.Position.X) + 0.5f) * (640)), (((-0.5f * joint.Position.Y) + 0.5f) * (480)));
                    spriteBatch.Draw(hand, v, Color.White);
                }
            }
            else
            {
                spriteBatch.DrawString(font, recordStatus, new Vector2(20, 80), Color.Blue);
                foreach (Vector2 v in jointPositions)
                {
                    spriteBatch.Draw(hand, v, Color.White);
                }
            }

            // Pinta el menu
            //for (int i = 0; i < NUMBER_OF_BUTTONS; i++)
            //    spriteBatch.Draw(button_texture[i], button_rectangle[i], button_color[i]);

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

        // Inicializa los atributos del menu
        private void initializeMenu()
        {
            // starting x and y locations to stack buttons 
            // vertically in the middle of the screen
            int x = Window.ClientBounds.Width / 2 - BUTTON_WIDTH / 2;
            int y = Window.ClientBounds.Height / 2 -
                NUMBER_OF_BUTTONS / 2 * BUTTON_HEIGHT -
                (NUMBER_OF_BUTTONS % 2) * BUTTON_HEIGHT / 2;
            for (int i = 0; i < NUMBER_OF_BUTTONS; i++)
            {
                button_state[i] = BState.UP;
                button_color[i] = Color.White;
                button_timer[i] = 0.0;
                button_rectangle[i] = new Rectangle(x, y, BUTTON_WIDTH, BUTTON_HEIGHT);
                y += BUTTON_HEIGHT;
            }
            IsMouseVisible = true;
            //background_color = Color.CornflowerBlue;
        }

        // wrapper for hit_image_alpha taking Rectangle and Texture
        Boolean hit_image_alpha(Rectangle rect, Texture2D tex, int x, int y)
        {
            return hit_image_alpha(0, 0, tex, tex.Width * (x - rect.X) /
                rect.Width, tex.Height * (y - rect.Y) / rect.Height);
        }

        // wraps hit_image then determines if hit a transparent part of image 
        Boolean hit_image_alpha(float tx, float ty, Texture2D tex, int x, int y)
        {
            if (hit_image(tx, ty, tex, x, y))
            {
                uint[] data = new uint[tex.Width * tex.Height];
                tex.GetData<uint>(data);
                if ((x - (int)tx) + (y - (int)ty) *
                    tex.Width < tex.Width * tex.Height)
                {
                    return ((data[
                        (x - (int)tx) + (y - (int)ty) * tex.Width
                        ] &
                                0xFF000000) >> 24) > 20;
                }
            }
            return false;
        }

        // determine if x,y is within rectangle formed by texture located at tx,ty
        Boolean hit_image(float tx, float ty, Texture2D tex, int x, int y)
        {
            return (x >= tx &&
                x <= tx + tex.Width &&
                y >= ty &&
                y <= ty + tex.Height);
        }

        // determine state and color of button
        void update_buttons()
        {
            for (int i = 0; i < NUMBER_OF_BUTTONS; i++)
            {

                if (hit_image_alpha(
                    button_rectangle[i], button_texture[i], mx, my))
                {
                    button_timer[i] = 0.0;
                    if (mpressed)
                    {
                        // mouse is currently down
                        button_state[i] = BState.DOWN;
                        button_color[i] = Color.Blue;
                    }
                    else if (!mpressed && prev_mpressed)
                    {
                        // mouse was just released
                        if (button_state[i] == BState.DOWN)
                        {
                            // button i was just down
                            button_state[i] = BState.JUST_RELEASED;
                        }
                    }
                    else
                    {
                        button_state[i] = BState.HOVER;
                        button_color[i] = Color.LightBlue;
                    }
                }
                else
                {
                    button_state[i] = BState.UP;
                    if (button_timer[i] > 0)
                    {
                        button_timer[i] = button_timer[i] - frame_time;
                    }
                    else
                    {
                        button_color[i] = Color.White;
                    }
                }

                if (button_state[i] == BState.JUST_RELEASED)
                {
                    take_action_on_button(i);
                }
            }
        }


        // Logic for each button click goes here
        void take_action_on_button(int i)
        {
            //take action corresponding to which button was clicked
            switch (i)
            {
                case REC_BUTTON_INDEX:
                    if (recordStatus == "RECORDED")
                        recordStatus = "PUSH TO RECORD";
                    else
                        if(recordFrameForPosture("test.dat"))
                            recordStatus = "RECORDED";
                    break;
                default:
                    break;
            }
        }

        float[,] readFrameForPosture(string filename)
        {
            string readed_joint;

            // Creamos el reader
            StreamReader file = new StreamReader(filename);

            // Creamos el array de Joints en formto float
            float[,] joint_float = new float[20,3];

            // Creamos los contadores
            int joint_ctr = 0;
            int coord_ctr = 0;

            // Leemos linea por linea
            while ((readed_joint = file.ReadLine()) != null)
            {
                string[] joint_point = readed_joint.Split(new string[] {","}, StringSplitOptions.None);

                foreach (string joint_str in joint_point)
                {
                    joint_float[joint_ctr, coord_ctr] = float.Parse(joint_str);
                    // Siguiente coordenada...
                    coord_ctr++;
                }

                // Siguiente joint y empezamos de nuevo con las coordenadas...
                coord_ctr = 0;
                joint_ctr++;
            }

            file.Close();

            /**
             * Aquí debería de ir una generación de un SkeletonFrame o algo parecido con los joints que acabamos de leer
             * pero no consigo crear uno con los parámetros que yo defina o meterselos a un objeto, asique: @PALUEGO
             */

            return joint_float;
        }

        Boolean recordFrameForPosture(string filename)
        {
            // Super-fix
            StreamWriter file = new StreamWriter(filename);

            // Fix para que funcione sin esqueletos
            if (skeletonToRecord == null)
            {
                recordStatus = "PUSH TO RECORD";
                return false;
            }

            foreach (Joint j in skeletonToRecord.Joints)
            {
                // Incluyo coordenadas Z por si las moscas... (que no, pero por si acaso)
                file.WriteLine((j.Position.X + ";" + j.Position.Y + ";" + j.Position.Z).Replace(",", ".").Replace(";", ","));
            }

            file.Close();

            /* La guarrerida de antes xD
             * FileStream fs = new FileStream("test.dat", FileMode.Create);
            BinaryWriter w = new BinaryWriter(fs, System.Text.Encoding.ASCII);
            

            foreach (Joint j in skeletonToRecord.Joints)
            {
                w.Flush();

                w.Write("equis,");
                w.Write("y\n");

                w.Flush();
            }
            w.Close();
            fs.Close();

            /*fs = new FileStream("test.dat", FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            Console.WriteLine(sr.ReadToEnd());
            fs.Position = 0;
            BinaryReader br = new BinaryReader(fs);
            Console.WriteLine(br.ReadDecimal());
            Console.WriteLine(br.ReadString());
            Console.WriteLine(br.ReadString());
            Console.WriteLine(br.ReadChar());
            fs.Close();
            */
            return true;
        }

        /*
         * DRAW BONES http://msdn.microsoft.com/en-us/library/jj131025.aspx
         */
    }
}
