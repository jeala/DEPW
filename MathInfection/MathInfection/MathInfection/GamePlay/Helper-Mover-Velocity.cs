using Microsoft.Xna.Framework;

namespace MathInfection
{
    class VelocityMover : IMoverStrategy
    {
        private ICharacter parent;
        private Vector2 velocity;

        public VelocityMover(ICharacter caller, Vector2 vel)
        {
            parent = caller;
            velocity = vel;
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

            if(position.X + parent.CharacterSize.X > parent.WindowSize.X)
            {
                float over = position.X + parent.CharacterSize.X - parent.WindowSize.X;
                position.X = position.X - (velocity.X - over);
                velocity.X *= -1;
            }

            if(position.Y + parent.CharacterSize.Y > parent.WindowSize.Y)
            {
                float over = position.Y + parent.CharacterSize.Y - parent.WindowSize.Y;
                position.Y = position.Y - (velocity.Y - over);
                velocity.Y *= -1;
            }

            return position;
        }
    }
}
