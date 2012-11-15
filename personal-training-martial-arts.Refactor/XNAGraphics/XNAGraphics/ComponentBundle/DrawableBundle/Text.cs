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
    class Text : XNAGraphics.KernelBundle.BasicsBundle.BasicDrawable
    {
        public string text;

        public Text(Object font, string text)
            : this(font, text, 0, 0, Color.White) { }

        public Text(Object font, string text, int x, int y)
            : this(font, text, x, y, Color.White) { }

        public Text(Object font, string text, int x, int y, Color color)
            : base(x, y, color)
        {
            this.sprite = font;
            this.text = text;
        }

        public override void load(Game game)
        { }

        public override void update(GameTime gameTime)
        { }

        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString((SpriteFont)this.sprite, this.text, this.getPosition(), this.color);
        }
    }
}
