using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MathInfection
{
    class QuestionScreen : GameScreen
    {
        private GameplayScreen parent;
        private Player player;
        private HeadsUpDisplay hud;
        private int correctAnswer;
        private int[] answers;
        private int lifeSpan;
        private string questionMessage;
        private string answerMessage;
        private string timeMessage;
        private const string usageText = "\n\n\nUse keyboard's Up/Down/Left/Right" +
                                                           " to enter your answer";
        private string message;
        private Texture2D questionFrameTex;

        public QuestionScreen(string msg, GameplayScreen caller, HeadsUpDisplay HUD,
                                                                          Player p1)
        {
            parent = caller;
            player = p1;
            hud = HUD;
            string question = RandomGenerator.RandomQuestion(parent.CurrentScore,
                                                 out correctAnswer, out answers);
            lifeSpan = 240;
            questionMessage = msg + "\n" + question;
            answerMessage = "\n" + answers[0].ToString().PadLeft(8) + "\n" +
                      answers[1] + answers[2].ToString().PadLeft(12) + "\n" +
                                   answers[3].ToString().PadLeft(8);
            timeMessage = "\nTime Left: " + lifeSpan;
            message = questionMessage + answerMessage + timeMessage + usageText;
            IsPopup = true;
            TransitionOnTime = TimeSpan.FromSeconds(0.1);
            TransitionOffTime = TimeSpan.Zero;
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
                ProcessAnswer(1);
                ExitScreen();
            }
            else if (input.IsMenuDown(ControllingPlayer))
            {
                ProcessAnswer(4);
                ExitScreen();
            }
            else if(input.IsMenuLeft(ControllingPlayer))
            {
                ProcessAnswer(2);
                ExitScreen();
            }
            else if(input.IsMenuRight(ControllingPlayer))
            {
                ProcessAnswer(3);
                ExitScreen();
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                      bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            lifeSpan--;
            if(lifeSpan <= 0)
            {
                ProcessAnswer(-1);
                ExitScreen();
            }
            timeMessage = "\nTime Left: " + lifeSpan;
            message = questionMessage + answerMessage + timeMessage + usageText;
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.TopScoresFont;

            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 messageSize = font.MeasureString(message);
            Vector2 messagePosition = (viewportSize - messageSize) / 2;

            const int hPad = 32;
            const int vPad = 16;
            Rectangle backgroundRectangle = new Rectangle((int)messagePosition.X - hPad,
                                                          (int)messagePosition.Y - vPad,
                                                          (int)messageSize.X + hPad * 2,
                                                          (int)messageSize.Y + vPad * 2);

            Color color = Color.White * TransitionAlpha;

            spriteBatch.Begin();
            spriteBatch.Draw(questionFrameTex, backgroundRectangle, color);
            spriteBatch.DrawString(font, message, messagePosition, color);
            spriteBatch.End();
        }

        private void ProcessAnswer(int playerChoice)
        {
            bool isBoss = player.EnemyType == "MathInfection.Boss";
            if(playerChoice == correctAnswer)
            {
                int bonus = lifeSpan * 2;
                player.Score += (20 + bonus);
                if(isBoss)
                {
                    player.Score += 80;
                }
            }
            else
            {
                player.Health -= 25;
                if(isBoss)
                {
                    player.Health -= 25;
                }
            }
            hud.QuestionAnswered(player);
        }
    }
}