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
            return new Vector2(rand.Next((int)(winSize.X - objSize.X)),
                               rand.Next((int)(winSize.Y - objSize.Y)));
        }

        public static int RandomMoveStrategy(int numStrategies)
        {
            return rand.Next(numStrategies);
        }

        public static Vector2 RandomVelocity()
        {
            int firstSign = PositiveOrNegative();
            int secondSign = PositiveOrNegative();
            return new Vector2(firstSign * rand.Next(speedLimitMin, speedLimitMax),
                              secondSign * rand.Next(speedLimitMin, speedLimitMax));
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
            if(temp > .6f)
            {
                temp -= .5f;
            }
            if(temp < -.6f)
            {
                temp += .5f;
            }
            if(isBoss)
            {
                temp *= 2;
            }
            return 1 + temp;
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
