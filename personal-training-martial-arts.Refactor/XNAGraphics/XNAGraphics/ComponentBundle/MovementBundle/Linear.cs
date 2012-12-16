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

namespace XNAGraphics.ComponentBundle.MovementBundle
{
    public class Linear : XNAGraphics.KernelBundle.BasicsBundle.BasicMovement
    {
        float a, b; // Lados de nuestro triángulo
        float a_speed, b_speed; // Velocudades de nuestros laterales

        float s; // Diferencia de escalas
        float s_speed; // Velocidad de escala

        public Linear(float to_x, float to_y, int speed)
            : base(to_x, to_y, 1f, 0f, speed) { }

        public Linear(float to_x, float to_y, int speed, Boolean infinite)
            : base(to_x, to_y, 1f, 0f, speed, infinite) { }

        public Linear(float to_x, float to_y, int speed, float scale)
            : base(to_x, to_y, scale, 0f, speed) { }

        public Linear(float to_x, float to_y, int speed, float scale, Boolean infinite)
            : base(to_x, to_y, scale, 0f, speed, infinite) { }

        public override void onReady()
        {
            // Calculamos los catetos
            this.a = this.to_x - this.drawable.x;
            this.b = this.to_y - this.drawable.y;

            // Calculamos la velocidad de nuestros laterales
            this.a_speed = this.a / this.speed;
            this.b_speed = this.b / this.speed;

            // Calculamos la distancia de escalas y la velocidad de escalas
            this.s = this.to_scale - this.drawable.scale;
            this.s_speed = this.s / this.speed;
        }

        public override void onUpdate(GameTime gameTime)
        {
            this.drawable.x = this.drawable.x + this.a_speed;
            this.drawable.y = this.drawable.y + this.b_speed;
            this.drawable.scale = this.drawable.scale + this.s_speed;
        }
    }
}
