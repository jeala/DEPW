using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MathInfection
{
    public class Player : ICharacter
    {
        private readonly Texture2D texture;
        private Vector2 position;
        private Vector2 velocity;
        private Vector2 characterSize;
        private Vector2 windowSize;
        private int score;
        private bool wasHit;
        private bool startBoost;

        public Player(Texture2D tex, Vector2 pos, Vector2 vel, Vector2 cSize, Vector2 wSize)
        {
            texture = tex;
            position = pos;
            velocity = vel;
            characterSize = cSize;
            windowSize = wSize;
            score = 1000;
            wasHit = false;
            startBoost = false;
        }

        public Vector2 CharacterSize
        {
            get
            {
                return characterSize;
            }
        }

        public Vector2 WindowSize
        {
            get
            {
                return windowSize;
            }
        }

        public Vector2 PlayerPosition
        {
            get
            {
                return position;
            }
        }

        public bool WasHit
        {
            set
            {
                wasHit = value;
            }
        }

        public bool StartBoost
        {
            set
            {
                startBoost = value;
            }
        }

        public void update(Vector2 dummy)
        {
            if (wasHit)
            {
                score--;
                wasHit = false;
            }

            Vector2 speed = velocity;
            if(startBoost)
            {
                speed = velocity * 2;
            }

            if(Keyboard.GetState().IsKeyDown(Keys.W) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > .25)
            {
                position.Y -= speed.Y;
            }
            if(Keyboard.GetState().IsKeyDown(Keys.S) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < -.25)
            {
                position.Y += speed.Y;
            }
            if(Keyboard.GetState().IsKeyDown(Keys.A) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < -.25)
            {
                position.X -= speed.X;
            }
            if(Keyboard.GetState().IsKeyDown(Keys.D) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > .25)
            {
                position.X += speed.X;
            }
            StopEdge();
        }

        public void draw(SpriteBatch sb)
        {
            sb.Draw(texture, position, Color.White);
        }

        private void StopEdge()
        {
            if(position.X < 0)
            {
                position.X = 0;
            }
            if(position.Y < 0)
            {
                position.Y = 0;
            }
            if(position.X + characterSize.X > windowSize.X)
            {
                position.X = windowSize.X - characterSize.X;
            }
            if(position.Y + characterSize.Y > windowSize.Y)
            {
                position.Y = windowSize.Y - characterSize.Y;
            }
        }
    }
}
