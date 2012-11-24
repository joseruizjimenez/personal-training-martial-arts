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
    class Animation : XNAGraphics.KernelBundle.BasicsBundle.BasicDrawable
    {
        public int frame_count;
        public float time_per_frame;
        public int frame;
        public float total_elapsed;
        public Vector2 origin;
        public bool paused;
        public bool IsPaused
        {
            get { return this.paused; }
        }

        public Animation(Object texture, Vector2 origin, int frame_count, int frames_per_sec)
            : this(texture, 0, 0, Color.White, origin, frame_count, frames_per_sec, 0f, 0f) { }

        public Animation(Object texture, int x, int y, Vector2 origin, int frame_count, int frames_per_sec)
            : this(texture, x, y, Color.White, origin, frame_count, frames_per_sec, 0f, 0f) { }

        public Animation(Object texture, int x, int y, Vector2 origin, int frame_count, int frames_per_sec, float scale)
            : this(texture, x, y, Color.White, origin, frame_count, frames_per_sec, scale, 0f) { }

        public Animation(Object texture, int x, int y, Vector2 origin, int frame_count, int frames_per_sec, float scale, float rotation)
            : this(texture, x, y, Color.White, origin, frame_count, frames_per_sec, scale, rotation) { }

        public Animation(Object texture, int x, int y, Color color, Vector2 origin, int frame_count, int frames_per_sec, float scale, float rotation)
            : base(x, y, color)
        {
            this.sprite = texture;
            this.frame_count = frame_count;
            this.time_per_frame = (float)1 / frames_per_sec;
            this.frame = 0;
            this.total_elapsed = 0;
            this.paused = false;
            this.origin = origin;
            this.scale = scale;
            this.rotation = rotation;
        }

        public void Reset()
        {
            this.frame = 0;
            this.total_elapsed = 0f;
        }

        public void Stop()
        {
            Pause();
            Reset();
        }

        public void Play()
        {
            this.paused = false;
        }

        public void Pause()
        {
            this.paused = true;
        }

        protected override void onLoad(Game game)
        { }

        protected override void onUpdate(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (this.IsPaused)
                return;
            
            

            this.total_elapsed += elapsed;

            if (this.total_elapsed > this.time_per_frame)
            {
                this.frame++;
                // Keep the Frame between 0 and the total frames, minus one.
                this.frame = this.frame % this.frame_count;
                this.total_elapsed -= this.time_per_frame;
            }
        }

        protected override void onDraw(SpriteBatch spriteBatch)
        {
            Texture2D texture = (Texture2D)this.sprite;
            int frame_width = texture.Width / this.frame_count;
            Rectangle source_rectangle = new Rectangle(frame_width * frame, 0, frame_width, texture.Height);
            spriteBatch.Draw(texture, this.getPosition(), source_rectangle, Color.White, this.rotation, this.origin, this.scale, SpriteEffects.None, 0f);
        }
    }
}
