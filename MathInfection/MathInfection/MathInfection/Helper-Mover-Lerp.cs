using System;
using Microsoft.Xna.Framework;

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

        public Vector2 update(Vector2 dummy)
        {
            // TODO: implemenet random moves targetting at player
            location += speed;

            if (location > 1.0f || location < .0f) location = .0f;

            Vector2 newPosition = Vector2.Lerp(initialPosition, finalPosition, location);
            newPosition.X = RoundFloat.Round(newPosition.X);
            newPosition.Y = RoundFloat.Round(newPosition.Y);

            if(newPosition == initialPosition)
            {
                initialPosition = finalPosition;
                finalPosition = RandomGenerator.RandomPosition(parent.WindowSize, parent.CharacterSize);
                newPosition = Vector2.Lerp(initialPosition, finalPosition, location);
            }
            return newPosition;
        }
    }
}
