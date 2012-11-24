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

namespace XNAGraphics.KernelBundle
{
    class Core : XNAGraphics.KernelBundle.BasicsBundle.BasicCore
    {
        Kinect kinect;

        // TODO: ¿Esto aquí? Ya se verá... (Está aquí para que en los cambios de pantalla no se note el cambio brusco)
        Layer background;

        public Core(Game1 game)
            : base(game)
        {
            this.kinect = new Kinect();

            // TODO: CREAR SCROLLING_TEXT Y --SCROLLING_IMAGE-- Y SCROLLING_ANIMATION?? Y TILE Y SCROLLING_TILE??
        }

        public override Boolean onInitialize()
        {
            this.kinect.initialize();
            return true;
        }

        public override Boolean onLoadContent()
        {
            // Texture2D
            this.content.add("bg", "background");
            this.content.add("bgnew", "new_bg");
            this.content.add("btn.menu", "menu");
            this.content.add("btn.play", "play");
            this.content.add("btn.next", "next");
            this.content.add("btn.exit", "exit");
            this.content.add("btn.continue", "continue");
            this.content.add("btn.pause", "pause");

            // Del video
            this.content.add("video.header", "video_header");
            this.content.add("video.footer", "video_footer");
            this.content.add("video.waiting", "waiting");

            // SpriteFont
            this.content.add("arial", "arial");
            this.content.add("grobold", "grobold");

            // Esto se hace siempre para que el ContentHandler lo cargue despues de haber añadido todas las texturas a manubrio
            this.content.load();

            // Inicializamos nuestro señor fondo (que nos va a servir para todos y sin cambios bruscos al tenerlo como variable de clase)
            this.background = new Layer("Background", new ScrollingImage(this.content.get("bgnew"), this.game.graphics, 0, 0, Color.White, 30, 1f, 0), 1000);

            /**
             * Pantalla de inicio
             */
            LayerCollection home = new LayerCollection("Inicio",
                this.background,
                new Layer("Logo del juego",
                    //new Image(this.content.get("logo"), 30, 30)
                    new Text(this.content.get("arial"), "Personal Training: Martial Arts", 100, 100)
                ),
                new Layer("Btn play",
                    new Image(this.content.get("btn.play"), 30, this.game.GraphicsDevice.Viewport.Height - 160)
                ),
                new Layer("Btn exit",
                    new Image(this.content.get("btn.exit"), 300, this.game.GraphicsDevice.Viewport.Height - 160)
                )
            ); r.add(home);

            /**
             * Mostrar una postura a imitar
             */
            LayerCollection showPosture = new LayerCollection("Mostrar postura",
                this.background,
                new Layer("Contenedor video",
                    new Panel(new Rectangle(28, 28, 644, 543), Color.Black * 0.95f, 2, Color.Gray * 0.9f)
                ),
                new Layer("video_header",
                    new Image(this.content.get("video.header"), 30, 30)
                ),
                new Layer("Postura",
                    // TODO: Aquí va un skeleton
                    new Image(this.content.get("video.waiting"), 30, 78)
                ),
                new Layer("video_footer",
                    new Image(this.content.get("video.footer"), 30, 558)
                ),
                new Layer("Btn continue",
                    new Image(this.content.get("btn.continue"), 300, this.game.GraphicsDevice.Viewport.Height - 160)
                )
            ); r.add(showPosture);

            /**
             * Detectar postura
             */
            LayerCollection detectPosture = new LayerCollection("Detectar postura",
                this.background,
                new Layer("Contenedor video",
                    new Panel(new Rectangle(28, 28, 644, 543), Color.Black * 0.95f, 2, Color.Gray * 0.9f)
                ),
                new Layer("video_header",
                    new Image(this.content.get("video.header"), 30, 30)
                ),
                new Layer("Kinect RGB Video",
                    new KinectVideo(30, 78, this.kinect)
                    //new Panel(new Rectangle(30, 30, 640, 480), Color.AliceBlue)
                ),
                new Layer("video_footer",
                    new Image(this.content.get("video.footer"), 30, 558)
                ),
                new Layer("Texto central",
                    new BorderedText(this.content.get("grobold"), "Casi lo tienes, quedate quieto!", this.game.GraphicsDevice.Viewport.Width / 2, this.game.GraphicsDevice.Viewport.Height / 2, Color.ForestGreen, 3f, Color.Black)
                ),
                new Layer("Btn pause",
                    new Image(this.content.get("btn.pause"), 300, this.game.GraphicsDevice.Viewport.Height - 160)
                )
            ); r.add(detectPosture);

            /**
             * Pantalla de pausa
             */
            LayerCollection pause = new LayerCollection("Pausa",
                this.background,
                new Layer("Btn continue",
                    new Image(this.content.get("btn.continue"), 0, this.game.GraphicsDevice.Viewport.Height - 160)
                ),
                new Layer("Btn replay",
                    new Image(this.content.get("btn.replay"), 300, this.game.GraphicsDevice.Viewport.Height - 160)
                ),
                new Layer("Btn exit",
                    new Image(this.content.get("btn.exit"), 500, this.game.GraphicsDevice.Viewport.Height - 160)
                )
            ); r.add(pause);

            /**
             * Pantalla de puntuación (posturíl)
             */
            LayerCollection postureScore = new LayerCollection("Puntuación de postura",
                this.background,
                new Layer("Texto central",
                    new BorderedText(this.content.get("grobold"), "Puntuación de la postura: 5.782", 100, this.game.GraphicsDevice.Viewport.Height/2, Color.DarkRed, 3f, Color.Black)
                ),
                new Layer("Btn next",
                    new Image(this.content.get("btn.next"), 0, this.game.GraphicsDevice.Viewport.Height - 160)
                ),
                new Layer("Btn replay",
                    new Image(this.content.get("btn.replay"), 300, this.game.GraphicsDevice.Viewport.Height - 160)
                ),
                new Layer("Btn exit",
                    new Image(this.content.get("btn.exit"), 500, this.game.GraphicsDevice.Viewport.Height - 160)
                )
            ); r.add(postureScore);

            /**
             * Pantalla de puntuación (final)
             */
            LayerCollection final_score = new LayerCollection("Puntuación final",
                this.background,
                new Layer("Texto central",
                    new BorderedText(this.content.get("grobold"), "Puntuación final: 3.983", 100, this.game.GraphicsDevice.Viewport.Height/2, Color.DarkRed, 3f, Color.Black)
                ),
                new Layer("Btn end",
                    new Image(this.content.get("btn.end"), 0, this.game.GraphicsDevice.Viewport.Height - 60)
                ),
                new Layer("Btn replay",
                    new Image(this.content.get("btn.replay"), 300, this.game.GraphicsDevice.Viewport.Height - 60)
                )
            ); r.add(final_score);

            this.kinect.load(this.game);

            return true;
        }

        public override Boolean onUnloadContent()
        {
            this.kinect.unload();
            return true;
        }

        public override LayerCollection onUpdate(GameTime gameTime)
        {
            this.r.get("Detectar postura").get("Texto central").drawable.addMovement(new Screw(1.05f, 0f, 30));
            this.r.get("Detectar postura").get("Texto central").drawable.addMovement(new Screw(1f, 0f, 30));
            return this.r.get("Detectar postura");
        }

        public override LayerCollection onDraw(GameTime gameTime)
        {
            return this.r.get("Detectar postura");
        }
    }
}
