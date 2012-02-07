using Microsoft.Xna.Framework;

namespace MathInfection
{
    interface IMoverStrategy
    {
        Vector2 update(Vector2 position);
    }
}
