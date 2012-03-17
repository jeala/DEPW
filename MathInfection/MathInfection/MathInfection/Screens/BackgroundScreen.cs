using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MathInfection
{
    class BackgroundScreen : GameScreen
    {
        private ContentManager content;
        private Texture2D[] backgroundTexture = new Texture2D[23];
        private Texture2D AdditionSign;
        private Texture2D MultiplicationSign;
        private Texture2D SubtractionSign;
        private Texture2D DivisionSign;
        private TimeSpan prevtime = TimeSpan.Zero;
        private TimeSpan framedur = TimeSpan.FromSeconds(.018f);
        private int curframe = 0;

        Stack<Box> myBoxStack = new Stack<Box>();
        Stack<Box> myBoxStack2 = new Stack<Box>();
        Stack<Box> myBoxStack3 = new Stack<Box>();
        Stack<Box> myBoxStack4 = new Stack<Box>();


        public BackgroundScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(.5);
            TransitionOffTime = TimeSpan.FromSeconds(.5);

            for (int i = 0; i < 4; i++)
            {
                myBoxStack.Push(new Box(RandomGenerator.RandomInt(784),
                                  RandomGenerator.RandomInt(584), 6, 6,
                                      RandomGenerator.RandomInt(4) + 1,
                                    RandomGenerator.RandomInt(4) + 1));
            }

            for (int i = 0; i <= 2; i++)
            {
                myBoxStack2.Push(new Box(RandomGenerator.RandomInt(2),
                       5, 6, 6, RandomGenerator.RandomInt(4) + 1, 5));
            }

            for (int i = 0; i < 3; i++)
            {
                myBoxStack3.Push(new Box(RandomGenerator.RandomInt(222),
                                   RandomGenerator.RandomInt(555), 6, 6,
                                       RandomGenerator.RandomInt(4) + 1,
                                     RandomGenerator.RandomInt(4) + 1));
            }

            for (int i = 0; i < 3; i++)
            {
                myBoxStack4.Push(new Box(RandomGenerator.RandomInt(700),
                                   RandomGenerator.RandomInt(500), 6, 6,
                                       RandomGenerator.RandomInt(4) + 1,
                                     RandomGenerator.RandomInt(4) + 1));
            }
        }

        public override void LoadContent()
        {
            if (content == null)
            {
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            }

            for (int i = 0; i < 23; i++)
            {
                backgroundTexture[i] = content.Load<Texture2D>(@"TitleScreenImages/Title-" + i);
            }

            AdditionSign       = content.Load<Texture2D>(@"TitleScreenImages/PlusSign");
            MultiplicationSign = content.Load<Texture2D>(@"TitleScreenImages/MultiplySign");
            SubtractionSign    = content.Load<Texture2D>(@"TitleScreenImages/MinusSign");
            DivisionSign       = content.Load<Texture2D>(@"TitleScreenImages/DivisionSign");
        }

        public override void UnloadContent()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            if (gameTime.TotalGameTime - prevtime > framedur)
            {
                if (curframe != 22)
                {
                    curframe++;
                    prevtime = gameTime.TotalGameTime;
                }
            }

            foreach (Box b in myBoxStack)
            {
                b.update();
            }

            foreach (Box b2 in myBoxStack2)
            {
                b2.update();
            }

            foreach (Box b3 in myBoxStack3)
            {
                b3.update();
            }

            foreach (Box b4 in myBoxStack4)
            {
                b4.update();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Vector2 boom = new Vector2();
            Vector2 boom2 = new Vector2(150, 400);
            Vector2 boom3 = new Vector2(200, 200);
            Vector2 boom4 = new Vector2(350, 550);
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            spriteBatch.Begin();
            
            spriteBatch.Draw(backgroundTexture[curframe], new Rectangle(0, 0, 1000, 660),
                                                                            Color.White);

            foreach (Box b in myBoxStack)
            {
                boom.X = b.X;
                boom.Y = b.Y;

                spriteBatch.Draw(AdditionSign, boom, Color.White);
            }

            foreach (Box b2 in myBoxStack2)
            {
                boom2.X = b2.X;
                boom2.Y = b2.Y;

                spriteBatch.Draw(SubtractionSign, boom2, Color.White);
            }

            foreach (Box b3 in myBoxStack3)
            {
                boom3.X = b3.X;
                boom3.Y = b3.Y;

                spriteBatch.Draw(MultiplicationSign, boom3, Color.White);
            }

            foreach (Box b4 in myBoxStack4)
            {
                boom4.X = b4.X;
                boom4.Y = b4.Y;

                spriteBatch.Draw(DivisionSign, boom4, Color.White);
            }

            spriteBatch.End();
        }
    }
}
