using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MathInfection
{
    public static class GameUpdate
    {
        public static void UpdateEnemyList(List<Enemy> eList)
        {
            int index = 0;
            while(index < eList.Count)
            {
                if(!eList[index].IsAlive())
                {
                    eList.RemoveAt(index);
                }
                index++;
            }
        }

        public static void UpdateBulletList(List<Bullet> bList)
        {
            int index = 0;
            while(index < bList.Count)
            {
                if(!bList[index].IsValid)
                {
                    bList.RemoveAt(index);
                }
                index++;
            }
        }

        public static void CheckInput(GameTime gameTime, Player player1, List<Bullet> dBulletList,
                                         List<Texture2D> bulletTexList, TimeSpan previousFireTime,
                                         TimeSpan dFireRate, Vector2 wSize, GameplayScreen caller)
        {
            // BoostButton pressing
            GamePadState newGamePadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState newKeyboardState = Keyboard.GetState();
            if (newGamePadState.IsButtonDown(Buttons.LeftTrigger) ||
                newKeyboardState.IsKeyDown(Keys.RightShift))
            {
                player1.StartBoost = true;
            }
            else
            {
                player1.StartBoost = false;
            }
            // endof BoostButton pressing

            // Bullet Generation
            if (gameTime.TotalGameTime - previousFireTime > dFireRate)
            {
                if (newGamePadState.Triggers.Right > .2f || newKeyboardState.IsKeyDown(Keys.Space))
                {
                    Vector2 bSize = new Vector2(bulletTexList[0].Width, bulletTexList[0].Height);
                    Vector2 bPos = new Vector2(player1.PlayerPosition.X +
                                               player1.CharacterSize.X / 2,
                                               player1.PlayerPosition.Y);
                    dBulletList.Add(new Bullet(bulletTexList[0], bPos, bSize, wSize,
                                                     Vector2.Zero, 10, 20));
                    caller.PreviousFireTime = gameTime.TotalGameTime;
                }
            }
            // endof Bullet Generation
        }

        public static void CheckCollision(List<Bullet> defaultBulletList, List<Enemy> enemyList,
                                                                Player p1, out int currentScore)
        {
            Rectangle r1 = new Rectangle();
            Rectangle r2 = new Rectangle();

            currentScore = 0;
            if(defaultBulletList.Count > 0)
            {
                foreach(Bullet b in defaultBulletList)
                {
                    r1.Width = (int)Math.Round(b.CharacterSize.X);
                    r1.Height = (int)Math.Round(b.CharacterSize.Y);
                    r1.X = (int)Math.Round(b.Position.X);
                    r1.Y = (int)Math.Round(b.Position.Y);
                    foreach(var e in enemyList)
                    {
                        r2.Width = (int)Math.Round(e.CharacterSize.X * e.ResizeRation);
                        r2.Height = (int)Math.Round(e.CharacterSize.Y * e.ResizeRation);
                        r2.X = (int)Math.Round(e.Position.X);
                        r2.Y = (int)Math.Round(e.Position.Y);
                        if(r1.Intersects(r2))
                        {
                            e.GetHit(b.Damage);
                            if(!e.IsAlive())
                            {
                                bool isBoss = e.GetType().ToString() == "MathInfection.Boss";
                                currentScore = 50;
                                if(isBoss)
                                {
                                    currentScore += 50;
                                }
                            }
                            b.IsValid = false;
                        }
                    }
                }
            }
            // endof Bullet Collision Detection

            // Player Collision Detection
            r1.Width = (int)Math.Round(p1.CharacterSize.X);
            r1.Height = (int)Math.Round(p1.CharacterSize.Y);
            r1.X = (int)Math.Round(p1.PlayerPosition.X);
            r1.Y = (int)Math.Round(p1.PlayerPosition.Y);
            foreach (Enemy e in enemyList)
            {
                r2.Width = (int)Math.Round(e.CharacterSize.X * e.ResizeRation);
                r2.Height = (int)Math.Round(e.CharacterSize.Y * e.ResizeRation);
                r2.X = (int)Math.Round(e.Position.X);
                r2.Y = (int)Math.Round(e.Position.Y);

                if (r1.Intersects(r2))
                {
                    p1.WasHit = true;
                    p1.EnemyType = e.GetType().ToString();
                    e.Health = 0;
                    break;
                }
            }
        }

        public static void UpdateGameData(GameData data, int currentLevel,
                                                         int currentScore)
        {
            
        }

        public static string GetHighScores(bool fromMainMenu)
        {
            string highScores = "";
            return highScores;
        }

        public static string GetTotalScore()
        {
            string totalScore = "";
            return totalScore;
        }

        public static int GetCurrentLevel()
        {
            int currentLevel = 0;
            return currentLevel;
        }

        // endof public static class GameUpdate
    }
}
