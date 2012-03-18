﻿using Microsoft.Xna.Framework;

namespace MathInfection
{
    class LerpMover : IMoverStrategy
    {
        private ICharacter parent;
        private Vector2 initialPosition;
        private Vector2 finalPosition;
        private float speed;
        private float location;

        public LerpMover(ICharacter caller, Vector2 init, Vector2 fin, float spd)
        {
            parent = caller;
            initialPosition = init;
            finalPosition = fin;
            speed = spd;
            location = 0;
        }

        public Vector2 update(Vector2 dummy, int score)
        {
            location += speed;

            if (location > 1.0f || location < .0f) location = .0f;

            Vector2 newPosition = Vector2.Lerp(initialPosition, finalPosition, location);
            if(CheckEqual(newPosition))
            {
                initialPosition = finalPosition;
                finalPosition = TargetPlayer(score);
                newPosition = Vector2.Lerp(initialPosition, finalPosition, location);
            }
            return newPosition;
        }

        private bool CheckEqual(Vector2 newPosition)
        {
            float tempX = newPosition.X - initialPosition.X;
            float tempY = newPosition.Y - initialPosition.Y;
            tempX = tempX > 0 ? tempX : -tempX;
            tempY = tempY > 0 ? tempY : -tempY;
            if(tempX <= speed && tempY <= speed)
            {
                return true;
            }
            return false;
        }

        private Vector2 TargetPlayer(int score)
        {
            bool targetPlayer = RandomGenerator.RandomChasePlayer(parent.GetType().ToString()
                                                              == "MathInfection.Boss", score);
            if(targetPlayer)
            {
                return parent.PlayerPosition;
            }
            return RandomGenerator.RandomPosition(parent.WindowSize, parent.CharacterSize);
        }
    }
}
