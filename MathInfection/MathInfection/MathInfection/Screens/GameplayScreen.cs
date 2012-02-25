using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ChainRxN
{
    public class GameplayScreen : GameScreen
    {
        ContentManager content;
        SpriteFont gameFont;
        float pauseAlpha;

        GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont hudFont;

        private int numStrategies;
        private int numBombs;
        private Texture2D bombTexture;
        private Texture2D fuseTexture;
        private List<Bomb> bombList;
        private Fuse fuse;
        private Vector2 bombSize;
        private Vector2 fuseSize;
        private Vector2 windowSize;
        private int lifeSpan;
        private float scaleRatio;
        private int bombsToDetonate;
        private HUD hud;
        private bool gameOver;
        private bool gameStarted;

        private GameData gameData;
        private int totalScore;
        private int currentLevel;
        private int currentScore;
        private int totalLevel;

        public bool GameOver
        {
            set
            {
                gameOver = value;
            }
        }

        public int CurrentScore
        {
            set
            {
                currentScore = value;
            }
        }

        public int TotalLevel
        {
            get
            {
                return totalLevel;
            }
        }

        public GameplayScreen(ScreenManager sMgr, bool isNewGame)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            ScreenManager = sMgr;
            GameplayInit(isNewGame);
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

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

            if (IsActive)
            {
                GameplayUpdate(gameTime);
            }
        }

        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            int playerIndex = 0;
            if (ControllingPlayer.HasValue)
            {
                playerIndex = (int)ControllingPlayer.Value;
            }

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.Black, 0, 0);
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();
            GameplayDraw(gameTime);
            spriteBatch.End();

            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        private void GameplayInit(bool isNewGame)
        {
            GameWindow window = ScreenManager.Game.Window;
            numStrategies = 3;
            bombList = new List<Bomb>();
            windowSize = new Vector2(window.ClientBounds.Width, window.ClientBounds.Height);
            lifeSpan = 200;
            scaleRatio = .04f;
            totalLevel = 7;
            gameOver = false;
            gameStarted = false;
            gameData = FileIO.DeserializeFromXML();
            if(gameData == null)
            {
                string uname = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                if(uname.Contains(@"\"))
                {
                    string[] nameAry = uname.Split('\\');
                    uname = nameAry[nameAry.Length - 1];
                }
                gameData = new GameData(uname, totalLevel);
                FileIO.SerializeToXML(gameData);
                totalScore = 0;
                currentLevel = 1;
            }
            else
            {
                if(isNewGame)
                {
                    gameData.TotalScore   = 0;
                    gameData.LastTotal    = 0;
                    gameData.CurrentLevel = 1;
                    gameData.LastGameWon  = false;
                    gameData.MiddleUpdate = false;
                    FileIO.SerializeToXML(gameData);
                    totalScore   = 0;
                    currentLevel = 1;
                }
                else
                {
                    totalScore   = gameData.TotalScore;
                    currentLevel = gameData.CurrentLevel;
                }
            }
            RandomGenerator.RandomBombNumber(isNewGame, out numBombs, out bombsToDetonate);
            hud = new HUD(ScreenManager ,windowSize, bombsToDetonate, numBombs, totalLevel);
        }

        private void GameplayLoad()
        {
            spriteBatch = ScreenManager.SpriteBatch;
            hudFont = content.Load<SpriteFont>("HUDFont");
            bombTexture = content.Load<Texture2D>("Bomb");
            fuseTexture = content.Load<Texture2D>("Explosion");
            bombSize = new Vector2(bombTexture.Width, bombTexture.Height);
            fuseSize = new Vector2(fuseTexture.Width, fuseTexture.Height);

            int bombNumber = numBombs;
            while(bombNumber > 0)
            {
                bombList.Add(new Bomb(bombTexture, fuseTexture,
                                      RandomGenerator.RandomPosition(windowSize, bombSize),
                                      bombSize, windowSize, lifeSpan, scaleRatio,
                                      RandomGenerator.RandomMoverStrategy(numStrategies)));
                bombNumber--;
            }
            fuse = new Fuse(fuseTexture, new Vector2(8, 8), fuseSize, windowSize, lifeSpan,
                         RandomGenerator.RandomPosition(windowSize, fuseSize), scaleRatio);
        }

        private void GameplayUpdate(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed ||
                                           Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                fuse.Triggered = true;
                gameStarted = true;
            }

            foreach(Bomb b in bombList)
            {
                if(b.Show)
                {
                    b.update();
                }
            }
            if(fuse.Show)
            {
                fuse.update(gameTime);
            }
            GameUpdate.UpdateCollisionStatus(fuse, bombList);
            int timeLeft = GameUpdate.HubGetTimeLeft(this, fuse, bombList, gameStarted);
            int bombTriggered = GameUpdate.HubGetBombTriggered(bombList);
            hud.update(this, timeLeft, bombTriggered, totalScore, currentLevel);
            if(gameOver)
            {
                bool win = bombTriggered >= bombsToDetonate;
                gameData.LastGameWon = win;
                GameUpdate.UpdateGameData(gameData, currentLevel, currentScore);
                FileIO.SerializeToXML(gameData);
                string greed = win ? "You Win." : "You Loose!";
                ScreenManager.AddScreen(new SummaryScreen(greed, win), null);
                IsExiting = true;
            }
        }

        private void GameplayDraw(GameTime gameTime)
        {
            foreach(Bomb b in bombList)
            {
                if(b.Show)
                {
                    b.draw(spriteBatch);
                }
            }
            if (fuse.Show)
            {
                fuse.draw(spriteBatch, gameTime);
            }
            hud.draw(hudFont, spriteBatch);
        }
    }
}
