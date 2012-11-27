using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Registry = XNAGraphics.KernelBundle.BasicsBundle.BasicRegistry;
using XNAGraphics.KernelBundle.BasicsBundle;
using XNAGraphics.ComponentBundle.LayerBundle;
using XNAGraphics.ComponentBundle.MovementBundle;
using XNAGraphics.ComponentBundle.DrawableBundle;
using XNAGraphics.KinectBundle;

namespace XNAGraphics.KernelBundle.BasicsBundle
{
    public abstract class BasicCore
    {
        protected Game1 game;
        protected Screen screen;
        protected ContentHandler content;
        protected Registry r;

        public BasicCore(Game1 game)
        {
            this.game = game;
            this.screen = new Screen();
            this.content = new ContentHandler(this.game.Content);
            this.r = new Registry();
        }

        public Boolean Initialize()
        {
            this.content = new ContentHandler(this.game.Content);
            return onInitialize();
        }

        public abstract Boolean onInitialize();

        public Boolean LoadContent()
        {
            Boolean init = onLoadContent();

            // Inicializamos el SpriteBatch y más cosas que tenga la pantalla...
            this.screen.loadContent(this.game.GraphicsDevice);

            // Cargamos lo necesario para nuestras capitas
            foreach (LayerCollection layer_collection in this.r.components)
            {
                layer_collection.sortByPriority();

                foreach (Layer layer in layer_collection.components)
                {
                    layer.drawable.load(this.game);
                }
            }

            return init;
        }

        public abstract Boolean onLoadContent();

        public Boolean UnloadContent()
        {
            return onUnloadContent();
        }

        public abstract Boolean onUnloadContent();

        public Boolean Update(GameTime gameTime)
        {
            this.screen.update(gameTime, onUpdate(gameTime));
            return true;
        }

        public abstract LayerCollection onUpdate(GameTime gameTime);

        public Boolean Draw(GameTime gameTime)
        {
            this.screen.draw(onDraw(gameTime));
            return true;
        }

        public abstract LayerCollection onDraw(GameTime gameTime);
    }
}
