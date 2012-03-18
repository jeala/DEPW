using Microsoft.Xna.Framework;

namespace MathInfection
{
    class CatmullRomMover : IMoverStrategy
    {
        private ICharacter parent;
        private Vector2 vec1, vec2, vec3, vec4;
        private float speed;
        private float location;

        public CatmullRomMover(ICharacter caller, Vector2 v1, Vector2 v2, Vector2 v3, Vector2 v4, float spd)
        {
            parent = caller;
            vec1 = v1;
            vec2 = v2;
            vec3 = v3;
            vec4 = v4;
            speed = spd;
            location = 0;
        }

        public Vector2 update(Vector2 dummy, int score)
        {
            location += speed;
            if (location > 1.0f || location < 0.0f) location = 0.0f;

            Vector2 newPosition = Vector2.CatmullRom(vec1, vec2, vec3, vec4, location);
            if(CheckEqual(newPosition, vec2) || CheckEqual(newPosition, vec3))
            {
                vec1 = RandomGenerator.RandomPosition(parent.WindowSize, parent.CharacterSize);
                vec2 = vec3;
                vec3 = TargetPlayer(score);
                vec4 = RandomGenerator.RandomPosition(parent.WindowSize, parent.CharacterSize);
                location = 0;
                newPosition = Vector2.CatmullRom(vec1, vec2, vec3, vec4, location);
            }
            newPosition = BounceEdges(newPosition);
            return newPosition;
        }

        private bool CheckEqual(Vector2 newPos, Vector2 oldPos)
        {
            float tempX = newPos.X - oldPos.X;
            float tempY = newPos.Y - oldPos.Y;
            tempX = tempX > 0 ? tempX : -tempX;
            tempY = tempY > 0 ? tempY : -tempY;
            if(tempX < speed && tempY < speed)
            {
                return true;
            }
            return false;
        }

        private Vector2 BounceEdges(Vector2 newPosition)
        {
            Vector2 newPos = newPosition;
            if(newPos.X < 0)
            {
                newPos.X = 0;
            }
            if(newPos.Y < 0)
            {
                newPos.Y = 0;
            }
            if(newPos.X > parent.WindowSize.X - parent.CharacterSize.X)
            {
                float over = newPos.X + parent.CharacterSize.X - parent.WindowSize.X;
                newPos.X -= over;
            }
            if(newPos.Y > parent.WindowSize.Y - parent.CharacterSize.Y)
            {
                float over = newPos.Y + parent.CharacterSize.Y - parent.WindowSize.Y;
                newPos.Y -= over;
            }
            return newPos;
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
