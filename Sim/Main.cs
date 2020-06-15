using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sim.Objects;
using System;
using System.Collections.Generic;

namespace Sim
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Game
    {
        public static int screenHeight = 720;
        public static int screenWidth = 1280;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // sprites
        private Texture2D pixelTexture;
        private Texture2D cursorTexture;
        private SpriteFont font;

        private Color backgroundColor = Color.White;
        private double deltaTime = 0.0;

        private Dictionary<int, Dot> dots = new Dictionary<int, Dot>();
        private double currentTime;

        private Cursor cursor = null;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            pixelTexture = Content.Load<Texture2D>("pixel");
            cursorTexture = Content.Load<Texture2D>("cursor");

            font = Content.Load<SpriteFont>("font");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            //double deltaTime = gameTime.ElapsedGameTime.TotalSeconds * 10;

            MouseState state = Mouse.GetState();

            // Check if Right Mouse Button pressed, if so, exit
            if (state.LeftButton == ButtonState.Pressed && !dots.ContainsKey(Dot.GetKey(state.X, state.Y)))
            {
                // Update our sprites position to the current cursor
                Dot dot = new Dot(pixelTexture, new Vector2(state.X, state.Y));
                dots.Add(dot.GetKey(), dot);
            }


            currentTime += gameTime.ElapsedGameTime.TotalSeconds; //Time passed since last Update()
            deltaTime = gameTime.ElapsedGameTime.TotalSeconds;

            double countDuration = .0;
            if (currentTime >= countDuration && !Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                Step();

                //currentTime -= countDuration;
            }

            // Update cursor position
            if (cursor == null)
            {
                cursor = new Cursor(cursorTexture);
            }
            cursor.Update(state, gameTime.TotalGameTime.TotalSeconds);

            base.Update(gameTime);
        }

        private void Step()
        {
            List<int> remove = new List<int>();
            HashSet<int> check = new HashSet<int>();
            List<int> add = new List<int>();

            foreach (KeyValuePair<int, Dot> dot in dots)
            {
                dot.Value.Update(dots, ref check);

                if (dot.Value.Dead)
                {
                    remove.Add(dot.Key);
                }
            }

            foreach (int position in check)
            {
                if (Dot.CheckNeighbours(dots, position))
                {
                    add.Add(position);
                }
            }

            foreach (int key in remove)
            {
                dots.Remove(key);
            }

            foreach (int key in add)
            {
                dots.Add(key, new Dot(pixelTexture, key));
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(backgroundColor);

            // TODO: Add your drawing code here
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            // Background
            foreach (KeyValuePair<int, Dot> dot in dots)
            {
                dot.Value.Draw(spriteBatch);
            }
            
            // Text/UI
            spriteBatch.DrawString(font, "Score: " + 1234567890, new Vector2(100, 100), Color.Black);

            // Cursor
            cursor.Draw(spriteBatch);

            // FPS
            spriteBatch.DrawString(font, Math.Round(1d / gameTime.ElapsedGameTime.TotalSeconds).ToString(), new Vector2(0, 0), Color.Red);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
