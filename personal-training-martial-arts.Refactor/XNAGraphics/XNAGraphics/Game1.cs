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

using XNAGraphics.KernelBundle;

namespace XNAGraphics
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager graphics;
        Core core;

        public Game1()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.core = new Core(this);

            // INFO: Resolución de pantalla
            this.graphics.PreferredBackBufferWidth = 1440;//640;
            this.graphics.PreferredBackBufferHeight = 900;//480;

            // INFO: Pantalla completa
            this.graphics.IsFullScreen = false;

            // INFO: Enseñamos el cursor en la ventana XNA
            this.IsMouseVisible = true;

            this.Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            if (!this.core.Initialize())
                throw new Exception("Couldn't initialize");

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        ///// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
            if (!this.core.LoadContent())
                throw new Exception("Couldn't load content");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            if (!this.core.UnloadContent())
                throw new Exception("Couldn't load content");
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();


            KeyboardState aCurrentKeyboardState = Keyboard.GetState();

            if (aCurrentKeyboardState.IsKeyDown(Keys.Up) == true)
            {
                this.graphics.PreferredBackBufferWidth += 50;
                this.graphics.PreferredBackBufferHeight += 50;
                this.graphics.ApplyChanges();
            }
            else if (aCurrentKeyboardState.IsKeyDown(Keys.Down) == true)
            {
                if (
                    ((this.graphics.PreferredBackBufferWidth - 50) >= 0) && 
                    ((this.graphics.PreferredBackBufferHeight - 50) >= 0)
                   )
                {
                    this.graphics.PreferredBackBufferWidth -= 50;
                    this.graphics.PreferredBackBufferHeight -= 50;
                    this.graphics.ApplyChanges();
                }
            }
            if (aCurrentKeyboardState.IsKeyDown(Keys.Escape) == true)
                this.Exit();


            // TODO: Add your update logic here
            if (!this.core.Update(gameTime))
                throw new Exception("Couldn't update");

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // INFO: Color de fondo pantalla por defecto
            this.GraphicsDevice.Clear(new Color(100, 100, 100));

            if (!this.core.Draw(gameTime))
                throw new Exception("Couldn't draw");

            base.Draw(gameTime);
        }
    }
}
