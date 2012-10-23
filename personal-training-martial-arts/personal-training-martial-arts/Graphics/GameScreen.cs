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

namespace personal_training_martial_arts.Graphics
{
    class GameScreen
    {
        /// <summary>
        /// El KinectSensor en uso. Es posible que este objeto se pueda sacar
        /// directamente y no tener que pasarselo.
        /// </summary>
        private KinectSensor sensor;

        /// <summary>
        /// La clase que guarda todos los sprites para ser pintados.
        /// </summary>
        private SpriteBatch spriteBatch;

        /// <summary>
        /// El device de graficos.
        /// </summary>
        private GraphicsDevice graphicsDevice;

        /// <summary>
        /// Mappeador de puntos de un espacio tridimensional a 2D.
        /// </summary>
        private CoordinateMapper mapper;

        /// <summary>
        /// Esqueletos a pintar
        /// </summary>

        private List<Tuple<Texture2D, Rectangle, Color>> layerComponents = new List<Tuple<Texture2D, Rectangle, Color>>();
        private List<Tuple<SpriteFont, string, Vector2, Color>> textComponents = new List<Tuple<SpriteFont, string, Vector2, Color>>();
        private List<Tuple<Skeleton, Texture2D>> skComponents = new List<Tuple<Skeleton, Texture2D>>();



        //******************************************************************************************
        //PROPOSITO GENERAL_________________________________________________________________________

        //Sin implementar (Serialización de objetos)
        public void saveView()
        {
        }

        //Sin implementar (Serialización de objetos)
        public void loadView()
        {
        }
        
        /// <summary>
        /// Para inicializar el escenario hay que pasar un sensor y un graphicsDevice.
        /// </summary>
        /// <param name="sensor">Sensor de kinect</param>
        /// <param name="graphicsDevice">Graficos XNA</param>
        public GameScreen(KinectSensor sensor,GraphicsDevice graphicsDevice)
        {
            this.sensor = sensor;
            this.graphicsDevice = graphicsDevice;
            if (sensor != null)
            {
                this.mapper = new CoordinateMapper(sensor);
            }
            if (graphicsDevice != null)
            {
                this.spriteBatch = new SpriteBatch(graphicsDevice);
            }
        }

        /// <summary>
        /// Pinta el escenario completo que se ha preparado. Llamar en cada draw.
        /// </summary>
        public void drawAll()
        {
            // Clear the screen. ADVERTENCIA: No se si debería legarse a CORE
            graphicsDevice.Clear(Color.White);
            //***************************************************************
            spriteBatch.Begin();
            //Pintamos esqueleto/s.
            foreach (Tuple<Skeleton, Texture2D> skeletonToRecord in this.skComponents)
            {
                drawPosture2D(skeletonToRecord.Item1, skeletonToRecord.Item2);
            }
            //Pintamos capas/s
            foreach (Tuple<Texture2D, Rectangle, Color> layerTuple in this.layerComponents)
            {
                spriteBatch.Draw(layerTuple.Item1, layerTuple.Item2, layerTuple.Item3);
            }
            //Pintamos texto/s
            foreach (Tuple<SpriteFont, string, Vector2, Color> textTuple in this.textComponents)
            {
                spriteBatch.DrawString(textTuple.Item1, textTuple.Item2, textTuple.Item3, textTuple.Item4);
            }
            //Se sobreentiende que los textos estarán sobre las capas y las capas sobre los esqueletos.
            //Lo ultimo es el esqueleto. 
            //La capa de video se deberá pintar antes que todo esto, deberá ir por libre.
            spriteBatch.End();
        }

        //Borra todo el escenario completamente.
        public void clearAll()
        {
            this.skeletonComponentsClear();
            this.layerComponentsClear();
            this.textComponentsClear();
        }



        //********************************************************************************************
        //LAYERS______________________________________________________________________________________


        /// <summary>
        /// Añadir una capa, ya sea para hacer un boton o un interfaz bonico.
        /// </summary>
        /// <param name="texture">Textura a mostrar</param>
        /// <param name="rectangle">Espacio que ocupa</param>
        /// <param name="color">Color de fondo?</param>
        /// <returns>Posición que ocupa en componentes para su edición</returns>
        public int layerAdd(Texture2D texture, Rectangle rectangle, Color color)
        {
            Tuple<Texture2D, Rectangle, Color> data = new Tuple<Texture2D, Rectangle, Color>(texture, rectangle, color);
            this.layerComponents.Add(data);
            return this.layerComponents.LastIndexOf(data);
        }

        /// <summary>
        /// Modificar una capa numero (index)
        /// </summary>
        /// <param name="index">La posición en componentes</param>
        /// <param name="texture">Nueva textura a aplicar</param>
        /// <returns>True si pudo realizarse correctamente, false si no.</returns>
        public bool updateLayerIndex(int index, Texture2D texture)
        {
            Tuple<Texture2D,Rectangle, Color> data;
            try
            {
                data = this.layerComponents.ElementAt(index);
                this.layerComponents.Insert(index, new Tuple<Texture2D, Rectangle, Color>(texture, data.Item2, data.Item3));
                return true;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                return false;
            }
        }

        //Borrar los componentes para rehacer todo el escenario
        public void layerComponentsClear()
        {
            textComponents.Clear();
        }

        //********************************************************************************************
        //TEXT________________________________________________________________________________________


        /// <summary>
        /// Añadir un texto a la lista de componentes de textos.
        /// </summary>
        /// <param name="spriteFont">Fuente del texto</param>
        /// <param name="text">Texto en sí</param>
        /// <param name="size">Espacio que ocupa? o posición, no se xD</param>
        /// <param name="color">Color del texto</param>
        /// <returns>Devuelve la posición en componentes de texto</returns>
        public int textAdd(SpriteFont spriteFont, string text, Vector2 size, Color color)
        {
            Tuple<SpriteFont, string, Vector2, Color> data = new Tuple<SpriteFont, string, Vector2, Color>(spriteFont, text, size, color);
            this.textComponents.Add(data);
            return this.textComponents.LastIndexOf(data);
        }

        /// <summary>
        /// Actualizar un componente de texto en posición (index)
        /// </summary>
        /// <param name="index">Posición en comonentes</param>
        /// <param name="text">Texto nuevo.</param>
        /// <returns>True si pudo realizarse correctamente, false si no.</returns>
        public bool updateTextIndex(int index, string text)
        {
            Tuple<SpriteFont, string, Vector2, Color> data;
            try{
                data = this.textComponents.ElementAt(index);
                this.textComponents.Insert(index, new Tuple<SpriteFont, string, Vector2, Color>(data.Item1, text, data.Item3, data.Item4));
                return true;
            }catch(Exception e){
                System.Console.WriteLine(e);
                return false;
            }
        }

        //Borrar los componentes para rehacer todo el escenario
        public void textComponentsClear()
        {
            textComponents.Clear();
        }


        //********************************************************************************************
        //SKELETON____________________________________________________________________________________

        /// <summary>
        /// Añade un componente de esqueleto a la escena.
        /// Devuelve un entero con el índice para poder ser actualizado posteriormente.
        /// </summary>
        /// <param name="skeletonToRecord">Esqueleto a pintar</param>
        /// <param name="spriteGraphic">El tipo de puntos que pinta</param>
        /// <returns>Indice en components</returns>
        public int skeletonAdd(Skeleton skeletonToRecord, Texture2D spriteGraphic)
        {
            Tuple<Skeleton,Texture2D> data = new Tuple<Skeleton,Texture2D>(skeletonToRecord, spriteGraphic);
            this.skComponents.Add(data); 
            return this.skComponents.LastIndexOf(data);
        }

        /// <summary>
        /// Modifica el esqueleto numero (index).
        /// </summary>
        /// <param name="index">Numero de esqueleto</param>
        /// <param name="skeletonToRecord">Esqueleto</param>
        /// <returns>True si pudo realizarse correctamente, false si no.</returns>
        public bool updateSkeletonIndex(int index, Skeleton skeletonToRecord)
        {
            Tuple<Skeleton,Texture2D> data;
            try{
                //Todavía no se si modifica en memoria o replica.
                data = this.skComponents.ElementAt(index);
                this.skComponents.Insert(index,new Tuple<Skeleton,Texture2D>(skeletonToRecord,data.Item2));

                return true;
            }catch(Exception e){
                System.Console.WriteLine(e);
                return false;
            }
        }

        //Borrar los componentes para rehacer todo el escenario
        public void skeletonComponentsClear()
        {
            skComponents.Clear();
        }

        private void drawPosture2D (Skeleton skeletonToRecord, Texture2D spriteGraphic)
        {
            Vector2 jointOrigin = new Vector2(spriteGraphic.Width / 2, spriteGraphic.Height / 2);

            foreach (Joint joint in skeletonToRecord.Joints)
            {
                Color jointColor = Color.White;
                if (joint.TrackingState != JointTrackingState.Tracked)
                {
                    jointColor = Color.Red;
                }
                //Es posible que se pueda usar otro override con menos parámetros.
                //Pero mola mas asi :P xD
                this.spriteBatch.Draw(
                    spriteGraphic,
                    this.SkeletonToColorMap(joint.Position),
                    null,
                    jointColor,
                    0.0f,
                    jointOrigin,
                    1.0f,
                    SpriteEffects.None,
                    0.0f);
            }
        }

        private Vector2 SkeletonToColorMap(SkeletonPoint point)
        {
            if ((null != sensor) && (null != sensor.ColorStream))
            {
                // This is used to map a skeleton point to the color image location
                ColorImagePoint colorPt = mapper.MapSkeletonPointToColorPoint(point, sensor.ColorStream.Format);
                return new Vector2(colorPt.X, colorPt.Y);
            }

            return Vector2.Zero;
        }
    }
}
