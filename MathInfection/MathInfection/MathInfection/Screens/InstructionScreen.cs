using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MathInfection
{
    class InstructionScreen : MenuScreen
    {
        private readonly string usage;
        private Texture2D backgroundTexture;

        public event EventHandler<PlayerIndexEventArgs> Accepted;

        public InstructionScreen() : base("Controls")
        {
            usage = "\n\n\nA-button, B-button, Space, Enter = Back to Main Screen";
        }

        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;
            backgroundTexture = content.Load<Texture2D>("InstructionScreen");
        }

        public override void HandleInput(InputState input)
        {
            PlayerIndex playerIndex;
            if (input.IsMenuSelect(ControllingPlayer, out playerIndex) ||
                input.IsMenuCancel(ControllingPlayer, out playerIndex))
            {
                if (Accepted != null)
                    Accepted(this, new PlayerIndexEventArgs(playerIndex));
                ScreenManager.AddScreen(new BackgroundScreen(), null);
                ScreenManager.AddScreen(new MainMenuScreen(), null);
                ExitScreen();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont scoreFont = ScreenManager.Font;
            SpriteFont usageFont = ScreenManager.TopScoresFont;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;

            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);
            Vector2 titlePosition = new Vector2(viewport.Width/2, 160);
            Vector2 titleOrigin = scoreFont.MeasureString("Instructions") / 2;
            Color titleColor = new Color(192, 192, 192) * TransitionAlpha;
            float titleScale = 1.25f;

            titlePosition.Y -= transitionOffset * 100;

            Vector2 usagePosition = new Vector2(300, viewport.Height-180);

            spriteBatch.Begin();
            spriteBatch.Draw(backgroundTexture, Vector2.Zero, Color.White);
            spriteBatch.DrawString(scoreFont, "Instructions", titlePosition, titleColor,
                                     0, titleOrigin, titleScale, SpriteEffects.None, 0);
            spriteBatch.DrawString(usageFont, usage, usagePosition, titleColor);
            spriteBatch.End();
        }
    }
}
