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
    class Panel : XNAGraphics.KernelBundle.BasicsBundle.BasicDrawable
    {
        public int width;
        public int height;

        public Vector2 b_top_left;
        public Vector2 b_top_right;
        public Vector2 b_bot_left;
        public Vector2 b_bot_right;

        public Color b_color;

        public Texture2D fill_texture;
        public Texture2D b_texture;

        public Panel(Rectangle position, Color color)
            : base(position.X, position.Y, color)
        {
            this.width = position.Width;
            this.height = position.Height;

            // FIX: OJO! Estoy pintando el borde con las mismas coordenadas que el panel, podriamos pintarlas en 0,0, w:0, h:0
            this.b_top_left = new Vector2(position.X, position.Y);
            this.b_top_right = new Vector2(position.X + position.Width, position.Y);
            this.b_bot_left = new Vector2(position.X, position.Y + position.Height);
            this.b_bot_right = new Vector2(position.X + position.Width, position.Y + position.Height);

            this.b_color = Color.Black;
        }

        public Panel(Rectangle position, Color color, int border_size, Color border_color)
            : base(position.X, position.Y, color)
        {
            this.width = position.Width;
            this.height = position.Height;

            this.b_top_left = new Vector2(position.X - border_size, position.Y - border_size);
            this.b_top_right = new Vector2(position.X + position.Width + border_size, position.Y - border_size);
            this.b_bot_left = new Vector2(position.X - border_size, position.Y + position.Height + border_size);
            this.b_bot_right = new Vector2(position.X + position.Width + border_size, position.Y + position.Height + border_size);

            this.b_color = border_color;
        }

        public Panel(Rectangle position, Color color, int border_size, Color border_color, float scale, float rotation)
            : base(position.X, position.Y, color, scale, rotation)
        {
            this.width = position.Width;
            this.height = position.Height;

            this.b_top_left = new Vector2(position.X - border_size, position.Y - border_size);
            this.b_top_right = new Vector2(position.X + position.Width + border_size, position.Y - border_size);
            this.b_bot_left = new Vector2(position.X - border_size, position.Y + position.Height + border_size);
            this.b_bot_right = new Vector2(position.X + position.Width + border_size, position.Y + position.Height + border_size);

            this.b_color = border_color;
        }

        protected override void onLoad(Game game)
        {
            this.fill_texture = new Texture2D(game.GraphicsDevice, 1, 1);
                this.fill_texture.SetData(new Color[] { this.color });

            this.b_texture = new Texture2D(game.GraphicsDevice, 1, 1);
                this.b_texture.SetData(new Color[] { this.b_color });
        }

        protected override void onUpdate(GameTime gameTime)
        { }

        protected override void onDraw(SpriteBatch spriteBatch)
        {
            // Dibujamos primero el borde
            spriteBatch.Draw(this.b_texture, new Rectangle(
                                                (int) this.b_top_left.X,
                                                (int) this.b_top_left.Y,
                                                (int) (this.b_bot_right.X - this.b_top_left.X),
                                                (int) (this.b_bot_right.Y - this.b_top_left.Y)), this.b_color);

            // Dibujamos el cuadro
            spriteBatch.Draw(this.fill_texture, new Rectangle((int)this.x, (int)this.y, (int)this.width, (int)this.height), this.color);
            //spriteBatch.Draw(this.fill_texture, this.getPosition(), null, this.color, this.rotation, new Vector2(), this.scale, SpriteEffects.None, 0f);
        }
    }
}
