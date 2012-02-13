using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace MathInfection
{
    public class Main : Game
    {
        GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont hudFont;
        private GamePadState oldGamePadState;
        private KeyboardState oldKeyboardState;

        private HeadsUpDisplay hud;

        private Player player1;
        private Player player2;
        private List<Enemy> enemyList;
        private List<Boss> bossList;
        private List<Bullet> defaultBulletList;
        private Texture2D player1Texture;
        private List<Texture2D> enemyTexList;
        private List<Texture2D> bossTexList;
        private List<Texture2D> bulletTexList;

        private Vector2 playerSize;
        private Vector2 windowSize;
        private Vector2 initialPlayerPosition;
        private Vector2 playerVelocity;

        private bool singleMode;
        private int numPlayers;
        private int numMoveStrategies;
        private int numEnemies;
        private TimeSpan previousFireTime;
        private TimeSpan defaultBulletFireRate;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 700;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "Math Infection";

            oldGamePadState = GamePad.GetState(PlayerIndex.One);
            hud = new HeadsUpDisplay();
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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            hudFont = Content.Load<SpriteFont>("HUDFont");

            player1Texture = Content.Load<Texture2D>(@"CharacterImages/Player1");
            playerSize = new Vector2(player1Texture.Width, player1Texture.Height);
            if(!singleMode)
            {
                // TODO: use player1's texture for now, might make a different tex for player2 later.
                // player2Texture = Content.Load<Texture2D>(@"CharacterImages/Player2");
            }
            player1 = new Player(player1Texture, initialPlayerPosition, playerVelocity, playerSize, windowSize);

            enemyTexList.Add(Content.Load<Texture2D>(@"CharacterImages/Boss1"));
            Vector2 charSize = new Vector2(enemyTexList[0].Width, enemyTexList[0].Height);
            int numEnemy = numEnemies;
            while(numEnemy > 0)
            {
                enemyList.Add(new Enemy(RandomGenerator.RandomMoveStrategy(numMoveStrategies), enemyTexList[0],
                                    RandomGenerator.RandomPosition(windowSize, charSize), charSize, windowSize, 100,
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
            CheckInput(gameTime);
            player1.update(Vector2.Zero);

            foreach(Enemy e in enemyList)
            {
                e.update(player1.PlayerPosition);
            }
            foreach(Bullet b in defaultBulletList)
            {
                b.update(player1.PlayerPosition);
            }
            int index = 0;
            while(index < enemyList.Count)
            {
                if(!enemyList[index].IsAlive())
                {
                    enemyList.RemoveAt(index);
                }
                index++;
            }
            index = 0;
            while(index < defaultBulletList.Count)
            {
                if(!defaultBulletList[index].IsValid)
                {
                    defaultBulletList.RemoveAt(index);
                }
                index++;
            }
            UpdateBulletCollision();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            foreach(Enemy e in enemyList)
            {
                e.draw(spriteBatch);
            }
            foreach(Bullet b in defaultBulletList)
            {
                b.draw(spriteBatch);
            }
            player1.draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void CheckInput(GameTime gameTime)
        {
            // BoostButton pressing
            GamePadState newGamePadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState newKeyboardState = Keyboard.GetState();
            // TODO: assume user wouldn't switch between keyboard and gamepad while speeding up for now
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
            // endof BoostButton pressing

            // Bullet Generation
            if (gameTime.TotalGameTime - previousFireTime > defaultBulletFireRate)
            {
                if (newGamePadState.Triggers.Right > .2f || newKeyboardState.IsKeyDown(Keys.Space))
                {
                    if (!(oldGamePadState.Triggers.Right > .25f) || !oldKeyboardState.IsKeyDown(Keys.Space))
                    {
                        Vector2 bSize = new Vector2(bulletTexList[0].Width, bulletTexList[0].Height);
                        Vector2 bPos = new Vector2(player1.PlayerPosition.X + player1.CharacterSize.X / 2,
                                                   player1.PlayerPosition.Y);
                        defaultBulletList.Add(new Bullet(bulletTexList[0], bPos, bSize, windowSize, Vector2.Zero, 10, 20));
                        previousFireTime = gameTime.TotalGameTime;
                    }
                }
            }
            // endof Bullet Generation
        }

        private void UpdateBulletCollision()
        {
            if(defaultBulletList.Count <1)
            {
                return;
            }
            Rectangle r1 = new Rectangle();
            Rectangle r2 = new Rectangle();
            foreach(Bullet b in defaultBulletList)
            {
                r1.Width  = (int)Math.Round(b.CharacterSize.X);
                r1.Height = (int)Math.Round(b.CharacterSize.Y);
                r1.X = (int)Math.Round(b.Position.X);
                r1.Y = (int)Math.Round(b.Position.Y);
                foreach(Enemy e in enemyList)
                {
                    r2.Width  = (int)Math.Round(e.CharacterSize.X);
                    r2.Height = (int)Math.Round(e.CharacterSize.Y);
                    r2.X = (int)Math.Round(e.Position.X);
                    r2.Y = (int)Math.Round(e.Position.Y);

                    if(r1.Intersects(r2))
                    {
                        e.WasHit = true;
                        e.DamageReceived = b.Damage;
                        b.IsValid = false;
                    }
                }
            }
        }
        // endof UpdateBulletCollision()
    }
}
