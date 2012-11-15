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

using Registry = XNAGraphics.KernelBundle.BasicsBundle.BasicRegistry;
using XNAGraphics.KernelBundle.BasicsBundle;
using XNAGraphics.ComponentBundle.LayerBundle;
using XNAGraphics.ComponentBundle.DrawableBundle;
using XNAGraphics.KinectBundle;

namespace XNAGraphics.KernelBundle
{
    class Core
    {
        Game1 game;
        Screen screen;
        ContentHandler content;
        Registry r;
        public Kinect kinect;

        public Core(Game1 game)
        {
            this.game = game;
            this.screen = new Screen();
            this.content = new ContentHandler(this.game.Content);
            this.r = new Registry();
            this.kinect = new Kinect();

            // TODO: CREAR SCROLLING_TEXT Y --SCROLLING_IMAGE-- Y SCROLLING_ANIMATION?? Y TILE Y SCROLLING_TILE??
        }

        public Boolean Initialize()
        {
            this.content = new ContentHandler(this.game.Content);
            this.kinect.initialize();

            return true;
        }

        public Boolean LoadContent()
        {
            // Texture2D
            this.content.add("bg", "background");
            this.content.add("bgnew", "new_bg");
            this.content.add("menu", "menu");
            this.content.add("play", "play");
            this.content.add("next", "next");
            this.content.add("stars", "starfield");

            // Texture2D: Animation
            this.content.add("ship", "ship");
            this.content.add("mario", "mario_running");
            this.content.add("robot", "walk_iso");

            // SpriteFont
            this.content.add("arial", "arial");
            this.content.add("grobold", "grobold");

            this.content.load();
            this.screen.loadContent(this.game.GraphicsDevice);

            LayerCollection juego = new LayerCollection(
                new Layer( // Background
                    new ScrollingImage(this.content.get("bgnew"), this.game.graphics, 0, 0, Color.White, 30, 1f, 0),
                    1000
                ),
                new Layer( // Background panel kinect (izquierda)
                    new Panel(new Rectangle(30, 30, 740, this.game.GraphicsDevice.Viewport.Height - 60), Color.Black * 0.9f, 2, Color.Gray * 0.5f)
                ),
                new Layer( // Background panel derecha
                    new Panel(new Rectangle(800, 30, 610, this.game.GraphicsDevice.Viewport.Height - 60), Color.Black * 0.9f, 2, Color.Gray * 0.5f)
                ),
                new Layer( // Panel kinect
                    //new Panel(new Rectangle(80, 80, 640, 480), Color.CornflowerBlue * 0.9f)
                    new KinectVideo(80, 80, this.kinect)
                ),
                new Layer( // Panel de postura actual
                    new Panel(new Rectangle(850, 80, 510, 480), Color.CornflowerBlue * 0.5f)
                ),
                new Layer( // Puntos totales
                    new Text(this.content.get("grobold"), "34201 Puntos", 830, 800, new Color(242, 242, 39) * 0.9f)
                )
            ); r.add(juego);

            // Cargamos lo necesario para nuestras capitas
            foreach (LayerCollection layer_collection in this.r.components)
            {
                layer_collection.sortByPriority();

                foreach (Layer layer in layer_collection.components)
                {
                    layer.drawable.load(this.game);
                }
            }

            this.kinect.load(this.game);

            return true;
        }

        public Boolean UnloadContent()
        {
            this.kinect.unload();
            return true;
        }

        public Boolean Update(GameTime gameTime)
        {
            this.screen.update(gameTime, r);
            return true;
        }

        public Boolean Draw(GameTime gameTime)
        {
            this.screen.draw(r);
            return true;
        }
    }
}
