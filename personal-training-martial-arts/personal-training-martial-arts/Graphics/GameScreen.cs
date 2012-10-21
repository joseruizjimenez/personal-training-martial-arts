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
        /// La clase que guarda todos los sprites para ser pintados.
        /// </summary>
        private SpriteBatch spriteBatch { get; set; }

        /// <summary>
        /// El KinectSensor en uso. Es posible que este objeto se pueda sacar
        /// directamente y no tener que pasarselo.
        /// </summary>
        private KinectSensor sensor;

        /// <summary>
        /// Mappeador de puntos de un espacio tridimensional a 2D.
        /// </summary>
        private CoordinateMapper mapper;

        GameScreen(SpriteBatch spriteBatch, KinectSensor sensor)
        {
            this.spriteBatch = spriteBatch;
            this.sensor = sensor;
            this.mapper = new CoordinateMapper(sensor);
        }

        public void drawPosture2D (Skeleton skeletonToRecord, Texture2D spriteGraphic)
        {
            Vector2 jointOrigin = new Vector2(spriteGraphic.Width / 2, spriteGraphic.Height / 2);

            foreach (Joint joint in skeletonToRecord.Joints)
            {
                //Es posible que se pueda usar otro override con menos parámetros.
                //Pero mola mas asi :P xD
                this.spriteBatch.Draw(
                    spriteGraphic,
                    this.SkeletonToColorMap(joint.Position),
                    null,
                    Color.White,
                    0.0f,
                    jointOrigin,
                    1.0f,
                    SpriteEffects.None,
                    0.0f);
            }
        }

        public void drawPosture2DMinimized();

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
