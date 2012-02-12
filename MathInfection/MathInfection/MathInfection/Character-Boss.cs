using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MathInfection
{
    class Boss : ICharacter
    {
        private IMoverStrategy mover;
        private readonly Texture2D texture;
        private Vector2 position;
        private Vector2 characterSize;
        private Vector2 windowSize;
        private Vector2 playerPosition;
        private int health;
        private bool wasHit;
        private int damage;

        public Boss(int moverId, Texture2D tex, Vector2 pos, Vector2 cSize, Vector2 wSize, int hp)
        {
            mover = SetMover(moverId);
            texture = tex;
            position = pos;
            characterSize = cSize;
            windowSize = wSize;
            playerPosition = Vector2.Zero;
            health = hp;
            wasHit = false;
        }

        public Vector2 CharacterSize
        {
            get
            {
                return characterSize;
            }
        }

        public Vector2 WindowSize
        {
            get
            {
                return windowSize;
            }
        }

        public Vector2 PlayerPosition
        {
            get
            {
                return playerPosition;
            }
        }

        public bool WasHit
        {
            set
            {
                wasHit = value;
            }
        }

        public int Damage
        {
            set
            {
                damage = value;
            }
        }

        public bool IsAlive()
        {
            return health > 0;
        }

        public void update(Vector2 playerPos)
        {
            if(wasHit)
            {
                health -= damage;
            }
            if (health > 0)
            {
                playerPosition = playerPos;
                position = mover.update(position);
            }
        }

        public void draw(SpriteBatch sb)
        {
            sb.Draw(texture, position, Color.White);
        }

        private IMoverStrategy SetMover(int moverId)
        {
            switch(moverId)
            {
                case 0:
                    return new VelocityMover(this, RandomGenerator.RandomVelocity());
                case 1:
                    return new LerpMover(this, RandomGenerator.RandomPosition(windowSize, characterSize),
                                               RandomGenerator.RandomPosition(windowSize, characterSize),
                                               RandomGenerator.RandomLerpSpeed());
                case 2:
                    return new CatmullRomMover(this, RandomGenerator.RandomPosition(windowSize, characterSize),
                                                     RandomGenerator.RandomPosition(windowSize, characterSize),
                                                     RandomGenerator.RandomPosition(windowSize, characterSize),
                                                     RandomGenerator.RandomPosition(windowSize, characterSize),
                                                     RandomGenerator.RandomCatSpeed());
                default:
                    return new VelocityMover(this, RandomGenerator.RandomVelocity());
            }
        }
    }
}
