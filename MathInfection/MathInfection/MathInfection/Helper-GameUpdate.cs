using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MathInfection
{
    public static class GameUpdate
    {
        public static bool CheckWindowMode(GraphicsDeviceManager gdm, Main caller)
        {
            // TODO: fullscreen mode doesn't get correct desktop resolution
            if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Y) || Keyboard.GetState().IsKeyDown(Keys.F))
            {
                if (caller.WindowMode)
                {
                    gdm.PreferredBackBufferWidth = 2560;
                    gdm.PreferredBackBufferHeight = 1440;
                    gdm.ToggleFullScreen();
                    gdm.ApplyChanges();
                    return false;
                }
                gdm.PreferredBackBufferWidth = 1124;
                gdm.PreferredBackBufferHeight = 700;
                gdm.ToggleFullScreen();
                gdm.ApplyChanges();
                return true;
            }
            return caller.WindowMode;
        }

        public static void CheckInput(GameTime gameTime, Player player1, List<Bullet> defaultBulletList,
                                      List<Texture2D> bulletTexList, TimeSpan previousFireTime,
                                      TimeSpan defaultBulletFireRate, Vector2 windowSize, Main caller)
        {
            // BoostButton pressing
            GamePadState newGamePadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState newKeyboardState = Keyboard.GetState();
            if (newGamePadState.IsButtonDown(Buttons.A) || newKeyboardState.IsKeyDown(Keys.LeftShift))
            {
                player1.StartBoost = true;
            }
            else
            {
                player1.StartBoost = false;
            }
            // endof BoostButton pressing

            // Bullet Generation
            if (gameTime.TotalGameTime - previousFireTime > defaultBulletFireRate)
            {
                if (newGamePadState.Triggers.Right > .2f || newKeyboardState.IsKeyDown(Keys.Space))
                {
                    Vector2 bSize = new Vector2(bulletTexList[0].Width, bulletTexList[0].Height);
                    Vector2 bPos = new Vector2(player1.PlayerPosition.X + player1.CharacterSize.X / 2,
                                               player1.PlayerPosition.Y);
                    defaultBulletList.Add(new Bullet(bulletTexList[0], bPos, bSize, windowSize,
                                                     Vector2.Zero, 10, 20));
                    caller.PreviousFireTime = gameTime.TotalGameTime;
                }
            }
            // endof Bullet Generation
        }

        public static void CheckCollision(List<Bullet> defaultBulletList, List<Enemy> enemyList, Player p1)
        {
            Rectangle r1 = new Rectangle();
            Rectangle r2 = new Rectangle();

            // Bullet Collision checks
            if(defaultBulletList.Count > 0)
            {
                foreach(Bullet b in defaultBulletList)
                {
                    r1.Width = (int)Math.Round(b.CharacterSize.X);
                    r1.Height = (int)Math.Round(b.CharacterSize.Y);
                    r1.X = (int)Math.Round(b.Position.X);
                    r1.Y = (int)Math.Round(b.Position.Y);
                    foreach(Enemy e in enemyList)
                    {
                        r2.Width = (int)Math.Round(e.CharacterSize.X);
                        r2.Height = (int)Math.Round(e.CharacterSize.Y);
                        r2.X = (int)Math.Round(e.Position.X);
                        r2.Y = (int)Math.Round(e.Position.Y);

                        if(r1.Intersects(r2))
                        {
                            e.GetHit(b.Damage);
                            b.IsValid = false;
                        }
                    }
                }
            }
            // endof Bullet Collision checks


            r1.Width = (int)Math.Round(p1.CharacterSize.X);
            r1.Height = (int)Math.Round(p1.CharacterSize.Y);
            r1.X = (int)Math.Round(p1.PlayerPosition.X);
            r1.Y = (int)Math.Round(p1.PlayerPosition.Y);
            foreach (Enemy e2 in enemyList)
            {
                r2.Width = (int)Math.Round(e2.CharacterSize.X);
                r2.Height = (int)Math.Round(e2.CharacterSize.Y);
                r2.X = (int)Math.Round(e2.Position.X);
                r2.Y = (int)Math.Round(e2.Position.Y);

                if (r1.Intersects(r2))
                {
                    p1.WasHit = true;
                }
            }
        }
        // endof CheckCollision()
    }
}
