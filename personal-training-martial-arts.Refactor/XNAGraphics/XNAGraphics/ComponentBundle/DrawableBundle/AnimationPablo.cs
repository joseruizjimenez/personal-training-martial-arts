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
    public class AnimationPablo : XNAGraphics.KernelBundle.BasicsBundle.BasicDrawable
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

        public int  TotalWidth;
        public int  TotalHeight;
        public int SpriteWidth;
        public int SpriteHeight;

        public int totalFrames;
        public int currentFrame;
        public int rows;
        public int columns;

        public int posX = 0;
        public int posY = 0;

        public int contador = 0;



        public AnimationPablo(Object texture, int x, int y, int rows, int columns, int totalFrames)
            : base(x, y, Color.Gray * 0.9f)
        {
            
            this.sprite = texture;
            this.rows = rows;
            this.columns = columns;
            this.totalFrames = totalFrames;
            
            this.currentFrame=0;
            Texture2D text = (Texture2D)this.sprite;
            this.TotalWidth = text.Width;
            this.TotalHeight = text.Height;
            this.SpriteWidth = this.TotalWidth / this.columns;
            this.SpriteHeight = this.TotalHeight / this.rows;
        }
            

        public AnimationPablo(Object texture, Vector2 origin, int frame_count, int frames_per_sec)
            : this(texture, 0, 0, Color.White, origin, frame_count, frames_per_sec, 0f, 0f) { }

        public AnimationPablo(Object texture, int x, int y, Vector2 origin, int frame_count, int frames_per_sec)
            : this(texture, x, y, Color.White, origin, frame_count, frames_per_sec, 0f, 0f) { }

        public AnimationPablo(Object texture, int x, int y, Vector2 origin, int frame_count, int frames_per_sec, float scale)
            : this(texture, x, y, Color.White, origin, frame_count, frames_per_sec, scale, 0f) { }

        public AnimationPablo(Object texture, int x, int y, Vector2 origin, int frame_count, int frames_per_sec, float scale, float rotation)
            : this(texture, x, y, Color.White, origin, frame_count, frames_per_sec, scale, rotation) { }

        public AnimationPablo(Object texture, int x, int y, Color color, Vector2 origin, int frame_count, int frames_per_sec, float scale, float rotation)
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
            

            if (this.IsPaused)
                return;

            

        }

        protected override void onDraw(SpriteBatch spriteBatch)
        {
            Texture2D texture = (Texture2D)this.sprite;


            slow();
            


            
            //currentFrame = ++currentFrame % this.columns;

            int srcX = posX * this.SpriteWidth;
            int srcY = posY * this.SpriteHeight;
            Rectangle source_rectangle = new Rectangle(srcX, srcY, this.SpriteWidth, this.SpriteHeight);
            spriteBatch.Draw(texture, this.getPosition(), source_rectangle, Color.White, this.rotation, this.origin, this.scale, SpriteEffects.None, 0f);
        }

        private void slow() {

            contador++;
            if (contador == 5) {
                contador = 0;
                updateNextFrame(this.currentFrame);
                currentFrame++;
                if (currentFrame > this.totalFrames) currentFrame = 0;
            
            
            }

            
        
        
        }

        private void updateNextFrame(int num) {

            int x = buscaX(num);
            int y = buscaY(num,x);
            this.posX = x;
            this.posY = y;
            
           
        }

        private int buscaX(int Num) {
            int count = 0;
            for (int i = 0; i < this.columns; i++) {
                count = count + this.rows;
                if (count > Num) return i;
            
            }

            return 0;
        }

        private int buscaY(int Num,int x) {
            int count = this.rows * x;
            for (int i = 0; i < this.rows; i++) {
                if (count == Num) return i;
                count++;
            }

            return 0;
        
        }
   

    }
}
