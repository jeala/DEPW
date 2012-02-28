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
        private string answerMessage;
        private int[] answers;
        private int correctAnswer;
        private int lifeSpan;
        private Texture2D questionFrameTex;
        private bool answered;
        private int answer;

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
                                        "Right to enter your answer";

            string question = RandomGenerator.RandomQuestion( parent.CurrentScore,
                                                  out correctAnswer, out answers);
            lifeSpan = 120;
            if (includeUsageText)
                message = msg + "\n" + question + usageText;
            else
                message = msg;

            answerMessage = "\n        " + answers[0] + "\n" + "    " +
                              answers[1] + "    " + answers[2] + "\n" +
                              "        " + answers[3];

            answered = false;
            answer = -1;

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
            if (input.IsMenuUp(ControllingPlayer))
            {
                answer = 1;
                answered = true;
            }
            else if (input.IsMenuDown(ControllingPlayer))
            {
                answer = 2;
                answered = true;
            }
            else if(input.IsMenuLeft(ControllingPlayer))
            {
                answer = 3;
                answered = true;
            }
            else if(input.IsMenuRight(ControllingPlayer))
            {
                answer = 4;
                answered = true;
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                      bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            lifeSpan--;
            if(answered)
            {
                if(answer == correctAnswer)
                {
                    parent.AnswerCorrect = true;
                    parent.AnswerTimeLeft = lifeSpan;
                }
                else
                {
                    parent.AnswerCorrect = false;
                }
                ExitScreen();
            }
            else if(lifeSpan < 0)
            {
                parent.AnswerCorrect = false;
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

            Vector2 answerSize = font.MeasureString(answerMessage);
            Vector2 answerPosition = (viewportSize - answerSize) / 2;

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
            spriteBatch.DrawString(font, answerMessage, answerPosition, color);
            spriteBatch.End();
        }
    }
}