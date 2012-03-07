using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MathInfection
{
    public class Health
    {
        public Vector2 healthPosition;
        public Vector2 healthDimensions;
        public Vector2 speed;
        public bool drawIcon;

        public Health(Vector2 position)
        {
            speed = new Vector2(2, 2);
            healthDimensions = new Vector2(64, 64);
            healthPosition = position;
            drawIcon = false;
        }

        public void update()
        {
            healthPosition.X += speed.X;
            healthPosition.Y += speed.Y;
            BounceEdge();
        }

        public void BounceEdge()
        {
            if (healthPosition.X < 0)
            {
                speed.X = -speed.X;
            }
            if (healthPosition.Y < 0)
            {
                speed.Y = -speed.Y;
            }
            if (healthPosition.Y + 64 > 660)
            {
                speed.Y = -speed.Y;
            }
            if (healthPosition.X + 64 > 1000)
            {
                speed.X = -speed.X;
            }
        }

        public void draw(Texture2D healthicon, SpriteBatch sb)
        {
            sb.Draw(healthicon, healthPosition, Color.White);
        }
    }
}
