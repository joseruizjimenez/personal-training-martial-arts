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
    public class Image : XNAGraphics.KernelBundle.BasicsBundle.BasicDrawable
    {
        // Coordenadas de recorte
        private int x1Fixed = 0;
        private int x2Fixed = 0;

        public Image(Object texture)
            : this(texture, 0, 0, 1, Color.White) { }

        public Image(Object texture, int x, int y)
            : this(texture, x, y, 1, Color.White) { }

        public Image(Object texture, int x, int y, int x1Fixed, int x2Fixed)
            : this(texture, x, y, 1, Color.White) 
        {
            this.x1Fixed = x1Fixed;
            this.x2Fixed = x2Fixed;
        }

        public Image(Object texture, int x, int y, float scale)
            : this(texture, x, y, scale, Color.White) { }

        public Image(Object texture, int x, int y, Color color)
            : this(texture, x, y, 1, color) { }

        public Image(Object texture, int x, int y, float scale, Color color)
            : base(x, y, color)
        {
            this.sprite = texture;
            this.scale = scale;
        }

        protected override void onLoad(Game game)
        { }

        protected override void onUpdate(GameTime gameTime)
        { }

        protected override void onDraw(SpriteBatch spriteBatch)
        {
            Texture2D texture = (Texture2D) this.sprite;
            //spriteBatch.Draw(texture, new Rectangle(this.x, this.y, texture.Width, texture.Height), this.color);
            spriteBatch.Draw(texture, this.getPosition(), new Rectangle(x1Fixed, 0, texture.Width, texture.Height), this.color, this.rotation, new Vector2(), this.scale, SpriteEffects.None, 0f);
        }
    }
}
