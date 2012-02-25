using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MathInfection
{
    class LoadingScreen : GameScreen
    {
        private bool loadingIsSlow;
        private bool otherScreenAreGone;
        private bool loadingNewGame;
        private GameScreen[] screensToLoad;

        private LoadingScreen(ScreenManager screenManager, bool loadingIsSlow,
                              GameScreen[] screensToLoad, bool isNewGame)
        {
            this.loadingIsSlow = loadingIsSlow;
            this.screensToLoad = screensToLoad;
            this.loadingNewGame = isNewGame;
            TransitionOnTime = TimeSpan.FromSeconds(.5);
        }

        public static void Load(ScreenManager screenManager, bool loadingIsSlow,
                                 PlayerIndex? controllingPlayer, bool isNewGame,
                                              params GameScreen[] screensToLoad)
        {
            foreach(GameScreen screen in screenManager.GetScreens())
            {
                screen.ExitScreen();
            }
            LoadingScreen loadingScreen = new LoadingScreen(screenManager,
                                        loadingIsSlow, screensToLoad, isNewGame);
            screenManager.AddScreen(loadingScreen, controllingPlayer);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

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
            if((ScreenState == ScreenState.Active) &&
               (ScreenManager.GetScreens().Length == 1))
            {
                otherScreenAreGone = true;
            }

            if(loadingIsSlow)
            {
                SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
                SpriteFont font = ScreenManager.Font;

                string message;
                string uname = "";
                GameData data = FileIO.DeserializeFromXML();
                if (data != null)
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
                    message = "Hi, " + uname + ".  Starting a new game...";
                }
                else
                {
                    message = "Hi, " + uname + ".  Restoring your last game...";
                }

                Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
                Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
                Vector2 textSize = font.MeasureString(message);
                Vector2 textPosition = (viewportSize - textSize) / 2;

                Color color = Color.White * TransitionAlpha;

                spriteBatch.Begin();
                spriteBatch.DrawString(font, message, textPosition, color);
                spriteBatch.End();
            }
        }
    }
}
