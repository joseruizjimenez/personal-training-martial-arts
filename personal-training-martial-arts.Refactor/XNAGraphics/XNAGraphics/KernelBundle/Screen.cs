using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Registry = XNAGraphics.KernelBundle.BasicsBundle.BasicRegistry;
using XNAGraphics.ComponentBundle.LayerBundle;

namespace XNAGraphics.KernelBundle
{
    class Screen
    {
        // INFO: [Screen] SpriteBatch.
        public SpriteBatch spriteBatch;

        public Screen()
        {
            // Layer l = new Layer("here_goes_the_sprite");
            // LayerCollection overlay_effects = new LayerCollection();
            // this.layer_registry.add(overlay_effects);
        }

        public Boolean loadContent(GraphicsDevice GraphicsDevice)
        {
            this.spriteBatch = new SpriteBatch(GraphicsDevice);

            return true;
        }

        public Boolean update(GameTime gameTime, Registry layer_registry)
        {
            foreach (LayerCollection layer_collection in layer_registry.components)
            {
                layer_collection.sortByPriority();

                foreach (Layer layer in layer_collection.components)
                {
                    layer.drawable.update(gameTime);
                }
            }

            return true;
        }

        public Boolean draw(Registry layer_registry)
        {
            layer_registry.sortByPriority();
            spriteBatch.Begin();
            
            foreach (LayerCollection layer_collection in layer_registry.components)
            {
                layer_collection.sortByPriority();

                foreach (Layer layer in layer_collection.components)
                {
                    layer.drawable.draw(spriteBatch);
                }
            }

            spriteBatch.End();

            return true;
        }
    }
}
