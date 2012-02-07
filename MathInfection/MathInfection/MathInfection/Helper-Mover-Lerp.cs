using Microsoft.Xna.Framework;

namespace MathInfection
{
    class LerpMover : IMoverStrategy
    {
        private ICharacter parent;
        private Vector2 initialPosition;
        private Vector2 finalPosition;
        private float location;
        private float speed;

        public LerpMover(Vector2 init, Vector2 fin, float spd, ICharacter parent)
        {
            this.parent = parent;
            initialPosition = init;
            finalPosition = fin;
            location = 0;
            speed = spd;
        }

        public Vector2 update(Vector2 dummy)
        {
            location += speed;

            if (location > 1.0f || location < .0f) location = .0f;

            Vector2 newPosition = Vector2.Lerp(initialPosition, finalPosition, location);
            newPosition.X = RoundFloat.Round(newPosition.X);
            newPosition.Y = RoundFloat.Round(newPosition.Y);

            if(newPosition == initialPosition)
            {
                initialPosition = finalPosition;
                finalPosition = RandomGenerator.RandomPosition(this.);
                newPosition = Vector2.Lerp(initialPosition, finalPosition, location);
            }
            return newPosition;
        }
    }
}
