﻿using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace MathInfection
{
    public class GameplayScreen : GameScreen
    {
        private ContentManager content;
        private float pauseAlpha;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont hudFont;

        private HeadsUpDisplay hud;

        private Player player1;
        private int player1CurrentScore;
        private int player1CurrentHealth;
        private Player player2;
        private List<Enemy> enemyList;
        private List<Boss> bossList;
        private List<Bullet> defaultBulletList;
        private Texture2D player1Texture;
        private Texture2D jettexture;
        private Texture2D jettexture2;
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

        private Texture2D Score;
        private Texture2D Health;
        private Texture2D healthIconF;
        private Texture2D shieldIconF;
        private Texture2D shieldIconP;
        private Texture2D gunIconF;
        Shield shield;
        private List<Health> healthList;
        private List<Shield> shieldList;

        private Song gameplaySong;
        private SoundEffect gunSound;
        private SoundEffect getHealth;
        private SoundEffect getShield;
        private SoundEffect noHealth;
        private SoundEffect questionNotice;

        public TimeSpan PreviousFireTime
        {
            set
            {
                previousFireTime = value;
            }
        }

        public GameplayScreen(ScreenManager sMgr, bool isNewGame)
        {
            TransitionOnTime  = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(.5);
            MediaPlayer.Stop();
            ScreenManager = sMgr;
            GameplayInit(isNewGame);
        }

        public override void LoadContent()
        {
            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }
            Score  = content.Load<Texture2D>(@"HUDimages/Score");
            Health = content.Load<Texture2D>(@"HUDimages/Health");
            gameplaySong = content.Load<Song>("Sounds\\Turns");
            MediaPlayer.Play(gameplaySong);
            GameplayLoad();
            Thread.Sleep(1000);
            ScreenManager.Game.ResetElapsedTime();
        }

        public override void UnloadContent()
        {
            GameUpdate.UpdateGameData(gameData, player1);
            FileIO.SerializeToXML(gameData);
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

            healthList = new List<Health>();
            shieldList = new List<Shield>();
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
            numEnemies = 5;
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
                player1CurrentHealth = 100;
                player1CurrentScore = 0;
                FileIO.SerializeToXML(gameData);
            }
            else
            {
                if(isNewGame)
                {
                    gameData.CurrentScore = 0;
                    player1CurrentScore = 0;
                    gameData.CurrentHealth = 100;
                    player1CurrentHealth = 100;
                    gameData.LastGameDied = false;
                    FileIO.SerializeToXML(gameData);
                }
                else
                {
                    player1CurrentHealth = gameData.CurrentHealth;
                    if(gameData.LastGameDied)
                    {
                        player1CurrentHealth = 1;
                    }
                    player1CurrentScore = gameData.CurrentScore;
                }
            }
            hud = new HeadsUpDisplay(new Vector2(windowSize.X / 2 - 200, 20));
        }

        private void GameplayLoad()
        {
            spriteBatch = ScreenManager.SpriteBatch;

            gunIconF    = content.Load<Texture2D>(@"PowerUps/GunFieldIcon");
            healthIconF = content.Load<Texture2D>(@"PowerUps/HealthUpgrade");
            shieldIconP = content.Load<Texture2D>(@"PowerUps/Shield");
            shieldIconF = content.Load<Texture2D>(@"PowerUps/ShieldFieldIcon");

            gunSound       = content.Load<SoundEffect>(@"Sounds/shootGun");
            getHealth      = content.Load<SoundEffect>(@"Sounds/grabHealth");
            getShield      = content.Load<SoundEffect>(@"Sounds/grabShield");
            noHealth       = content.Load<SoundEffect>(@"Sounds/noHealth");
            questionNotice = content.Load<SoundEffect>(@"Sounds/notice");

            hudFont    = content.Load<SpriteFont>("HUDFont");
            background = new Background();
            background.Load(ScreenManager.GraphicsDevice,
                                     content.Load<Texture2D>(@"BackgroundImages/BloodVein"),
                                     content.Load<Texture2D>(@"BackgroundImages/BloodCells"));

            player1Texture = content.Load<Texture2D>(@"CharacterImages/Player1");
            jettexture     = content.Load<Texture2D>(@"CharacterImages/Jet1Normal");
            jettexture2    = content.Load<Texture2D>(@"CharacterImages/Jet1Boost");
            playerSize = new Vector2(player1Texture.Width, player1Texture.Height);
            if(!singleMode)
            {
                // TODO: use player1's texture for now, might make another for player2 later.
                // player2Texture = Content.Load<Texture2D>(@"CharacterImages/Player2");
            }
            player1 = new Player(player1Texture, jettexture, jettexture2, initialPlayerPosition,
                                    playerVelocity, playerSize, windowSize, player1CurrentScore,
                                                                          player1CurrentHealth);

            enemyTexList.Add(content.Load<Texture2D>(@"CharacterImages/VirusGreen"));
            enemyTexList.Add(content.Load<Texture2D>(@"CharacterImages/VirusPurple"));
            enemyTexList.Add(content.Load<Texture2D>(@"CharacterImages/ShockingBloodCell"));

            GameUpdate.AddEnemy(enemyList, numEnemies, numMoveStrategies, enemyTexList,
                                                                  windowSize, player1);

            bulletTexList.Add(content.Load<Texture2D>(@"BulletImages/Bullets"));
            shield = new Shield(new Vector2(0, 0));
        }

        private void GameplayUpdate(GameTime gameTime)
        {
            GameUpdate.CheckInput(gameTime, player1, defaultBulletList, bulletTexList,
                 previousFireTime, defaultBulletFireRate, windowSize, this, gunSound);

            player1.update(Vector2.Zero, gameTime, 0);
            bool playerAlive = player1.IsAlive();
            bool noHealthInstance = false;
            if(!playerAlive)
            {
                noHealthInstance = noHealth.Play();
                ScreenManager.AddScreen(new SummaryScreen("You loose", playerAlive),
                                                                 ControllingPlayer);
                GameUpdate.UpdateGameData(gameData, player1);
                FileIO.SerializeToXML(gameData);
                ExitScreen();
                MediaPlayer.Stop();
                MediaPlayer.Play(ScreenManager.menuSong);
            }

            foreach(Enemy e in enemyList)
            {
                e.update(player1.PlayerPosition, gameTime, player1.Score);
            }
            foreach (Health h in healthList)
            {
                h.update();
            }
            foreach (Shield s in shieldList)
            {
                s.update();
            }

            foreach(Bullet b in defaultBulletList)
            {
                b.update(player1.PlayerPosition, gameTime, 0);
            }
            GameUpdate.UpdateHealthList(ref healthList, player1);
            GameUpdate.UpdateShieldList(ref shieldList, player1);
            GameUpdate.CheckCollision(defaultBulletList, enemyList, player1,
                          ref shield.shield_active, healthList, spriteBatch,
                             healthIconF, shieldList, getHealth, getShield);
            GameUpdate.UpdateEnemyList(enemyList);
            GameUpdate.UpdateBulletList(defaultBulletList);
            if(enemyList.Count == 0)
            {
                ScreenManager.AddScreen(new SummaryScreen("You win", playerAlive),
                                                               ControllingPlayer);
                ExitScreen();
                MediaPlayer.Stop();
                MediaPlayer.Play(ScreenManager.menuSong);
            }
            bool questionNoticeInstance = false;
            if(player1.WasHit)
            {
                ScreenManager.AddScreen(new QuestionScreen("Question Time", hud,
                                                   player1), ControllingPlayer);
                questionNoticeInstance = questionNotice.Play();
                player1.WasHit = false;
            }
            if (enemyList.Count < numEnemies)
            {
                int numToAdd = RandomGenerator.RandomNumberToAdd(player1.Score);
                GameUpdate.AddEnemy(enemyList, numToAdd, numMoveStrategies,
                                        enemyTexList, windowSize, player1);
            }
            hud.update(player1);

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            background.Update(elapsed * 100);
        }

        private void GameplayDraw(GameTime gameTime)
        {
            background.Draw(spriteBatch);
            hud.draw(hudFont, spriteBatch, Score, Health);
            foreach(Bullet b in defaultBulletList)
            {
                b.draw(spriteBatch);
            }
            foreach (Shield s in shieldList)
            {
                if (s.drawShieldF)
                {
                    s.draw(shieldIconF, spriteBatch);
                }
            }
            foreach (Health h in healthList)
            {
                if (h.drawIcon)
                {
                    h.draw(healthIconF, spriteBatch);
                }
            }
            foreach(Enemy e in enemyList)
            {
                e.draw(spriteBatch);
            }

            player1.draw(spriteBatch);

            if (shield.shield_active)
            {
                GameUpdate.ModifyShield(ref shield, spriteBatch, player1, shieldIconP);
            }
        }
        // endof class GameplayScreen
    }
}