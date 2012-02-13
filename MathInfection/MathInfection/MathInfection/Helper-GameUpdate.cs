using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MathInfection
{
    public static class GameUpdate
    {
        public static void CheckInput(GamePadState oldGamePadState, KeyboardState oldKeyboardState, GameTime gameTime,
                                      Player player1, List<Bullet> defaultBulletList, List<Texture2D> bulletTexList,
                                      TimeSpan previousFireTime, TimeSpan defaultBulletFireRate, Vector2 windowSize,
                                      Main caller)
        {
            // BoostButton pressing
            GamePadState newGamePadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState newKeyboardState = Keyboard.GetState();
            // TODO: assume user wouldn't switch between keyboard and gamepad while speeding up for now
            if (newGamePadState.IsButtonDown(Buttons.A) || newKeyboardState.IsKeyDown(Keys.LeftShift))
            {
                player1.StartBoost = true;
            }
            else if(oldGamePadState.IsButtonUp(Buttons.A) || oldKeyboardState.IsKeyUp(Keys.LeftShift))
            {
                player1.StartBoost = false;
            }
            oldGamePadState = newGamePadState;
            oldKeyboardState = newKeyboardState;
            // endof BoostButton pressing

            // Bullet Generation
            if (gameTime.TotalGameTime - previousFireTime > defaultBulletFireRate)
            {
                if (newGamePadState.Triggers.Right > .2f || newKeyboardState.IsKeyDown(Keys.Space))
                {
                    if (!(oldGamePadState.Triggers.Right > .25f) || !oldKeyboardState.IsKeyDown(Keys.Space))
                    {
                        Vector2 bSize = new Vector2(bulletTexList[0].Width, bulletTexList[0].Height);
                        Vector2 bPos = new Vector2(player1.PlayerPosition.X + player1.CharacterSize.X / 2,
                                                   player1.PlayerPosition.Y);
                        defaultBulletList.Add(new Bullet(bulletTexList[0], bPos, bSize, windowSize, Vector2.Zero, 10, 20));
                        caller.PreviousFireTime = gameTime.TotalGameTime;
                    }
                }
            }
            // endof Bullet Generation
        }

        public static void BulletCollision(List<Bullet> defaultBulletList, List<Enemy> enemyList)
        {
            if(defaultBulletList.Count <1)
            {
                return;
            }
            Rectangle r1 = new Rectangle();
            Rectangle r2 = new Rectangle();
            foreach(Bullet b in defaultBulletList)
            {
                r1.Width  = (int)Math.Round(b.CharacterSize.X);
                r1.Height = (int)Math.Round(b.CharacterSize.Y);
                r1.X = (int)Math.Round(b.Position.X);
                r1.Y = (int)Math.Round(b.Position.Y);
                foreach(Enemy e in enemyList)
                {
                    r2.Width  = (int)Math.Round(e.CharacterSize.X);
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
        // endof BulletCollision()
    }
}
