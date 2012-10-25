using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace personal_training_martial_arts.Core
{
    class Button
    {
        public enum BState
        {
            HOVER,
            UP,
            JUST_RELEASED,
            DOWN
        }

        public Rectangle rectangle;
        public string name;
        public BState state;
        private Boolean mpressed;
        private Boolean prev_mpressed;

        public Button(String name, int x, int y, int width, int height)
        {
            this.name = name;
            this.rectangle = new Rectangle(x, y, width, height);
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
            if(hit_button(this.rectangle, mx, my))
            {
                // si se esta presionando el boton
                if (mpressed)
                {
                    state = BState.DOWN;
                }
                // si ha habido un click y el boton estaba pulsado
                else if (!mpressed && prev_mpressed)
                {
                    if (state == BState.DOWN)
                    {
                        state = BState.JUST_RELEASED;
                    }
                }
                // si el raton esta encima del boton
                else
                {
                    state = BState.HOVER;
                }
            }
            // si el raton no esta sobre el boton
            else
            {
                state = BState.UP;
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
    
    }
}
