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

namespace XNAGraphics.KernelBundle.BasicsBundle
{
    public abstract class BasicMovement
    {
        public BasicDrawable drawable;
        public int speed;
        public int elapsed;
        public float to_x;
        public float to_y;
        public float to_scale;
        public float to_rotation;
        public Boolean playing;
        public Boolean finished;
        public Boolean infinite;

        public BasicMovement(float to_x, float to_y, float to_scale, float to_rotation, int speed)
            : this(to_x, to_y, to_scale, to_rotation, speed, false) { }

        public BasicMovement(float to_x, float to_y, float to_scale, float to_rotation, int speed, Boolean infinite)
        {
            this.speed = speed;
            this.elapsed = 0;
            this.to_x = to_x;
            this.to_y = to_y;
            this.to_scale = to_scale;
            this.to_rotation = to_rotation;
            this.playing = false;
            this.finished = false;
            this.infinite = infinite;
        }

        public Boolean isPlaying()
        {
            return this.playing;
        }

        public void play()
        {
            this.playing = true;
        }

        public Boolean isFinished()
        {
            return this.finished;
        }

        public abstract void onReady();

        public void update(GameTime gameTime)
        {
            if (this.elapsed == this.speed)
            {
                // FIX: (CREO QUE ESTO YA ESTABA ARREGLADO) Pasar esto al movimiento en sí, porque se le pira la flepa, dejar solo lo del finished! :P
                if ((this.to_x != -0.1f) && (this.to_y != -0.1f))
                {
                    // Para que no quede como movimientos bruscos, al terminar la animación vamos a la posición deseada
                    this.drawable.x = this.to_x;
                    this.drawable.y = this.to_y;
                    this.drawable.scale = this.to_scale;
                    this.drawable.rotation = this.to_rotation;
                }

                if (this.infinite == true)
                {
                    // Si el movimiento es infinito, entonces cuando se llega al final, volvemos al principio de la animación
                    this.elapsed = 0;
                }
                else
                {
                    // Si no es un movimiento infinito, terminamos la animación
                    this.finished = true;
                }
            }
            else
            {
                onUpdate(gameTime);
                this.elapsed++;
            }
        }

        public abstract void onUpdate(GameTime gameTime);
    }
}
