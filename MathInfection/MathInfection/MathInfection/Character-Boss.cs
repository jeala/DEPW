using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MathInfection
{
    class Boss : ICharacter
    {
        private IMoverStrategy mover;
        private Vector2 characterSize;
        private Vector2 windowSize;

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
            
        }

        public void draw()
        {
            
        }
    }
}
