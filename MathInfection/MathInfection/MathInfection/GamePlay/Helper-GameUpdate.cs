using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace MathInfection
{
    public static class GameUpdate
    {
        public static void AddEnemy(List<Enemy> eList, int numEnemies, int numMovers,
                              List<Texture2D> enemyTexList, Vector2 wSize, Player p1)
        {
            int count = 0;
            while(count < numEnemies)
            {
                int enemyType = RandomGenerator.RandomInt(enemyTexList.Count);
                eList.Add(new Enemy(RandomGenerator.RandomMoveStrategy(numMovers, p1.Score),
                                       wSize, 100, RandomGenerator.RandomEnemySize(false)));
                int index = eList.Count - 1;
                switch(enemyType)
                {
                    case 0: eList[index].InitializeAnim(enemyTexList[0], 2, 400, 44, 40);
                        break;
                    case 1: eList[index].InitializeAnim(enemyTexList[1], 8, 200, 46, 50);
                        break;
                    case 2: eList[index].InitializeAnim(enemyTexList[2], 4, 150, 70, 50);
                        break;
                }
                count++;
            }
        }

        public static void UpdateEnemyList(List<Enemy> eList)
        {
            int index = 0;
            while(index < eList.Count)
            {
                if(!eList[index].IsAlive())
                {
                    eList.RemoveAt(index);
                    index--;
                }
                index++;
            }
        }

        public static void UpdateHealthList(ref List<Health> hlist, Player p1)
        {
            int index = 0;
            while (index < hlist.Count)
            {
                if (!hlist[index].drawIcon)
                {
                    hlist.RemoveAt(index);
                    index--;
                }
                index++;
            }
        }

        public static void UpdateShieldList(ref List<Shield> sList, Player p1)
        {
            int index = 0;
            while (index < sList.Count)
            {
                if (!sList[index].drawShieldF)
                {
                    sList.RemoveAt(index);
                    index--;
                }
                index++;
            }
        }

        public static void ModifyShield(ref Shield shield, SpriteBatch sb,
                                  Player player1, Texture2D player_shield)
        {
            Color color = new Color(0, 155, 155, 75);
            shield.player1_position.X = player1.PlayerPosition.X +
                                       (player_shield.Bounds.X / 2) - 6;
            shield.player1_position.Y = player1.PlayerPosition.Y +
                                           (player_shield.Bounds.Y / 2);
            sb.Draw(player_shield, shield.player1_position, color);
        }

        public static void UpdateBulletList(List<Bullet> bList)
        {
            int index = 0;
            while(index < bList.Count)
            {
                if(!bList[index].IsValid)
                {
                    bList.RemoveAt(index);
                    index--;
                }
                index++;
            }
        }

        public static void CheckInput(GameTime gameTime, Player player1,
                List<Bullet> dBulletList, List<Texture2D> bulletTexList, 
                          TimeSpan previousFireTime, TimeSpan dFireRate,
            Vector2 wSize, GameplayScreen caller, SoundEffect gunSound)
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
            bool gunSoundInstance = false;
            if (gameTime.TotalGameTime - previousFireTime > dFireRate)
            {
                if (newGamePadState.Triggers.Right > .2f ||
                    newKeyboardState.IsKeyDown(Keys.Space))
                {
                    gunSoundInstance = gunSound.Play();
                    Vector2 bSize = new Vector2(bulletTexList[0].Width,
                                                bulletTexList[0].Height);
                    Vector2 bPos = new Vector2(player1.PlayerPosition.X +
                                               player1.CharacterSize.X / 2,
                                               player1.PlayerPosition.Y);

                    dBulletList.Add(new Bullet(bulletTexList[0], bPos,
                                    new Vector2(bSize.X / 2, bSize.Y),
                                        wSize, Vector2.Zero, 10, 20));
                    caller.PreviousFireTime = gameTime.TotalGameTime;
                }
            }
            // endof Bullet Generation
        }

        public static void CheckCollision(List<Bullet> defaultBulletList,
                                        List<Enemy> enemyList, Player p1, 
                              ref bool shield_active, List<Health> hlist,
                     SpriteBatch sb, Texture2D heart, List<Shield> sList,
                          SoundEffect grabHealth, SoundEffect grabShield)
        {
            Rectangle r1 = new Rectangle();
            Rectangle r2 = new Rectangle();
            bool grabShieldInstance = false;
            bool grabHealthInstance = false;

            int index = 0;
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
                        double ratio = Math.Sqrt(e.ResizeRation);
                        r2.Width = (int)Math.Round(e.CharacterSize.X * ratio);
                        r2.Height = (int)Math.Round(e.CharacterSize.Y * ratio);
                        r2.X = (int)Math.Round(e.Position.X);
                        r2.Y = (int)Math.Round(e.Position.Y);
                        if(r1.Intersects(r2))
                        {
                            e.GetHit(b.Damage);
                            if (!e.IsAlive())
                            {
                                bool isBoss = e.GetType().ToString() ==
                                                     "MathInfection.Boss";
                                p1.Score += 100;
                                if (isBoss)
                                {
                                    p1.Score += 100;
                                }
                                GeneratePowerUps(hlist, sList, e.Position, ref index);
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
                double ratio = Math.Sqrt(e.ResizeRation);
                r2.Width = (int)Math.Round(e.CharacterSize.X * ratio);
                r2.Height = (int)Math.Round(e.CharacterSize.Y * ratio);
                r2.X = (int)Math.Round(e.Position.X);
                r2.Y = (int)Math.Round(e.Position.Y);

                if (r1.Intersects(r2))
                {
                    p1.EnemyType = e.GetType().ToString();
                    e.Health = 0;
                    if (shield_active)
                    {
                        shield_active = false;
                    }
                    else
                    {
                        p1.WasHit = true;
                        break;
                    }
                }
            }
            index = 0;
            while (index < hlist.Count)
            {
                if (hlist[index].drawIcon)
                {
                    r2.Width = (int)Math.Round(hlist[index].healthDimensions.X);
                    r2.Height = (int)Math.Round(hlist[index].healthDimensions.Y);
                    r2.X = (int)Math.Round(hlist[index].healthPosition.X);
                    r2.Y = (int)Math.Round(hlist[index].healthPosition.Y);

                    if (r1.Intersects(r2))
                    {
                        grabHealthInstance = grabHealth.Play();
                        hlist[index].drawIcon = false;
                        p1.Health += 10;
                    }
                }
                index++;
            }
            index = 0;
            while (index < sList.Count)
            {
                if (sList[index].drawShieldF)
                {
                    r2.Width = (int)sList[index].shield_sizeF.X;
                    r2.Width = (int)sList[index].shield_sizeF.Y;
                    r2.X = (int)sList[index].shield_positionF.X;
                    r2.Y = (int)sList[index].shield_positionF.Y;

                    if (r1.Intersects(r2))
                    {
                        grabShieldInstance = grabShield.Play();
                        sList[index].drawShieldF = false;
                        shield_active = true;
                    }
                }
                index++;
            }
            // endof Player Collision Detection
        }

        public static void UpdateGameData(GameData data, Player player)
        {
            data.CurrentScore = player.Score;
            if (data.TopScores.Count < data.TopScoreCapacity)
            {
                data.TopScores.Add(data.CurrentScore);
                data.TopScoresDateTime.Add(DateTime.Now);
            }
            else if (data.CurrentScore > data.TopScores.Min())
            {
                int lowest = data.TopScores.Min();
                int lowestIndex = data.TopScores.IndexOf(lowest);
                data.TopScores.RemoveAt(lowestIndex);
                data.TopScoresDateTime.RemoveAt(lowestIndex);
                data.TopScores.Add(data.CurrentScore);
                data.TopScoresDateTime.Add(DateTime.Now);
            }
        }

        public static string GetHighScores(bool fromMainMenu)
        {
            GameData data = FileIO.DeserializeFromXML();
            string highScores = "";
            if(data != null)
            {
                if(data.TopScores.Count > 0 &&
                  (data.TopScores.Count == data.TopScoresDateTime.Count))
                {
                    List<int> topscores = data.TopScores;
                    List<DateTime> tScoreTD = data.TopScoresDateTime;
                    int count = topscores.Count;
                    for(int i = 0; i < count; i++)
                    {
                        if(highScores.Length == 0 && !fromMainMenu)
                        {
                            highScores = "\n\n\nTop Scores";
                        }
                        int highest = topscores.Max();
                        int highestIndex = topscores.IndexOf(highest);
                        DateTime highestDT = tScoreTD[highestIndex];
                        string indexStr = (i + 1).ToString().PadLeft(2);
                        string highestStr = highest.ToString().PadLeft(7);
                        string space = "    :  ";
                        space = space.PadLeft(2);
                        highScores += "\n" + indexStr + "  " + highestStr +
                                                          space + highestDT;
                        topscores.RemoveAt(highestIndex);
                        tScoreTD.RemoveAt(highestIndex);
                    }
                }
            }
            return highScores;
        }

        public static string GetTotalScore()
        {
            GameData data = FileIO.DeserializeFromXML();
            string totalScore = "";
            if(data != null)
            {
                string total = data.CurrentScore.ToString();
                totalScore += "    Current Total: " + total;
            }
            return totalScore;
        }

        private static void GeneratePowerUps(List<Health> hList, List<Shield> sList,
                                                         Vector2 pos, ref int index)
        {
            int randInt = RandomGenerator.RandomInt(1, 8);
            switch(randInt)
            {
                case 1:
                        hList.Add(new Health(pos));
                        hList[index].drawIcon = true;
                        index++;
                        break;
                case 2:
                        sList.Add(new Shield(pos));
                        sList[index].drawShieldF = true;
                        index++;
                        break;
                default:
                    break;
            }
        }
        // endof public static class GameUpdate
    }
}
