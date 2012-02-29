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
        private GameData gameData;
        private int currentScore;
        private bool answerCorrect;
        private int answerTimeLeft;

        public TimeSpan PreviousFireTime
        {
            set
            {
                previousFireTime = value;
            }
        }

        public int CurrentScore
        {
            get
            {
                return currentScore;
            }
            set
            {
                currentScore = value;
            }
        }

        public bool AnswerCorrect
        {
            set
            {
                answerCorrect = value;
            }
            get
            {
                return answerCorrect;
            }
        }

        public int AnswerTimeLeft
        {
            get
            {
                return answerTimeLeft;
            }
            set
            {
                answerTimeLeft = value;
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

            gameData = FileIO.DeserializeFromXML();
            if(gameData == null)
            {
                string uname = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                if(uname.Contains(@"\"))
                {
                    string[] nameArry = uname.Split('\\');
                    uname = nameArry[nameArry.Length - 1];
                }
                gameData = new GameData(uname);
                FileIO.SerializeToXML(gameData);
            }
            else
            {
                if(isNewGame)
                {
                    gameData.TotalScore = 0;
                    gameData.LastTotal = 0;
                    gameData.CurrentLevel = 1;
                    gameData.LastGameWon = false;
                    gameData.MiddleUpdate = false;
                    FileIO.SerializeToXML(gameData);
                }
            }
            currentScore = 0;
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
            bulletTexList.Add(content.Load<Texture2D>(@"BulletImages/Bullet3"));
        }

        private void GameplayUpdate(GameTime gameTime)
        {
            GameUpdate.CheckInput(gameTime, player1, defaultBulletList,
                                  bulletTexList, previousFireTime,
                                  defaultBulletFireRate, windowSize, this);

            player1.update(Vector2.Zero);
            bool playerAlive = player1.IsAlive();
            if(!playerAlive)
            {
                ScreenManager.AddScreen(new SummaryScreen("You loose", playerAlive),
                                                                 ControllingPlayer);
                ExitScreen();
            }

            foreach(Enemy e in enemyList)
            {
                e.update(player1.PlayerPosition);
            }

            foreach(Bullet b in defaultBulletList)
            {
                b.update(player1.PlayerPosition);
            }

            GameUpdate.CheckCollision(defaultBulletList, enemyList, player1,
                                                          out currentScore);
            GameUpdate.UpdateEnemyList(enemyList);
            GameUpdate.UpdateBulletList(defaultBulletList);
            if(enemyList.Count == 0)
            {
                ScreenManager.AddScreen(new SummaryScreen("You win", playerAlive),
                                                               ControllingPlayer);
                ExitScreen();
            }
            if(player1.WasHit)
            {
                ScreenManager.AddScreen(new QuestionScreen("Question Time", this),
                                                               ControllingPlayer);
                player1.WasHit = false;
            }
            hud.update(player1, enemyList.Count);

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

        public void ProcessAnswer(bool correct, int timeLeft)
        {
            bool isBoss = player1.EnemyType == "MathInfection.Boss";
            if(correct)
            {
                int bonus = timeLeft * 2;
                if (isBoss)
                {
                    currentScore = (100 + bonus);
                }
                else
                {
                    currentScore = (20 + bonus);
                }
            }
            else
            {
                if (isBoss)
                {
                    player1.Health -= 25;
                }
                else
                {
                    player1.Health -= 10;
                }
            }
            player1.Score += currentScore;
            currentScore = 0;
        }
        // endof class GameplayScreen
    }
}