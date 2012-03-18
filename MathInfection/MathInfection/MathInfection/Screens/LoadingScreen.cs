using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MathInfection
{
    class LoadingScreen : GameScreen
    {
        private ContentManager content;
        private bool loadingIsSlow;
        private bool otherScreenAreGone;
        private bool loadingNewGame;
        private Texture2D loadingScreenTex;
        private GameScreen[] screensToLoad;
        private Texture2D[] introTexture;
        private TimeSpan prevtime;
        private TimeSpan frameDur;
        private int curFrame;
        private bool exitGamePlayScreen;

        private LoadingScreen(bool loadingIsSlow, GameScreen[] screensToLoad,
                                                              bool isNewGame)
        {
            this.loadingIsSlow = loadingIsSlow;
            this.screensToLoad = screensToLoad;
            loadingNewGame = isNewGame;
            introTexture = new Texture2D[8];
            prevtime = TimeSpan.Zero;
            frameDur = TimeSpan.FromSeconds(.6);
            curFrame = 0;
            TransitionOnTime = TimeSpan.FromSeconds(.5);
            exitGamePlayScreen = screensToLoad.Length > 1;
        }

        public override void LoadContent()
        {
            if(content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            if (loadingNewGame)
            {
                for(int i = 0; i < 8; i++)
                {
                    introTexture[i] = content.Load<Texture2D>(@"IntroImages/Intro" + i);
                }
            }
            else
            {
                loadingScreenTex = content.Load<Texture2D>("LoadingScreen");
            }
        }

        public static void Load(ScreenManager screenManager, bool loadingIsSlow,
                                 PlayerIndex? controllingPlayer, bool isNewGame,
                                              params GameScreen[] screensToLoad)
        {
            foreach(GameScreen screen in screenManager.GetScreens())
            {
                screen.ExitScreen();
            }
            LoadingScreen loadingScreen = new LoadingScreen(loadingIsSlow, screensToLoad,
                                                                              isNewGame);
            screenManager.AddScreen(loadingScreen, controllingPlayer);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            if (!exitGamePlayScreen)
            {
                if(prevtime == TimeSpan.Zero)
                {
                    prevtime = gameTime.TotalGameTime;
                }

                if(gameTime.TotalGameTime - prevtime > frameDur && curFrame <= 6)
                {
                    curFrame++;
                    prevtime = gameTime.TotalGameTime;
                }
            }

            if(otherScreenAreGone)
            {
                ScreenManager.RemoveScreen(this);
                foreach(GameScreen screen in screensToLoad)
                {
                    if(screen != null)
                    {
                        ScreenManager.AddScreen(screen, ControllingPlayer);
                    }
                }
                ScreenManager.Game.ResetElapsedTime();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if(exitGamePlayScreen || !loadingNewGame)
            {
                if ((ScreenState == ScreenState.Active) &&
                    (ScreenManager.GetScreens().Length == 1))
                {
                    otherScreenAreGone = true;
                }
            }
            else
            {
                if(ScreenState == ScreenState.Active && curFrame == 7 &&
                                    ScreenManager.GetScreens().Length == 1)
                {
                    otherScreenAreGone = true;
                }
            }

            if(loadingIsSlow)
            {
                SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
                SpriteFont font = ScreenManager.Font;
                SpriteFont newGameFont = ScreenManager.GreedingFont;

                string message;
                string uname = "";
                GameData data = FileIO.DeserializeFromXML();
                if(data != null)
                {
                    uname = data.PlayerName;
                    if(uname.Length > 0)
                    {
                        char lowerInitial = uname[0];
                        char upperInitial = Char.ToUpper(lowerInitial);
                        uname = uname.Replace(lowerInitial, upperInitial);
                    }
                }

                if(loadingNewGame)
                {
                    message = "Hello, " + uname;
                }
                else
                {
                    message = "Welcome Back, " + uname + ".  Restoring Game File...";
                }

                if (!loadingNewGame)
                {
                    Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
                    Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
                    Vector2 textSize = font.MeasureString(message);
                    Vector2 textPosition = (viewportSize - textSize) / 2;
                    Vector2 loadingscreenimg = new Vector2(0, -3);
                    spriteBatch.Begin();
                    spriteBatch.Draw(loadingScreenTex, loadingscreenimg, Color.White);
                    spriteBatch.DrawString(font, message, textPosition, Color.DarkBlue);
                    spriteBatch.End();
                }
                else
                {
                    spriteBatch.Begin();
                    Rectangle introPosition = new Rectangle(0, 0, 1000, 660);
                    Vector2 textPosition = new Vector2(428, 480);
                    spriteBatch.Draw(introTexture[curFrame], introPosition, Color.White);
                    spriteBatch.DrawString(newGameFont, message, textPosition,
                                                                           Color.Orange);
                    spriteBatch.End();
                }
            }
        }
        // endof Draw()
    }
}
