<<<<<<< HEAD
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace MathInfection
{
    public class Main : Game
    {
        GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont hudFont;
        private bool windowMode;
=======
using Microsoft.Xna.Framework;

namespace MathInfection
{
    public class Main : Game
    {
        private GraphicsDeviceManager graphics;
        private ScreenManager screenManager;
>>>>>>> 2b6ae63cf727e98dd6fcaf91ff4d7e699c286703

        private static readonly string[] preloadAssets = { "gradient" };

        public Main()
        {
            Content.RootDirectory = "Content";

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth  = 1000;
            graphics.PreferredBackBufferHeight = 660;

<<<<<<< HEAD
        private bool singleMode;
        private int numPlayers;
        private int numMoveStrategies;

        //Currently working on
        private Texture2D powerUpTex_shield;
        Enhancement HandlePowerUps;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        private int numEnemies;
        private TimeSpan previousFireTime;
        private TimeSpan defaultBulletFireRate;
=======
            screenManager = new ScreenManager(this);
            Components.Add(screenManager);
>>>>>>> 2b6ae63cf727e98dd6fcaf91ff4d7e699c286703

            screenManager.AddScreen(new BackgroundScreen(), null);
            screenManager.AddScreen(new MainMenuScreen(), null);
        }

        protected override void LoadContent()
        {
            foreach(string asset in preloadAssets)
            {
                Content.Load<object>(asset);
            }
        }
<<<<<<< HEAD

        public bool WindowMode
        {
            set
            {
                windowMode = value;
            }
            get
            {
                return windowMode;
            }
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 660;
            graphics.IsFullScreen = false;
            //graphics.PreferredBackBufferWidth = 2560;
            //graphics.PreferredBackBufferHeight = 1440;
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            Window.Title = "Math Infection";
            Window.AllowUserResizing = true;
            windowMode = false;

=======
>>>>>>> 2b6ae63cf727e98dd6fcaf91ff4d7e699c286703

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }
<<<<<<< HEAD

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>

            // Create a new SpriteBatch, which can be used to draw textures.
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            hudFont = Content.Load<SpriteFont>("HUDFont");

            Texture2D backgroundTex = Content.Load<Texture2D>("Background");
            background = new Background();
            background.Load(GraphicsDevice, backgroundTex);

            player1Texture = Content.Load<Texture2D>(@"CharacterImages/Player1");
            playerSize = new Vector2(player1Texture.Width, player1Texture.Height);
            if(!singleMode)
            {
                // TODO: use player1's texture for now, might make another for player2 later.
                // player2Texture = Content.Load<Texture2D>(@"CharacterImages/Player2");
            }
            player1 = new Player(player1Texture, initialPlayerPosition, playerVelocity, playerSize, windowSize);
            
            //Currently Working on.
            powerUpTex_shield = Content.Load<Texture2D>(@"PowerUps/Temp-Shield");
            HandlePowerUps = new Enhancement(powerUpTex_shield, playerSize);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
               Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            GameUpdate.CheckInput(gameTime, player1, defaultBulletList, bulletTexList,
                                  previousFireTime, defaultBulletFireRate, windowSize, this);

            player1.update(Vector2.Zero);
            if(!player1.IsAlive())

            {
                //TODO: gameover
            }

            foreach(Enemy e in enemyList)
            {
                e.update(player1.PlayerPosition);
            }

            foreach(Bullet b in defaultBulletList)
            {
                b.update(player1.PlayerPosition);
            }

            GameUpdate.CheckCollision(defaultBulletList, enemyList, player1);
            GameUpdate.UpdateEnemyList(enemyList);
            GameUpdate.UpdateBulletList(defaultBulletList);
            hud.update(player1, enemyList.Count);

            // windowMode = GameUpdate.CheckWindowMode(graphics, this);
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            background.Update(elapsed * 100);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            background.Draw(spriteBatch);
            hud.draw(hudFont, spriteBatch);
            foreach(Bullet b in defaultBulletList)
            {
                b.draw(spriteBatch);
            }
            foreach(Enemy e in enemyList)
            {
                e.draw(spriteBatch);
            }
            player1.draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
=======
    }
>>>>>>> 2b6ae63cf727e98dd6fcaf91ff4d7e699c286703
}