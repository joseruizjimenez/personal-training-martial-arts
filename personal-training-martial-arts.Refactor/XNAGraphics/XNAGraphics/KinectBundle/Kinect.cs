using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Kinect;

namespace XNAGraphics.KinectBundle
{
    class Kinect
    {
        public KinectSensor kinectSensor;
        public Texture2D kinectRGBVideo;
        public Skeleton skeleton;
        int elevationAngle;

        public Kinect()
        {
            this.elevationAngle = 2;
        }

        public void initialize()
        {
            // En la inicializacion del proyecto XNA lanzamos la busqueda del Kinect cuando
            // se produce un nuevo evento en StatusChangedEventArgs
            KinectSensor.KinectSensors.StatusChanged += new EventHandler<StatusChangedEventArgs>(this.KinectSensors_StatusChanged);
            this.DiscoverKinectSensor();
        }

        public void load(Game game)
        {
            this.kinectRGBVideo = new Texture2D(game.GraphicsDevice, 640, 480);
        }

        public void unload()
        {
            if (this.kinectSensor != null)
            {
                this.kinectSensor.Stop();
                this.kinectSensor.Dispose();
            }
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
                    this.kinectSensor = sensor;
                    break;
                }
            }

            // Si no ha encontrado ninguno:
            if (this.kinectSensor == null)
            {
                //connectedStatus = "KINECT NOT DETECTED";
                return;
            }

            // Localizado el kinect, podemos especificar su estado concreto:
            switch (this.kinectSensor.Status)
            {
                case KinectStatus.Connected:
                    {
                        //connectedStatus = "KINECT DETECTED";
                        break;
                    }
                case KinectStatus.Disconnected:
                    {
                        //connectedStatus = "Status: Disconnected";
                        break;
                    }
                case KinectStatus.NotPowered:
                    {
                        //connectedStatus = "Status: Connect the power";
                        break;
                    }
                default:
                    {
                        //connectedStatus = "Status: Error";
                        break;
                    }
            }

            // Inicializa el kinect encontrado si esta operativo
            if (this.kinectSensor.Status == KinectStatus.Connected)
            {
                this.InitializeKinect();
            }
        }

        /// <summary>
        /// Activamos y configuramos la camara RGB y la deteccion del esqueleto en el Kinect
        /// </summary>
        private bool InitializeKinect()
        {
            // Color stream
            this.kinectSensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
            this.kinectSensor.ColorFrameReady += new EventHandler<ColorImageFrameReadyEventArgs>(this.kinectSensor_ColorFrameReady);

            // Skeleton Stream
            this.kinectSensor.SkeletonStream.Enable(new TransformSmoothParameters()
            {
                Smoothing = 0.5f,
                Correction = 0.5f,
                Prediction = 0.5f,
                JitterRadius = 0.05f,
                MaxDeviationRadius = 0.04f
            });
            this.kinectSensor.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(this.kinectSensor_SkeletonFrameReady);

            try
            {
                this.kinectSensor.Start();
                this.kinectSensor.ElevationAngle = this.elevationAngle;
            }
            catch
            {
                //connectedStatus = "Unable to start the Kinect Sensor";
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
                    Skeleton playerSkeleton = (from s in skeletonData
                                               where s.TrackingState ==
                                                   SkeletonTrackingState.Tracked
                                               select s).FirstOrDefault();
                    if (playerSkeleton != null)
                    {
                        this.skeleton = playerSkeleton;
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
                    // FIX: kinectRGBVideo = new Texture2D(graphics.GraphicsDevice, colorImageFrame.Width, colorImageFrame.Height);

                    // Recorre los pixels y asigna el byte adecuado a cada punto
                    // El indice se incrementa de 4 en 4 porque hay 3 colores: red, green, blue
                    // El bytemap pixelsFromFrame se compone de un array unidimensional
                    int index = 0;
                    for (int y = 0; y < colorImageFrame.Height; y++)
                    {
                        for (int x = 0; x < colorImageFrame.Width; x++, index += 4)
                        {
                                color[y * colorImageFrame.Width + x] = 
                                    new Color(pixelsFromFrame[index + 2], pixelsFromFrame[index + 1], pixelsFromFrame[index + 0]);
                        }
                    }

                    // Actualizamos los datos de los pixels del ColorImageFrame a nuestra Texture2D
                    this.kinectRGBVideo.SetData(color);
                }
            }
        }
    }
}
