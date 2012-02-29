using System;
using Microsoft.Xna.Framework;

namespace MathInfection
{
    public static class RandomGenerator
    {
        static private readonly Random rand;
        static private readonly int speedLimitMin;
        static private readonly int speedLimitMax;
        static private readonly float weight;

        static RandomGenerator()
        {
            rand = new Random();
            speedLimitMin = 3;
            speedLimitMax = 5;
            weight = .1f;
        }

        public static Vector2 RandomPosition(Vector2 winSize, Vector2 objSize)
        {
            return new Vector2(rand.Next((int)Math.Round(winSize.X - objSize.X)),
                               rand.Next((int)Math.Round(winSize.Y - objSize.Y)));
        }

        public static int RandomMoveStrategy(int numStrategies)
        {
            return rand.Next(numStrategies);
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

        public static bool RandomChasePlayer(bool isBoss)
        {
            float temp = (float)rand.NextDouble();
            if(isBoss)
            {
                temp *= 2;
            }
            if(temp >= .5f)
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

        public static string RandomQuestion(int currentScore, out int answer,
                                                         out float[] answers)
        {
            answer = rand.Next(1, 5);
            answers = new float[]{0, 0, 0, 0};

            string question = "";
            float firstVal = rand.Next(10);
            float SecondVal = rand.Next(10);
            while(SecondVal == 0)
            {
                SecondVal = rand.Next(10);
            }
            int myOperator = Random4Choice1(currentScore);
            float correctAnswer;

            switch(myOperator)
            {
                case 1:
                    question = firstVal + " + " + SecondVal + " = ?";
                    correctAnswer = firstVal + SecondVal;
                    break;
                case 2:
                    question = firstVal + " - " + SecondVal + " = ?";
                    correctAnswer = firstVal - SecondVal;
                    break;
                case 3:
                    question = firstVal + " * " + SecondVal + " = ?";
                    correctAnswer = firstVal * SecondVal;
                    break;
                default:
                    correctAnswer = firstVal / SecondVal;
                    question = firstVal + " / " + SecondVal + " = ?";
                    break;
            }

            for(int i = 1; i < 5; i++)
            {
                if(i == answer)
                {
                    answers[i-1] = correctAnswer;
                }
                else
                {
                    answers[i-1] = WrongAnswer(correctAnswer);
                }
            }
            return question;
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

        private static float WrongAnswer(float cAnswer)
        {
            switch(Random4Choice1(0))
            {
                case 1:
                    return cAnswer + 1;
                case 2:
                    return cAnswer + 2;
                case 3:
                    return cAnswer * 2;
                default:
                    return cAnswer - 3;
            }
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
