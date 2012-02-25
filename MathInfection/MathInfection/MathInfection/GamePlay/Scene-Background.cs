using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MathInfection
{
    class Background
    {
        private Vector2 screenPos, origin, textureSize;
        private Texture2D myTexture;
        private int screenHeight;

        public void Load(GraphicsDevice device, Texture2D backgroundTexture)
        {
            myTexture = backgroundTexture;
            screenHeight = device.Viewport.Height;
            int screenWidth = device.Viewport.Width;

            origin = new Vector2(myTexture.Width /2f, 0f);
            screenPos = new Vector2(screenWidth / 2f, screenHeight / 2f);
            textureSize = new Vector2(0, myTexture.Height);
        }

        public void Update(float deltaY)
        {
            screenPos.Y += deltaY;
            screenPos.Y = screenPos.Y % myTexture.Height;
        }

        public void Draw(SpriteBatch sb)
        {
            if(screenPos.Y < screenHeight)
            {
                sb.Draw(myTexture, screenPos, null, Color.White, 0, origin, 1,
                        SpriteEffects.None, 0f);
            }
            sb.Draw(myTexture, screenPos - textureSize, null, Color.White, 0,
                    origin, 1, SpriteEffects.None, 0f);
        }
    }
}
