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
using personal_training_martial_arts.Core;

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
        public SpriteBatch spriteBatch;

        /// <summary>
        /// El device de graficos.
        /// </summary>
        private GraphicsDevice graphicsDevice;

        /// <summary>
        /// Mappeador de puntos de un espacio tridimensional a 2D.
        /// </summary>
        private CoordinateMapper mapper;

        // Textura del hueso
        private Texture2D boneTexture;

        /// <summary>
        /// Escenario a pintar
        /// </summary>
        private List<Tuple<Texture2D, Rectangle, Color>> backgroundComponents = new List<Tuple<Texture2D, Rectangle, Color>>();
        private List<Tuple<Texture2D, Rectangle, Color>> layerComponents = new List<Tuple<Texture2D, Rectangle, Color>>();
        private List<Tuple<SpriteFont, string, Vector2, Color>> textComponents = new List<Tuple<SpriteFont, string, Vector2, Color>>();
        private List<Tuple<Skeleton, Texture2D,double[]>> skComponents = new List<Tuple<Skeleton, Texture2D,double[]>>();
        private List<Tuple<PostureInformation, Texture2D>> postureComponents = new List<Tuple<PostureInformation, Texture2D>>();

        private ContentHandler ch;

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
        public GameScreen(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
        }


        public void updateKinectSensor(KinectSensor sensor)
        {
            this.sensor = sensor;
            if (sensor != null)
            {
                this.mapper = new CoordinateMapper(sensor);
            }
        }

        /// <summary>
        /// Pinta el escenario completo que se ha preparado. Llamar en cada draw.
        /// </summary>
        public void drawAll()
        {
            // Clear the screen. ADVERTENCIA: No se si debería legarse a CORE
            
            //***************************************************************
            spriteBatch.Begin();

            //Pintamos backgrounds/s
            foreach (Tuple<Texture2D, Rectangle, Color> backgroundTuple in this.backgroundComponents)
            {
                spriteBatch.Draw(backgroundTuple.Item1, backgroundTuple.Item2, backgroundTuple.Item3);
            }
            //Pintamos esqueleto/s.
            foreach (Tuple<Skeleton, Texture2D,double[]> skeletonToRecord in this.skComponents)
            {
                drawPosture2D(skeletonToRecord.Item1, skeletonToRecord.Item2,skeletonToRecord.Item3);
            }
            //Pintamos capas/s
            foreach (Tuple<Texture2D, Rectangle, Color> layerTuple in this.layerComponents)
            {
                spriteBatch.Draw(layerTuple.Item1, layerTuple.Item2, layerTuple.Item3);
            }
            //Pintamos postura/s.
            foreach (Tuple<PostureInformation, Texture2D> posture in this.postureComponents)
            {
                drawPosture2D(posture.Item1, posture.Item2);
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
            this.skComponents.Clear();
            this.layerComponents.Clear();
            this.textComponents.Clear();
            this.postureComponents.Clear();
            this.backgroundComponents.Clear();
        }



        //********************************************************************************************
        //BACKGROUND__________________________________________________________________________________


        /// <summary>
        /// Añadir una capa, ya sea para hacer un boton o un interfaz bonico.
        /// </summary>
        /// <param name="texture">Textura a mostrar</param>
        /// <param name="rectangle">Espacio que ocupa</param>
        /// <param name="color">Color de fondo?</param>
        /// <returns>Posición que ocupa en componentes para su edición</returns>
        public int backgroundAdd(Texture2D texture, Rectangle rectangle, Color color)
        {
            Tuple<Texture2D, Rectangle, Color> data = new Tuple<Texture2D, Rectangle, Color>(texture, rectangle, color);
            this.backgroundComponents.Add(data);
            return this.backgroundComponents.LastIndexOf(data);
        }

        /// <summary>
        /// Modificar una capa numero (index)
        /// </summary>
        /// <param name="index">La posición en componentes</param>
        /// <param name="texture">Nueva textura a aplicar</param>
        /// <returns>True si pudo realizarse correctamente, false si no.</returns>
        public bool updateBackgroundIndex(int index, Texture2D texture)
        {
            Tuple<Texture2D, Rectangle, Color> data;
            try
            {
                data = this.backgroundComponents.ElementAt(index);
                this.backgroundComponents.Insert(index, new Tuple<Texture2D, Rectangle, Color>(texture, data.Item2, data.Item3));
                return true;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                return false;
            }
        }

        //Borrar los componentes para rehacer todo el escenario
        public void backgroundComponentsClear()
        {
            backgroundComponents.Clear();
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
            layerComponents.Clear();
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
        /// Actualizar un componente de texto
        /// en posición (index)
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
        public int skeletonAdd(Skeleton skeletonToRecord, Texture2D spriteGraphic,double[] pointData)
        {
            Tuple<Skeleton,Texture2D,double[]> data = new Tuple<Skeleton,Texture2D,double[]>(skeletonToRecord, spriteGraphic, pointData);
            this.skComponents.Add(data); 
            return this.skComponents.LastIndexOf(data);
        }

        // Ya no pinta el titulo de la postura de forma automatica, solo su esqueleto :)
        public int postureAdd(PostureInformation skeletonToRecord, Texture2D spriteGraphic)
        {
            Tuple<PostureInformation, Texture2D> data = new Tuple<PostureInformation, Texture2D>(skeletonToRecord, spriteGraphic);
            this.postureComponents.Add(data);
            return this.postureComponents.LastIndexOf(data);
        }

        /// <summary>
        /// Modifica el esqueleto numero (index).
        /// </summary>
        /// <param name="index">Numero de esqueleto</param>
        /// <param name="skeletonToRecord">Esqueleto</param>
        /// <returns>True si pudo realizarse correctamente, false si no.</returns>
        public bool updateSkeletonIndex(int index, Skeleton skeletonToRecord,double[] pointData)
        {
            Tuple<Skeleton,Texture2D,double[]> data;
            try{
                //Todavía no se si modifica en memoria o replica.
                data = this.skComponents.ElementAt(index);
                this.skComponents.Insert(index,new Tuple<Skeleton,Texture2D,double[]>(skeletonToRecord,data.Item2,pointData));

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

        private void drawPosture2D (Posture.Posture posture, Texture2D spriteGraphic)
        {
            // PINTA HUESOS
            this.DrawBone(posture.joints, JointType.Head, JointType.ShoulderCenter);
            this.DrawBone(posture.joints, JointType.ShoulderCenter, JointType.ShoulderLeft);
            this.DrawBone(posture.joints, JointType.ShoulderCenter, JointType.ShoulderRight);
            this.DrawBone(posture.joints, JointType.ShoulderCenter, JointType.Spine);
            this.DrawBone(posture.joints, JointType.Spine, JointType.HipCenter);
            this.DrawBone(posture.joints, JointType.HipCenter, JointType.HipLeft);
            this.DrawBone(posture.joints, JointType.HipCenter, JointType.HipRight);

            this.DrawBone(posture.joints, JointType.ShoulderLeft, JointType.ElbowLeft);
            this.DrawBone(posture.joints, JointType.ElbowLeft, JointType.WristLeft);
            this.DrawBone(posture.joints, JointType.WristLeft, JointType.HandLeft);

            this.DrawBone(posture.joints, JointType.ShoulderRight, JointType.ElbowRight);
            this.DrawBone(posture.joints, JointType.ElbowRight, JointType.WristRight);
            this.DrawBone(posture.joints, JointType.WristRight, JointType.HandRight);

            this.DrawBone(posture.joints, JointType.HipLeft, JointType.KneeLeft);
            this.DrawBone(posture.joints, JointType.KneeLeft, JointType.AnkleLeft);
            this.DrawBone(posture.joints, JointType.AnkleLeft, JointType.FootLeft);

            this.DrawBone(posture.joints, JointType.HipRight, JointType.KneeRight);
            this.DrawBone(posture.joints, JointType.KneeRight, JointType.AnkleRight);
            this.DrawBone(posture.joints, JointType.AnkleRight, JointType.FootRight);

            // PINTA ARTICULACIONES
            Vector2 jointOrigin = new Vector2(spriteGraphic.Width / 2, spriteGraphic.Height / 2);

            foreach (Vector3 joint in posture.joints)
            {
                Color jointColor = Color.White;
                //Es posible que se pueda usar otro override con menos parámetros.
                //Pero mola mas asi :P xD
                this.spriteBatch.Draw(
                    spriteGraphic,
                    this.SkeletonToColorMap(joint),
                    null,
                    jointColor,
                    0.0f,
                    jointOrigin,
                    1.0f,
                    SpriteEffects.None,
                    0.0f);
            }
        }

        private void drawPosture2D(Skeleton skeletonToRecord, Texture2D spriteGraphic, double[] pointData)
        {
            // PINTA HUESOS
            this.DrawBone(skeletonToRecord.Joints, JointType.Head, JointType.ShoulderCenter);
            this.DrawBone(skeletonToRecord.Joints, JointType.ShoulderCenter, JointType.ShoulderLeft);
            this.DrawBone(skeletonToRecord.Joints, JointType.ShoulderCenter, JointType.ShoulderRight);
            this.DrawBone(skeletonToRecord.Joints, JointType.ShoulderCenter, JointType.Spine);
            this.DrawBone(skeletonToRecord.Joints, JointType.Spine, JointType.HipCenter);
            this.DrawBone(skeletonToRecord.Joints, JointType.HipCenter, JointType.HipLeft);
            this.DrawBone(skeletonToRecord.Joints, JointType.HipCenter, JointType.HipRight);

            this.DrawBone(skeletonToRecord.Joints, JointType.ShoulderLeft, JointType.ElbowLeft);
            this.DrawBone(skeletonToRecord.Joints, JointType.ElbowLeft, JointType.WristLeft);
            this.DrawBone(skeletonToRecord.Joints, JointType.WristLeft, JointType.HandLeft);

            this.DrawBone(skeletonToRecord.Joints, JointType.ShoulderRight, JointType.ElbowRight);
            this.DrawBone(skeletonToRecord.Joints, JointType.ElbowRight, JointType.WristRight);
            this.DrawBone(skeletonToRecord.Joints, JointType.WristRight, JointType.HandRight);

            this.DrawBone(skeletonToRecord.Joints, JointType.HipLeft, JointType.KneeLeft);
            this.DrawBone(skeletonToRecord.Joints, JointType.KneeLeft, JointType.AnkleLeft);
            this.DrawBone(skeletonToRecord.Joints, JointType.AnkleLeft, JointType.FootLeft);

            this.DrawBone(skeletonToRecord.Joints, JointType.HipRight, JointType.KneeRight);
            this.DrawBone(skeletonToRecord.Joints, JointType.KneeRight, JointType.AnkleRight);
            this.DrawBone(skeletonToRecord.Joints, JointType.AnkleRight, JointType.FootRight);

            // PINTA ARTICULACIONES
            Vector2 jointOrigin = new Vector2(spriteGraphic.Width / 2, spriteGraphic.Height / 2);
            int i = 0;
            foreach (Joint joint in skeletonToRecord.Joints)
            {
                // *BETA* - LO COMENTO PARA SACAR LA BETA
                //float colorValue = float.Parse(""+pointData[i]);
                //Color jointColor = new Color(new Vector3(colorValue,(1-colorValue),0.0F));
                i++;
                Color jointColor = Color.SlateBlue;
                if (joint.TrackingState != JointTrackingState.Tracked)
                {
                    jointColor = Color.Red;
                } //Tracked state, no los keremos
                
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

        private Vector2 SkeletonToColorMap(Vector3 joint)
        {
            SkeletonPoint point = new SkeletonPoint();
            point.X = joint.X;
            point.Y = joint.Y;
            point.Z = joint.Z;
            if ((null != sensor) && (null != sensor.ColorStream))
            {
                // This is used to map a skeleton point to the color image location
                ColorImagePoint colorPt = mapper.MapSkeletonPointToColorPoint(point, sensor.ColorStream.Format);
                return new Vector2(colorPt.X, colorPt.Y);
            }

            return Vector2.Zero;
        }

        /// <summary>
        /// This method draws a bone.
        /// </summary>
        /// <param name="joints">The joint data.</param>
        /// <param name="startJoint">The starting joint.</param>
        /// <param name="endJoint">The ending joint.</param>
        private void DrawBone(Vector3[] joints, JointType startJoint, JointType endJoint)
        {
            Vector2 start = this.SkeletonToColorMap(joints[(int) startJoint]);
            Vector2 end = this.SkeletonToColorMap(joints[(int) endJoint]);
            Vector2 diff = end - start;
            Vector2 scale = new Vector2(1.0f, diff.Length() / this.boneTexture.Height);

            float angle = (float)Math.Atan2(diff.Y, diff.X) - MathHelper.PiOver2;
            Color color = Color.LightGreen;

            this.spriteBatch.Draw(this.boneTexture, start, null, color, angle, new Vector2(0.5f, 0.0f), scale, SpriteEffects.None, 1.0f);
        }

        /// <summary>
        /// This method draws a bone.
        /// </summary>
        /// <param name="joints">The joint data.</param>
        /// <param name="startJoint">The starting joint.</param>
        /// <param name="endJoint">The ending joint.</param>
        private void DrawBone(JointCollection joints, JointType startJoint, JointType endJoint)
        {
            Vector2 start = this.SkeletonToColorMap(joints[startJoint].Position);
            Vector2 end = this.SkeletonToColorMap(joints[endJoint].Position);
            Vector2 diff = end - start;
            Vector2 scale = new Vector2(1.0f, diff.Length() / this.boneTexture.Height);

            float angle = (float)Math.Atan2(diff.Y, diff.X) - MathHelper.PiOver2;

            Color color = Color.LightGreen;
            if (joints[startJoint].TrackingState != JointTrackingState.Tracked ||
                joints[endJoint].TrackingState != JointTrackingState.Tracked)
            {
                color = Color.Gray;
            }

            this.spriteBatch.Draw(this.boneTexture, start, null, color, angle, new Vector2(0.5f, 0.0f), scale, SpriteEffects.None, 1.0f);
        }

        public void updateContentHandler(ContentHandler ch)
        {
            this.ch = ch;
            boneTexture = (Texture2D)this.ch.get("bone");
        }

    }
}
