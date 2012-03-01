using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MathInfection
{


    interface ICharacter
    {
        Vector2 Position { get; }
        Vector2 CharacterSize { get; }
        Vector2 WindowSize { get; }
        Vector2 PlayerPosition { get; }

        void update(Vector2 playerPosition, GameTime gametime);
        void draw(SpriteBatch spriteBatch);
    }
}
