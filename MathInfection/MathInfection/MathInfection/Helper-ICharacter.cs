using Microsoft.Xna.Framework;

namespace MathInfection
{
    interface ICharacter
    {
        Vector2 CharacterSize { get; }
        Vector2 WindowSize { get; }

        void update();
        void draw();
    }
}
