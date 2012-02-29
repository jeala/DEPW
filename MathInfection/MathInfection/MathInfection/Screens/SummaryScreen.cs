using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MathInfection
{
    class SummaryScreen : GameScreen
    {
        string message;
        Texture2D gradientTexture;

        public event EventHandler<PlayerIndexEventArgs> Accepted;
        public event EventHandler<PlayerIndexEventArgs> Cancelled;

        public SummaryScreen(string message, bool win) : this(message, true, win)
        { }

        public SummaryScreen(string message, bool includeUsageText, bool win)
        {
            string result = win ? "Next Level" : "Try Again";
            if(win)
            {
                message += GameUpdate.GetTotalScore();
                string hs = GameUpdate.GetHighScores(false);
                message += hs.Length > 0 ? hs : "\n";
            }
            string usageText = "\n\n\nA-button, Space, Enter = " + result +
                               "\nB-button, Esc = Back to Main Screen";

            if (includeUsageText)
                this.message = message + usageText;
            else
                this.message = message;

            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
        }

        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;

            gradientTexture = content.Load<Texture2D>("gradient");
        }


        public override void HandleInput(InputState input)
        {
            PlayerIndex playerIndex;
            if (input.IsMenuSelect(ControllingPlayer, out playerIndex))
            {
                if (Accepted != null)
                    Accepted(this, new PlayerIndexEventArgs(playerIndex));
                ExitScreen();
                ScreenManager.AddScreen(new GameplayScreen(ScreenManager, false), null);
            }
            else if (input.IsMenuCancel(ControllingPlayer, out playerIndex))
            {
                if (Cancelled != null)
                    Cancelled(this, new PlayerIndexEventArgs(playerIndex));
                ScreenManager.AddScreen(new BackgroundScreen(), null);
                ScreenManager.AddScreen(new MainMenuScreen(), null);
                ExitScreen();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.TopScoresFont;

            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 textSize = font.MeasureString(message);
            Vector2 textPosition = (viewportSize - textSize) / 2;

            const int hPad = 32;
            const int vPad = 16;

            Rectangle backgroundRectangle = new Rectangle((int)textPosition.X - hPad,
                                                          (int)textPosition.Y - vPad,
                                                          (int)textSize.X + hPad * 2,
                                                          (int)textSize.Y + vPad * 2);

            Color color = Color.White * TransitionAlpha;

            spriteBatch.Begin();
            spriteBatch.Draw(gradientTexture, backgroundRectangle, color);
            spriteBatch.DrawString(font, message, textPosition, color);
            spriteBatch.End();
        }
    }
}
