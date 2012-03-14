using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MathInfection
{
    public class Player : ICharacter
    {
        private readonly Texture2D texture;
        private Vector2 position;
        private Helper_Animation jet1;
        private Helper_Animation jet2;
        private readonly Vector2 velocity;
        private Vector2 characterSize;
        private Vector2 windowSize;
        private int score;
        private bool startBoost;
        private int health;
        private bool wasHit;
        private string enemyType;
        private int maxAddSub;
        private int maxMul;
        private int maxDiv;

        public Player(Texture2D tex, Texture2D jtex, Texture2D jtex2, Vector2 pos,
                                        Vector2 vel, Vector2 cSize, Vector2 wSize)
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
            maxAddSub = 20;
            maxMul = 10;
            maxDiv = 30;
            jet1 = new Helper_Animation(jtex, new Vector2(pos.X, (float)pos.Y - 80),
                                                               2, 100, 0, 0, 26, 6);
            jet2 = new Helper_Animation(jtex2, new Vector2(pos.X, (float)pos.Y - 80),
                                                               2, 100, 0, 0, 26, 9);
        }

        public int MaxAddSub
        {
            get
            {
                return maxAddSub;
            }
            set
            {
                maxAddSub = value;
            }
        }

        public int MaxMul
        {
            get
            {
                return maxMul;
            }
            set
            {
                maxMul = value;
            }
        }

        public int MaxDiv
        {
            get
            {
                return maxDiv;
            }
            set
            {
                maxDiv = value;
            }
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
            set
            {
                score = value;
            }
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
            set
            {
                health = value;
            }
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

        public void update(Vector2 dummy, GameTime gametime, int score)
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

            if (startBoost) jet2.Update(gametime, new Vector2(position.X + 5, position.Y + 43));
            else jet1.Update(gametime, new Vector2(position.X + 5, position.Y + 44));
        }

        public void draw(SpriteBatch sb)
        {
            sb.Draw(texture, position, Color.White);
            if (startBoost) jet2.Draw(sb);
            else jet1.Draw(sb);
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
