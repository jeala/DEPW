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

        public override void LoadContent()
        {
            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }
            gameFont = content.Load<SpriteFont>("gamefont");
            GameplayLoad();
            Thread.Sleep(1000);
            ScreenManager.Game.ResetElapsedTime();
        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if(IsActive)
            {
                GameplayUpdate(gameTime);
            }
        }

        public override void HandleInput(InputState input)
        {
            if(input == null)
                throw new ArgumentNullException("input");

            int playerIndex = 0;
            if(ControllingPlayer.HasValue)
            {
                playerIndex = (int)ControllingPlayer.Value;
            }

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            if(input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.Black, 0, 0);
            SpriteBatch spriteBacth = ScreenManager.SpriteBatch;

            spriteBacth.Begin();
            GameplayDraw(gameTime);
            spriteBacth.End();

            if(TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);
                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        private void GameplayInit(bool isNewGame)
        {
            GameWindow window = ScreenManager.Game.Window;

            enemyList = new List<Enemy>();
            bossList = new List<Boss>();
            defaultBulletList = new List<Bullet>();
            enemyTexList = new List<Texture2D>();
            bossTexList = new List<Texture2D>();
            bulletTexList = new List<Texture2D>();
            windowSize = new Vector2(window.ClientBounds.Width, window.ClientBounds.Height);
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
        }

        private void GameplayLoad()
        {
            spriteBatch = ScreenManager.SpriteBatch;
            hudFont = content.Load<SpriteFont>("HUDFont");
            background = new Background();
            background.Load(ScreenManager.GraphicsDevice, content.Load<Texture2D>("BloodVein"));

            player1Texture = content.Load<Texture2D>(@"CharacterImages/Player1");
            playerSize = new Vector2(player1Texture.Width, player1Texture.Height);
            if(!singleMode)
            {
                // TODO: use player1's texture for now, might make another for player2 later.
                // player2Texture = Content.Load<Texture2D>(@"CharacterImages/Player2");
            }
            player1 = new Player(player1Texture, initialPlayerPosition, playerVelocity,
                                 playerSize, windowSize);

            enemyTexList.Add(content.Load<Texture2D>(@"CharacterImages/Boss1"));
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
            bulletTexList.Add(content.Load<Texture2D>(@"BulletImages/Bullet1"));
        }

        private void GameplayUpdate(GameTime gameTime)
        {
            GameUpdate.CheckInput(gameTime, player1, defaultBulletList,
                                  bulletTexList, previousFireTime,
                                  defaultBulletFireRate, windowSize, this);

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
        }

        private void GameplayDraw(GameTime gameTime)
        {
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
        }


        private void BackgroundUpdate()
        {
            
        }

        private void BackgroundDraw()
        {
            
        }
    }
}