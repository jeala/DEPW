using Microsoft.Xna.Framework;

namespace MathInfection
{
    class VelocityMover : IMoverStrategy
    {
        private Vector2 velocity;
        private Vector2 itemSize;
        private Vector2 windowSize;

        public VelocityMover(Vector2 vel, Vector2 iSize, Vector2 winSize)
        {
            velocity = vel;
            itemSize = iSize;
            windowSize = winSize;
        }

        public Vector2 update(Vector2 position)
        {
            position += velocity;
            return BounceEdges(position);
        }

        private Vector2 BounceEdges(Vector2 position)
        {
            if(position.X < 0)
            {
                position.X *= -1;
                velocity.X *= -1;
            }

            if(position.Y < 0)
            {
                position.Y *= -1;
                velocity.Y *= -1;
            }

            if(position.X + itemSize.X > windowSize.X)
            {
                float over = position.X + itemSize.X - windowSize.X;
                position.X = position.X - (velocity.X - over);
                velocity.X *= -1;
            }

            if(position.Y + itemSize.Y > windowSize.Y)
            {
                float over = position.Y + itemSize.Y - windowSize.Y;
                position.Y = position.Y - (velocity.Y - over);
                velocity.Y *= -1;
            }

            return position;
        }
    }
}
