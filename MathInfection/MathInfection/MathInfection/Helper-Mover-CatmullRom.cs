using Microsoft.Xna.Framework;

namespace MathInfection
{
    class CatmullRomMover : IMoverStrategy
    {
        private ICharacter parent;

        public CatmullRomMover()
        {
            
        }

        public Vector2 update(Vector2 dummy)
        {
            return dummy;
        }
    }
}
