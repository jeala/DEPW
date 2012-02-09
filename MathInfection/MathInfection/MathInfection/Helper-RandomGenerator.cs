using System;
using Microsoft.Xna.Framework;

namespace MathInfection
{
    class RandomGenerator
    {
        private Random rand;
        private int speedLimitMin;
        private int speedLimitMax;
        private float weight;

        public RandomGenerator()
        {
            rand = new Random();
            speedLimitMin = 3;
            speedLimitMax = 5;
            weight = .1f;
        }

        public Vector2 RandomPosition(Vector2 winSize, Vector2 objSize)
        {
            return new Vector2(rand.Next((int)(winSize.X - objSize.X)),
                               rand.Next((int)(winSize.Y - objSize.Y)));
        }

        public int RandomMoveStrategy(int numStrategies)
        {
            return rand.Next(numStrategies);
        }

        public Vector2 RandomVelocity()
        {
            int firstSign = PositiveOrNegative() ? 1 : -1;
            int secondSign = PositiveOrNegative() ? 1 : -1;
            return new Vector2(firstSign * rand.Next(speedLimitMin, speedLimitMax),
                              secondSign * rand.Next(speedLimitMin, speedLimitMax));
        }

        public float RandomLerpSpeed()
        {
            return RandomFloatLessThanOne();
        }

        public float RandomCatSpeed()
        {
            return RandomFloatLessThanOne();
        }


        private bool PositiveOrNegative()
        {
            return rand.Next() % 2 == 0;
        }

        private float RandomFloatLessThanOne()
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
