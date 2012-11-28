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
    public abstract class BasicDrawable
    {
        public Object sprite;
        public Color color;
        public float x;
        public float y;

        public float scale;
        public float rotation;
        public BasicCollection movementQueue;
        public BasicMovement actualMovement;

        public Boolean hiden;

        public BasicDrawable(float x, float y)
            : this(x, y, Color.White) { }

        public BasicDrawable(Color color)
            : this(0, 0, color) { }

        public BasicDrawable(float x, float y, Color color)
            : this(x, y, color, 1f, 0f) { }

        public BasicDrawable(float x, float y, Color color, float scale)
            : this(x, y, color, scale, 0f) { }

        public BasicDrawable(float x, float y, Color color, float scale, float rotation)
        {
            this.color = color;
            this.x = x;
            this.y = y;

            this.rotation = rotation;
            this.scale = scale;

            this.movementQueue = new BasicCollection("movementQueue");
            this.actualMovement = null;

            this.hiden = false;
        }

        public Vector2 getPosition()
        {
            return new Vector2(this.x, this.y);
        }

        public void addMovement(BasicMovement m)
        {
            m.drawable = this;
            this.movementQueue.add(m);
        }

        public void load(Game game)
        {
            onLoad(game);
        }

        protected abstract void onLoad(Game game);

        public void update(GameTime gameTime)
        {
            if(this.movementQueue.hasNext() && this.actualMovement == null)
            {
                this.actualMovement = (BasicMovement) this.movementQueue.first();
                this.movementQueue.remove(this.actualMovement);
                this.actualMovement.onReady();
            }

            if (this.actualMovement != null)
            {
                if (!this.actualMovement.isPlaying())
                    this.actualMovement.play();

                this.actualMovement.update(gameTime);

                if (this.actualMovement.isFinished())
                    this.actualMovement = null;
            }

            onUpdate(gameTime);
        }

        protected abstract void onUpdate(GameTime gameTime);

        public void draw(SpriteBatch spriteBatch)
        {
            if (!hiden)
                onDraw(spriteBatch);
        }

        protected abstract void onDraw(SpriteBatch spriteBatch);
    }
}
