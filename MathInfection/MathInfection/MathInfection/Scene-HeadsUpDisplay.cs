using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MathInfection
{
    class HeadsUpDisplay
    {
        private Vector2 hudPosition;
        private Vector2 msgPosition;
        private string hudContent;
        private string msgContent;
        private bool gameOver;

        public HeadsUpDisplay(Vector2 pos)
        {
            hudPosition = pos;
            msgPosition.X = hudPosition.X + 120;
            msgPosition.Y = hudPosition.Y + 300;
            gameOver = false;
        }

        public void update(Player p1, int eLeft)
        {
            int score = p1.Score;
            int health = p1.Health;

            hudContent = "Enemy Left: " + eLeft + "     Score: " + score + "     Health :" + health;
            if(health <= 0)
            {
                msgContent = "Sorry, you lost!";
                gameOver = true;
            }
            else if(health > 0 && eLeft == 0)
            {
                msgContent = "Cool, you win!";
                gameOver = true;
            }
        }

        public void draw(SpriteFont font, SpriteBatch batch)
        {
            if(gameOver)
            {
                batch.DrawString(font, msgContent, msgPosition, Color.White);
            }
            batch.DrawString(font, hudContent, hudPosition, Color.White);
        }
    }
}
