using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MathInfection
{
    public class Shield
    {
        public Vector2 player1_position;
        private Vector2 player1_size;
        private Vector2 shield_size;
        public Vector2 shield_sizeF; 
        private Rectangle r1 = new Rectangle();
        private Rectangle r2 = new Rectangle();
        public bool drawShieldF;
        public bool shield_active;
        public Vector2 shield_positionF;
        public Vector2 speed;

        public Shield(Vector2 position)
        {
            speed = new Vector2(1, 1);
            drawShieldF = false;
            shield_size = new Vector2(47, 54);
            shield_positionF = position;

            player1_size = new Vector2(35, 45);
            shield_sizeF = new Vector2(42, 42);
        }

        public void update()
        {
            shield_positionF.X += speed.X;
            shield_positionF.Y += speed.Y;
            BounceEdge();
        }

        public void BounceEdge()
        {
            if (shield_positionF.X < 0)
            {
                speed.X = -speed.X;
            }
            if (shield_positionF.Y < 0)
            {
                speed.Y = -speed.Y;
            }
            if (shield_positionF.Y + 64 > 660)
            {
                speed.Y = -speed.Y;
            }
            if (shield_positionF.X + 64 > 1000)
            {
                speed.X = -speed.X;
            }
        }

        public void draw(Texture2D shieldicon, SpriteBatch sb)
        {
            sb.Draw(shieldicon, shield_positionF, Color.White);
        }

        public void draw(Texture2D shieldicon, SpriteBatch sb, Vector2 playerpos)
        {
            sb.Draw(shieldicon, playerpos, Color.White);
        }
    }
}
