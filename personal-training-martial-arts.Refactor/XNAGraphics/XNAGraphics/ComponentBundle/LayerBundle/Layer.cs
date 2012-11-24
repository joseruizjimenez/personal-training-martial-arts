using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using XNAGraphics.KernelBundle.BasicsBundle;

namespace XNAGraphics.ComponentBundle.LayerBundle
{
    class Layer
    {
        public BasicDrawable drawable;
        public int priority;
        public string identifier;

        public Layer(string identifier, BasicDrawable drawable)
            : this(identifier, drawable, 999) { }

        public Layer(string identifier, BasicDrawable drawable, int priority)
        {
            this.identifier = identifier;
            this.drawable = drawable;
            this.priority = priority;
        }

        /// <summary>
        /// Generates a new Vector2 based on layer's coordinates.
        /// </summary>
        /// <returns>A new Vector2 with layer's coordinates.</returns>
        public Vector2 getCoordinatesAsVector()
        {
            return new Vector2(this.drawable.x, this.drawable.y);
        }
    }
}
