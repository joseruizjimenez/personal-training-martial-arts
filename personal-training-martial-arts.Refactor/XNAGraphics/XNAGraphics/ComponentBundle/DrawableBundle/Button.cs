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
    class Button : XNAGraphics.KernelBundle.BasicsBundle.BasicDrawable
    {
        public enum BState
        {
            HOVER,
            UP,
            JUST_RELEASED,
            DOWN
        }

        public Rectangle rectangle;
        public BState state;
        private Boolean mpressed;
        private Boolean prev_mpressed;

        public Button(Object texture, int x, int y)
            : base(x, y)
        {
            this.sprite = texture;
            Texture2D t = (Texture2D) this.sprite;
            this.rectangle = new Rectangle(x, y, t.Width, t.Height);
            state = BState.UP;
            mpressed = false;
            prev_mpressed = false;
        }

        public BState updateState()
        {
            MouseState mouse_state = Mouse.GetState();
            int mx = mouse_state.X;
            int my = mouse_state.Y;
            prev_mpressed = mpressed;
            mpressed = mouse_state.LeftButton == ButtonState.Pressed;

            // raton encima del boton
            if (hit_button(this.rectangle, mx, my))
            {
                // si se esta presionando el boton
                if (mpressed)
                {
                    state = BState.DOWN;
                    this.color = Color.Red;
                }
                // si ha habido un click y el boton estaba pulsado
                else if (!mpressed && prev_mpressed)
                {
                    if (state == BState.DOWN)
                    {
                        state = BState.JUST_RELEASED;
                        //this.color = Color.Green;
                    }
                }
                // si el raton esta encima del boton
                else
                {
                    state = BState.HOVER;
                    this.color = Color.White * 0.7f;
                }
            }
            // si el raton no esta sobre el boton
            else
            {
                state = BState.UP;
                this.color = Color.White;
            }

            return state;
        }

        private Boolean hit_button(Rectangle button, int mx, int my)
        {
            return (mx >= button.X &&
                mx <= button.X + button.Width &&
                my >= button.Y &&
                my <= button.Y + button.Height);
        }

        public Boolean justPushed()
        {
            if (this.state == BState.JUST_RELEASED)
                return true;
            else
                return false;
        }

        protected override void onLoad(Game game)
        { }

        protected override void onUpdate(GameTime gameTime)
        {
            this.updateState();
        }

        protected override void onDraw(SpriteBatch spriteBatch)
        {
            Texture2D texture = (Texture2D)this.sprite;
            spriteBatch.Draw(texture, this.getPosition(), new Rectangle(0, 0, texture.Width, texture.Height), this.color, this.rotation, new Vector2(), this.scale, SpriteEffects.None, 0f);
        }
    }
}
