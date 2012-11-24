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

using XNAGraphics.KinectBundle;

namespace XNAGraphics.ComponentBundle.DrawableBundle
{
    class KinectVideo : XNAGraphics.KernelBundle.BasicsBundle.BasicDrawable
    {
        Kinect kinect;

        public KinectVideo(int x, int y, Kinect k)
            : base(x, y)
        {
            this.kinect = k;
        }

        protected override void onLoad(Game game)
        { }

        protected override void onUpdate(GameTime gameTime)
        {
            this.sprite = (Texture2D)this.kinect.kinectRGBVideo;
        }

        protected override void onDraw(SpriteBatch spriteBatch)
        {
            // Simple texture draw with a rectangle 0, 0, 640, 480, texture kinectRGBVideo y color.white
            Texture2D texture = (Texture2D)this.sprite;
            spriteBatch.Draw(texture, this.getPosition(), new Rectangle(0, 0, texture.Width, texture.Height), this.color, this.rotation, new Vector2(), this.scale, SpriteEffects.None, 0f);
        }
    }
}
