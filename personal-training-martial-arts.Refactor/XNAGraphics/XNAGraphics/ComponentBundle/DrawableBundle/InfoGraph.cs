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
    public class InfoGraph : XNAGraphics.KernelBundle.BasicsBundle.BasicDrawable
    {
        MouseState mouse_state;
        string text;

        public InfoGraph(Object spriteFont, int x, int y)
            : base(x, y, Color.White)
        {
            this.text = "No data";
            this.sprite = spriteFont;
            this.mouse_state = Mouse.GetState();
            this.text = "X: " + this.mouse_state.X + " || Y: " + this.mouse_state.Y;
        }

        protected override void onLoad(Microsoft.Xna.Framework.Game game)
        { }

        protected override void onUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            this.mouse_state = Mouse.GetState();
            this.text = "X: " + this.mouse_state.X + "\nY: " + this.mouse_state.Y;
        }

        protected override void onDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString((SpriteFont)this.sprite, this.text, this.getPosition(), this.color);
        }
    }
}
