using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MathInfection
{
    class Boss : ICharacter
    {
        private IMoverStrategy mover;
        private readonly Texture2D texture;
        private Vector2 position;
        private Vector2 characterSize;
        private Vector2 windowSize;

        public Boss(int moverID, Texture2D tex, Vector2 pos, Vector2 cSize, Vector2 wSize)
        {
            mover = setMover(moverID);
            texture = tex;
            position = pos;
            characterSize = cSize;
            windowSize = wSize;
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

        public void update()
        {
            position = mover.update(position);
        }

        public void draw(SpriteBatch sb)
        {
            sb.Draw(texture, position, Color.White);
        }

        private IMoverStrategy setMover(int moverID)
        {
            switch(moverID)
            {
                case 1:
                    return new VelocityMover(this, RandomGenerator.RandomVelocity());
                case 2:
                    return new LerpMover(this, RandomGenerator.RandomPosition(windowSize, characterSize),
                                               RandomGenerator.RandomPosition(windowSize, characterSize),
                                               RandomGenerator.RandomLerpSpeed());
                case 3:
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
