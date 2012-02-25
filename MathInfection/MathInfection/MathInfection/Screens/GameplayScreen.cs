using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MathInfection
{
    public class GameplayScreen : GameScreen
    {
        private ContentManager content;
        private SpriteFont gameFont;
        private float pauseAlpha;

        private GraphicsDeviceManager graphics;
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

        public GameplayScreen(ScreenManager sMgr, bool isNewGame)
        {
            TransitionOnTime  = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(.5);

            ScreenManager = sMgr;
            GameplayInit(isNewGame);
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


            enemyList = new List<Enemy>();
            bossList = new List<Boss>();
            defaultBulletList = new List<Bullet>();
            enemyTexList = new List<Texture2D>();
            bossTexList = new List<Texture2D>();
            bulletTexList = new List<Texture2D>();
            windowSize = new Vector2(Window.ClientBounds.Width, Window.ClientBounds.Height);
            initialPlayerPosition = new Vector2(windowSize.X / 2, windowSize.Y - 60);
            playerVelocity = new Vector2(6, 6);
            // TODO: determine game mode: single or versus. Use single for now.
            singleMode = true;
            numPlayers = singleMode ? 1 : 2;
            numMoveStrategies = 3;
            numEnemies = 10;
            previousFireTime = TimeSpan.Zero;
            defaultBulletFireRate = TimeSpan.FromSeconds(.15f);
            hud = new HeadsUpDisplay(new Vector2(windowSize.X / 2 - 200, 20));
            base.Initialize();
        }

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

        public override void UnloadContent()
        {
            content.Unload();
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

        private void GameplayInit(bool isNewGame)
        {}

        private void GameplayLoad()
        {}

        private void GameplayUpdate(GameTime gameTime)
        {}

        private void GameplayDraw(GameTime gameTime)
        {}
    }
}