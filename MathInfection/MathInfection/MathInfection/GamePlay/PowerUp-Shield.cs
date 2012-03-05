using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MathInfection
{
    public class Shield
    {
        private Texture2D player_shield;
        public Vector2 player1_position;
        private Vector2 player1_size;
        private Vector2 shield_size;
        public Vector2 shield_sizeF; 
        private Rectangle r1 = new Rectangle();
        private Rectangle r2 = new Rectangle();
        public bool drawShieldF;
        public bool shield_active;
        public Vector2 location;
        public Vector2 speed;

        public Shield(Texture2D shieldP, Vector2 playerSize)
        {
            speed = new Vector2(1, 1);
            drawShieldF = false;
            shield_active = false;
            player_shield = shieldP;
            player1_size = playerSize;
            shield_size.X = shieldP.Width;
            shield_size.Y = shieldP.Height;
            shield_sizeF = new Vector2(42, 42);
        }


/*
        public void PickUpShield(Player player1)
        {

            r1.Width = (int)shield_sizeF.X;
            r1.Height = (int)shield_sizeF.Y;
            r1.X = (int)player1.PlayerPosition.X;
            r1.Y = (int)player1.PlayerPosition.Y;

            r2.Width = (int)player1_size.X;
            r2.Height = (int)player1_size.Y;
            r2.X = 100;
            r2.Y = 100;
            if (r1.Intersects(r2))
            {
                shield_active = true;
                
            }            
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
*/
    }
}
