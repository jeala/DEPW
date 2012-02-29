
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MathInfection
{
    class Shield
    {
        private Texture2D player_shield;
        private Texture2D healthIcon;
        private Vector2 player1_position;
        private Vector2 player1_size;
        private Vector2 shield_size;
        private Vector2 shield_sizeF;
        Rectangle r1 = new Rectangle();
        Rectangle r2 = new Rectangle();

        public Shield(Texture2D shield, Vector2 playerSize, Texture2D health)
        {
            healthIcon = health;
            player_shield = shield;
            player1_size = playerSize;
            shield_size.X = shield.Width;
            shield_size.Y = shield.Height;
            shield_sizeF = new Vector2(42, 42);
        }

        public void ModifyShield(SpriteBatch sb, Player player1)
        {
            player1_position.X = player1.position.X + (player1_size.X / 2) - 22; //22 is aprox. half shield image width (43/2)
            player1_position.Y = player1.position.Y + (player1_size.Y / 2) - 24; //24 is half shield image height
            sb.Draw(player_shield, player1_position, Color.White);
        }

        public void PickUpShield(Player player1, ref bool shield)
        {

            r1.Width = (int)shield_sizeF.X;
            r1.Height = (int)shield_sizeF.Y;
            r1.X = (int)player1.position.X;
            r1.Y = (int)player1.position.Y;

            r2.Width = (int)player1_size.X;
            r2.Height = (int)player1_size.Y;
            r2.X = 100;
            r2.Y = 100;
            if (r1.Intersects(r2))
            {
                shield = true;
            }
        }

        public void PickUpHealth(Player player1)
        {

            r1.Width = (int)shield_sizeF.X;
            r1.Height = (int)shield_sizeF.Y;
            r1.X = (int)player1.position.X;
            r1.Y = (int)player1.position.Y;

            r2.Width = (int)player1_size.X;
            r2.Height = (int)player1_size.Y;
            r2.X = 300;
            r2.Y = 300;
            if (r1.Intersects(r2))
            {
                player1.GetHit(-20);
            }
        }

    }
}
