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
    abstract class BasicDrawable
    {
        public Object sprite;
        public Color color;
        public float x;
        public float y;

        public float scale;
        public float rotation;

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
        }

        public Vector2 getPosition()
        {
            return new Vector2(this.x, this.y);
        }

        public abstract void load(Game game);

        public abstract void update(GameTime gameTime);

        public abstract void draw(SpriteBatch spriteBatch);
    }
}
