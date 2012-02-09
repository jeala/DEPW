using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MathInfection
{


    interface ICharacter
    {
        Vector2 CharacterSize { get; }
        Vector2 WindowSize { get; }

        void update();
        void draw(SpriteBatch spriteBatch);
    }
}
