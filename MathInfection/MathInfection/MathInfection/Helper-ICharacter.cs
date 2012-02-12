using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MathInfection
{


    interface ICharacter
    {
        Vector2 CharacterSize { get; }
        Vector2 WindowSize { get; }
        Vector2 PlayerPosition { get; }

        void update(Vector2 playerPosition);
        void draw(SpriteBatch spriteBatch);
    }
}
