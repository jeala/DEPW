using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace MathInfection
{
    class Enhancement
    {
        //Currently working on        
        private Texture2D powerUpTex_shield;
        private Vector2 player1_position;
        private Vector2 player1_size;
        private bool hasShield = false;

        public Enhancement(Texture2D shield, Vector2 playerSize)
        {
            powerUpTex_shield = shield;
            player1_size.X = playerSize.X;
            player1_size.Y = playerSize.Y;
        }

        public void ModifyShield(SpriteBatch sb, Player player1)
        {
            player1_position.X = player1.position.X + player1_size.X/2 - 50;
            player1_position.Y = player1.position.Y + player1_size.Y/2 - 50;
            sb.Draw(powerUpTex_shield, player1_position, Color.White);
        }

    }
}
