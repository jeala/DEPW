using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MathInfection
{
    public class Player : ICharacter
    {
        private readonly Texture2D texture;
        private Vector2 position;
        private readonly Vector2 velocity;
        private Vector2 characterSize;
        private Vector2 windowSize;
        private int score;
        private bool startBoost;
        private int health;
        private bool wasHit;
        private string enemyType;

        public Player(Texture2D tex, Vector2 pos, Vector2 vel,
                                 Vector2 cSize, Vector2 wSize)
        {
            texture = tex;
            position = pos;
            velocity = vel;
            characterSize = cSize;
            windowSize = wSize;
            score = 0;
            startBoost = false;
            health = 100;
            wasHit = false;
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

        public Vector2 Position
        {
            get
            {
                return position;
            }
        }

        public Vector2 PlayerPosition
        {
            get
            {
                return position;
            }
        }

        public int Score
        {
            get
            {
                return score;
            }
        }

        public bool StartBoost
        {
            set
            {
                startBoost = value;
            }
        }

        public int Health
        {
            get
            {
                return health;
            }
        }

        public bool WasHit
        {
            get
            {
                return wasHit;
            }
            set
            {
                wasHit = value;
            }
        }

        public string EnemyType
        {
            get
            {
                return enemyType;
            }
            set
            {
                enemyType = value;
            }
        }

        public bool IsAlive()
        {
            return health > 0;
        }

        public void GetHit(int damage)
        {
            health -= damage;
        }

        public void GetPoints(int answerTimeLeft)
        {
            bool isBoss = enemyType == "MathInfection.Boss";
            int bonus = answerTimeLeft * 2;
            if(isBoss)
            {
                score += (100 + bonus);
            }
            else
            {
                score += (20 + bonus);
            }
        }

        public void update(Vector2 dummy)
        {
            Vector2 speed = velocity;
            if(startBoost)
            {
                speed = velocity * 2;
            }

            if(Keyboard.GetState().IsKeyDown(Keys.W) ||
               GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > .25)
            {
                position.Y -= speed.Y;
            }
            if(Keyboard.GetState().IsKeyDown(Keys.S) ||
               GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < -.25)
            {
                position.Y += speed.Y;
            }
            if(Keyboard.GetState().IsKeyDown(Keys.A) ||
               GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < -.25)
            {
                position.X -= speed.X;
            }
            if(Keyboard.GetState().IsKeyDown(Keys.D) ||
               GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > .25)
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

        public void QuestionResult(bool answerCorrect, int answerTimeUsed)
        {

        }
    }
}
