using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace personal_training_martial_arts.Graphics
{
    class ColorRender
    {

        /// <summary>
        /// El KinectSensor en uso. Es posible que este objeto se pueda sacar
        /// directamente y no tener que pasarselo.
        /// </summary>
        private KinectSensor sensor;

        /// <summary>
        /// SpriteBatch para añadir la imagen.
        /// </summary>
        private SpriteBatch spriteBatch;

        /// <summary>
        /// Graphics device for paint.
        /// </summary>
        private GraphicsDevice gameGraphics;


        /// <summary>
        /// Color data formatted.
        /// </summary>
        private byte[] colorData;

        /// <summary>
        /// The color frame as a texture.
        /// </summary>
        private Texture2D colorTexture;

        /// <summary>
        /// The back buffer where color frame is scaled as requested by the Size.
        /// </summary>
        private RenderTarget2D backBuffer;

        public ColorRender(SpriteBatch spriteBatch, KinectSensor sensor, GraphicsDevice gameGraphics)
        {
            this.sensor = sensor;
            this.spriteBatch = spriteBatch;
            this.gameGraphics = gameGraphics;
        }


        public void renderFrame(ColorImageFrame frame)
        {

                // Reallocate values if necessary
                if (colorData == null || colorData.Length != frame.PixelDataLength)
                {
                    colorData = new byte[frame.PixelDataLength];

                    colorTexture = new Texture2D(
                        gameGraphics,
                        frame.Width,
                        frame.Height,
                        false,
                        SurfaceFormat.Color);

                    this.backBuffer = new RenderTarget2D(
                        gameGraphics,
                        frame.Width,
                        frame.Height,
                        false,
                        SurfaceFormat.Color,
                        DepthFormat.None,
                        gameGraphics.PresentationParameters.MultiSampleCount,
                        RenderTargetUsage.PreserveContents);
                }

                frame.CopyPixelDataTo(this.colorData);
        }

        public void drawUntilRendered(Effect kinectColorVisualizer)
        {
            // Set the backbuffer and clear
            gameGraphics.SetRenderTarget(this.backBuffer);
            gameGraphics.Clear(ClearOptions.Target, Color.Black, 1.0f, 0);

            this.colorTexture.SetData<byte>(this.colorData);

            // Draw the color image
            this.spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, kinectColorVisualizer);
            this.spriteBatch.Draw(this.colorTexture, Vector2.Zero, Color.White);
            this.spriteBatch.End();
        }
    }
}