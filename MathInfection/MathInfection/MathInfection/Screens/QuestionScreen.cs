using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MathInfection
{
    class QuestionScreen : GameScreen
    {
        private GameplayScreen parent;
        private string message;
        private int[] answers;
        private int correctAnswer;
        private int lifeSpan;
        private Texture2D questionFrameTex;

        public event EventHandler<PlayerIndexEventArgs> Accepted;
        public event EventHandler<PlayerIndexEventArgs> Cancelled;

        public QuestionScreen(string msg, GameplayScreen caller)
                                       : this(msg, true, caller)
        { }

        public QuestionScreen(string msg, bool includeUsageText,
                                              GameplayScreen caller)
        {
            parent = caller;
            string usageText = "\n\n\nUse keyboard's Up/Down/Left/" +
                                       "Right to choose your answer";
            string question = RandomGenerator.RandomQuestion( parent.CurrentScore,
                                                  out correctAnswer, out answers);
            lifeSpan = 60;
            if (includeUsageText)
                message = msg + question + usageText;
            else
                message = msg;

            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.1);
            TransitionOffTime = TimeSpan.FromSeconds(0.1);
        }

        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;
            // TODO: create a question frame image?
            questionFrameTex = content.Load<Texture2D>("gradient");
        }

        public override void HandleInput(InputState input)
        {
            parent.AnswerTimeLeft = lifeSpan;
            parent.AnswerCorrect = false;
            PlayerIndex playerIndex;
            if (input.IsMenuUp(ControllingPlayer))
            {
                if(correctAnswer == 1)
                {
                    parent.AnswerCorrect = true;

                }
                ExitScreen();
            }
            else if (input.IsMenuDown(ControllingPlayer))
            {
                if(correctAnswer == 2)
                {
                    parent.AnswerCorrect = true;
                }
                ExitScreen();
            }
            else if(input.IsMenuLeft(ControllingPlayer))
            {
                if(correctAnswer == 3)
                {
                    parent.AnswerCorrect = true;
                }
                ExitScreen();
            }
            else if(input.IsMenuRight(ControllingPlayer))
            {
                if(correctAnswer == 4)
                {
                    parent.AnswerCorrect = true;
                }
                ExitScreen();
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                      bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            lifeSpan--;
            if(lifeSpan < 0)
            {
                parent.AnswerCorrect = false;
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
            spriteBatch.Draw(questionFrameTex, backgroundRectangle, color);
            spriteBatch.DrawString(font, message, textPosition, color);
            spriteBatch.End();
        }
    }
}