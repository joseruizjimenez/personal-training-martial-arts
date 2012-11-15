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

namespace XNAGraphics.ComponentBundle.DrawableBundle
{
    class ScrollingImage : XNAGraphics.KernelBundle.BasicsBundle.BasicDrawable
    {
        public int speed;
        public GraphicsDeviceManager graphicsDevice;

        // FIX: Agregar overloaded methods.
        public ScrollingImage(Object texture, GraphicsDeviceManager graphicsDevice, int x, int y, Color color, int speed, float scale, float rotation)
            : base(x, y, color, scale, rotation)
        {
            this.sprite = texture;
            this.graphicsDevice = graphicsDevice;
            this.speed = speed;

            // Set the screen position to the center of the screen.
            this.x = this.graphicsDevice.PreferredBackBufferWidth / 2;
            this.y = this.graphicsDevice.PreferredBackBufferHeight / 2;
        }

        public override void load(Game game)
        { }

        public override void update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Texture2D texture = (Texture2D) this.sprite;

            this.y += elapsed * speed;
            this.y = this.y % texture.Height;

            //if (elapsed > 1)
                //throw new Exception("Oopa GANGNAMSTYLE!!");
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = (Texture2D)this.sprite;

            Vector2 origin = new Vector2(texture.Width / 2, 0);            
            // Offset to draw the second texture, when necessary.
            Vector2 texturesize = new Vector2(0, texture.Height);

            // Draw the texture, if it is still onscreen.
            if (this.y < this.graphicsDevice.PreferredBackBufferHeight)
            {
                spriteBatch.Draw(texture, this.getPosition(), null, this.color, this.rotation, origin, this.scale, SpriteEffects.None, 0f);
            }
            // Draw the texture a second time, behind the first,
            // to create the scrolling illusion.
            spriteBatch.Draw(texture, this.getPosition() - texturesize, null, this.color, this.rotation, origin, this.scale, SpriteEffects.None, 0f);
        }
    }
}
