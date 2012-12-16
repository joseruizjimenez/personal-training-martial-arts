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
    public class Screw : XNAGraphics.KernelBundle.BasicsBundle.BasicMovement
    {
        float s, r;
        float s_speed, r_speed;

        public Screw(float scale, float rotation, int speed)
            : base(-0.1f, -0.1f, scale, rotation, speed) { }

        public Screw(float scale, float rotation, int speed, Boolean infinite)
            : base(-0.1f, -0.1f, scale, rotation, speed, infinite) { }

        public override void onReady()
        {
            // Calculamos los catetos
            this.s = this.to_scale - this.drawable.scale;
            this.r = this.to_rotation - this.drawable.rotation;

            // Calculamos la velocidad de nuestros laterales
            this.s_speed = this.s / this.speed;
            this.r_speed = this.r / this.speed;
        }

        public override void onUpdate(GameTime gameTime)
        {
            this.drawable.scale = this.drawable.scale + this.s_speed;
            this.drawable.rotation = this.drawable.rotation + this.r_speed;
        }
    }
}
