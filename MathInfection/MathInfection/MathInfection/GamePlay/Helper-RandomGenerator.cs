using System;
using Microsoft.Xna.Framework;

namespace MathInfection
{
    public static class RandomGenerator
    {
        private static readonly Random rand;
        private static readonly int speedLimitMin;
        private static readonly int speedLimitMax;
        private static readonly float weight;

        static RandomGenerator()
        {
            rand = new Random();
            speedLimitMin = 3;
            speedLimitMax = 5;
            weight = .1f;
        }

        public static int RandomInt()
        {
            return rand.Next();
        }

        public static int RandomInt(int bound)
        {
            return rand.Next(bound);
        }

        public static int RandomInt(int min, int max)
        {
            return rand.Next(min, max);
        }

        public static Vector2 RandomPosition(Vector2 winSize, Vector2 objSize)
        {
            Vector2 vec = new Vector2(rand.Next((int)Math.Round(winSize.X - objSize.X)),
                                     rand.Next((int)Math.Round(winSize.Y - objSize.Y)));
            return vec;
        }

        public static int RandomMoveStrategy(int numStrategies, int score)
        {
            if(score < 10000)
            {
                return rand.Next(numStrategies);
            }
            if(score < 30000)
            {
                return rand.Next(1, numStrategies);
            }
            return 2;
        }

        public static Vector2 RandomVelocity()
        {
            int firstSign = PositiveOrNegative();
            int secondSign = PositiveOrNegative();
            return new Vector2(firstSign * rand.Next(speedLimitMin,
                                                     speedLimitMax),
                               secondSign * rand.Next(speedLimitMin,
                                                      speedLimitMax));
        }

        public static float RandomLerpSpeed()
        {
            return RandomFloatLessThanOne();
        }

        public static float RandomCatSpeed()
        {
            return RandomFloatLessThanOne();
        }

        public static bool RandomChasePlayer(bool isBoss, int score)
        {
            float temp = (float)rand.NextDouble();
            int weight = 1;
            if(score > 10000)
            {
                weight = 2;
            }
            else if(score > 30000)
            {
                weight = 3;
            }
            temp *= weight;
            if(isBoss)
            {
                temp *= 2;
            }
            if(temp >= .7f)
            {
                return true;
            }
            return false;
        }

        public static float RandomEnemySize(bool isBoss)
        {
            int sign = PositiveOrNegative();
            float temp = (float)rand.NextDouble() * sign;
            if(temp > .5f)
            {
                temp -= .4f;
            }
            if(temp < -.5f)
            {
                temp += .4f;
            }
            if(isBoss && temp > 0)
            {
                temp *= -1;
            }
            return 1 + temp;
        }

        public static string RandomQuestion(Player p1, out int answer, out int[] answers)
        {
            answer = rand.Next(1, 5);
            answers = new[] {0, 0, 0, 0};

            string question = "";
            int firstVal;
            int SecondVal;
            int myOperator = Random4Choice1(p1.Score);
            int correctAnswer;

            switch(myOperator)
            {
                case 1:
                    firstVal = rand.Next(p1.MaxAddSub);
                    SecondVal = rand.Next(p1.MaxAddSub);
                    question = firstVal + " + " + SecondVal + " = ?";
                    correctAnswer = firstVal + SecondVal;
                    break;
                case 2:
                    firstVal = rand.Next(p1.MaxAddSub);
                    SecondVal = rand.Next(p1.MaxAddSub);
                    question = firstVal + " - " + SecondVal + " = ?";
                    correctAnswer = firstVal - SecondVal;
                    break;
                case 3:
                    firstVal = rand.Next(p1.MaxMul);
                    SecondVal = rand.Next(p1.MaxMul);
                    question = firstVal + " * " + SecondVal + " = ?";
                    correctAnswer = firstVal * SecondVal;
                    break;
                default:
                    firstVal = rand.Next(p1.MaxDiv);
                    SecondVal = rand.Next(p1.MaxDiv);
                    correctAnswer = HandleDivision(ref firstVal, ref SecondVal,
                                                                    p1.MaxDiv);
                    question = firstVal + " / " + SecondVal + " = ?";
                    break;
            }

            for(int i = 1; i < 5; i++)
            {
                if(i == answer)
                {
                    answers[i - 1] = correctAnswer;
                }
                else
                {
                    answers[i - 1] = WrongAnswer(correctAnswer);
                }
            }
            p1.MaxAddSub++;
            p1.MaxDiv++;
            p1.MaxMul++;
            return question;
        }

        public static int RandomNumberToAdd(int score)
        {
            if(score < 10000)
            {
                return rand.Next(0, 2);
            }
            if(score < 30000)
            {
                return rand.Next(1, 3);
            }
            return rand.Next(2, 4);
        }

        private static int HandleDivision(ref int fVal, ref int sVal, int max)
        {
            while(sVal == 0)
            {
                sVal = rand.Next(max);
            }
            while(fVal % sVal != 0)
            {
                fVal = rand.Next(max);
                sVal = rand.Next(max);
                while(sVal == 0)
                {
                    sVal = rand.Next(max);
                }
            }
            return fVal / sVal;
        }

        private static int Random4Choice1(int cScore)
        {
            int choice = rand.Next();
            if(choice % 2 == 0)
            {
                return 1;
            }
            if(choice % 3 == 0)
            {
                return 2;
            }
            if(choice % 5 == 0)
            {
                return 3;
            }
            return 4;
        }

        private static int WrongAnswer(int cAnswer)
        {
            int wAnswer;
            if(cAnswer == 0)
            {
                wAnswer = RandomInt(1, 5);
            }
            else
            {
                switch (rand.Next() % 10)
                {
                    case 0:
                        wAnswer = cAnswer + 1;
                        break;
                    case 1:
                        wAnswer = cAnswer + 2;
                        break;
                    case 3:
                        wAnswer = cAnswer + 3;
                        break;
                    case 4:
                        wAnswer = cAnswer + 4;
                        break;
                    case 5:
                        wAnswer = (cAnswer + 1) * 2;
                        break;
                    case 6:
                        wAnswer = cAnswer - 4;
                        break;
                    case 7:
                        wAnswer = cAnswer - 3;
                        break;
                    case 8:
                        wAnswer = cAnswer - 2;
                        break;
                    default:
                        wAnswer = cAnswer - 1;
                        break;
                }
                while(wAnswer == cAnswer)
                {
                    wAnswer++;
                }
            }
            return wAnswer;
        }

        private static int PositiveOrNegative()
        {
            return rand.Next() % 2 == 0 ? 1 : -1;
        }

        private static float RandomFloatLessThanOne()
        {
            float temp = (float)rand.NextDouble();
            if(temp >= .9f)
            {
                temp /= 9;
            }
            else if(temp >= .7f)
            {
                temp /= 7;
            }
            else if(temp >= .5f)
            {
                temp /= 5;
            }
            else if(temp >= .3f)
            {
                temp /= 3;
            }
            else if(temp >= .1f)
            {
                temp /= 1;
            }
            else
            {
                temp = .1f;
            }
            return temp * weight;
        }



    }
}