<<<<<<< HEAD
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace MathInfection
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Main : Game
    {
=======
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace MathInfection
{
    public class Main : Game
    {
>>>>>>> 36d0c3cdf80c55c5610441a3a0b74198678b4577
        GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont hudFont;
        private bool windowMode;

        private HeadsUpDisplay hud;

        private Player player1;
        private Player player2;
        private List<Enemy> enemyList;
        private List<Boss> bossList;
        private List<Bullet> defaultBulletList;
        private Texture2D player1Texture;
        private List<Texture2D> enemyTexList;
        private List<Texture2D> bossTexList;
<<<<<<< HEAD
=======
        private List<Texture2D> bulletTexList;
>>>>>>> 36d0c3cdf80c55c5610441a3a0b74198678b4577

        private Vector2 playerSize;
        private Vector2 windowSize;
        private Vector2 initialPlayerPosition;
        private Vector2 playerVelocity;

        private bool singleMode;
        private int numPlayers;
        private int numMoveStrategies;
<<<<<<< HEAD

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
=======
        private int numEnemies;
        private TimeSpan previousFireTime;
        private TimeSpan defaultBulletFireRate;

        private Background background;

        public TimeSpan PreviousFireTime
        {
            set
            {
                previousFireTime = value;
            }
        }

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

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

>>>>>>> 36d0c3cdf80c55c5610441a3a0b74198678b4577
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


            enemyList = new List<Enemy>();
            bossList = new List<Boss>();
            defaultBulletList = new List<Bullet>();
            enemyTexList = new List<Texture2D>();
            bossTexList = new List<Texture2D>();
            bulletTexList = new List<Texture2D>();
            windowSize = new Vector2(Window.ClientBounds.Width, Window.ClientBounds.Height);
            initialPlayerPosition = new Vector2(windowSize.X/2, windowSize.Y-60);
            playerVelocity = new Vector2(6, 6);
            // TODO: determine game mode: single or versus. Use single for now.
            singleMode = true;
            numPlayers = singleMode ? 1 : 2;
            numMoveStrategies = 3;
            numEnemies = 10;
            previousFireTime = TimeSpan.Zero;
            defaultBulletFireRate = TimeSpan.FromSeconds(.15f);
            hud = new HeadsUpDisplay(new Vector2(windowSize.X/2-200, 20));
            base.Initialize();
        }

<<<<<<< HEAD
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
=======
        protected override void LoadContent()
        {
>>>>>>> 36d0c3cdf80c55c5610441a3a0b74198678b4577
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
<<<<<<< HEAD
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

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            CheckBoostKeyPress();

            if(player1.IsAlive())
=======
            player1 = new Player(player1Texture, initialPlayerPosition, playerVelocity,
                                 playerSize, windowSize);

            enemyTexList.Add(Content.Load<Texture2D>(@"CharacterImages/Boss1"));
            Vector2 charSize = new Vector2(enemyTexList[0].Width, enemyTexList[0].Height);
            int numEnemy = numEnemies;
            while(numEnemy > 0)
            {
                enemyList.Add(new Enemy(RandomGenerator.RandomMoveStrategy(numMoveStrategies),
                                                                              enemyTexList[0],
                                        RandomGenerator.RandomPosition(windowSize, charSize),
                                                                   charSize, windowSize, 100,
                                        RandomGenerator.RandomEnemySize(false)));
                numEnemy--;
            }
            bulletTexList.Add(Content.Load<Texture2D>(@"BulletImages/Bullet1"));
        }

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
>>>>>>> 36d0c3cdf80c55c5610441a3a0b74198678b4577
            {
                //TODO: gameover
            }

            foreach(Enemy e in enemyList)
            {
<<<<<<< HEAD
                // TODO: gameover implementation
            }

            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            if(player1.IsAlive())
            {
                player1.draw(spriteBatch);
                HandlePowerUps.ModifyShield(spriteBatch, player1);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void CheckBoostKeyPress()
        {
            GamePadState newGamePadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState newKeyboardState = Keyboard.GetState();
            // NOTE: assume user wouldn't switch between keyboard and gamepad while speeding up
            if (newGamePadState.IsButtonDown(Buttons.A) || newKeyboardState.IsKeyDown(Keys.LeftShift))
            {
                if(!oldGamePadState.IsButtonDown(Buttons.A) || !oldKeyboardState.IsKeyDown(Keys.LeftShift))
                {
                    player1.StartBoost = true;
                }
            }
            else if(oldGamePadState.IsButtonDown(Buttons.A) || oldKeyboardState.IsKeyDown(Keys.LeftShift))
            {
                player1.StartBoost = false;
            }
            oldGamePadState = newGamePadState;
            oldKeyboardState = newKeyboardState;
        }
    }
}
=======
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
}
>>>>>>> 36d0c3cdf80c55c5610441a3a0b74198678b4577
