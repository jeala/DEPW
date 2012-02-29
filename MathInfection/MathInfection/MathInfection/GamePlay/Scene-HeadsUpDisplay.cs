using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MathInfection
{
    class HeadsUpDisplay
    {
        private readonly Vector2 hudPosition;
        private string hudContent;

        public HeadsUpDisplay(Vector2 pos)
        {
            hudPosition = pos;
            hudContent = "";
        }

        public void update(Player p1, int eLeft)
        {
            int score = p1.Score;
            int health = p1.Health;

            hudContent = "Enemy Left: " + eLeft + "     Score: " + score + "     Health :" + health;
        }

        public void draw(SpriteFont font, SpriteBatch batch)
        {
            batch.DrawString(font, hudContent, hudPosition, Color.White);
        }
    }
}
