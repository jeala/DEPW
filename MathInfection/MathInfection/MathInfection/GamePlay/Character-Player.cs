using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MathInfection
{
    class Player : ICharacter
    {
        private readonly Texture2D texture;
        //Changed to public for testing purposes
        public Vector2 position;
        private Vector2 velocity;
        private Vector2 characterSize;
        private Vector2 windowSize;
        private int health;
        private bool wasHit;
        private bool startBoost;
<<<<<<< HEAD:MathInfection/MathInfection/MathInfection/Character-Player.cs

        public Player(Texture2D tex, Vector2 pos, Vector2 vel, Vector2 cSize, Vector2 wSize)
=======
        private int health;
        private bool wasHit;
        private string enemyType;

        public Player(Texture2D tex, Vector2 pos, Vector2 vel,
                                 Vector2 cSize, Vector2 wSize)
>>>>>>> 2b6ae63cf727e98dd6fcaf91ff4d7e699c286703:MathInfection/MathInfection/MathInfection/GamePlay/Character-Player.cs
        {
            texture = tex;
            position = pos;
            velocity = vel;
            characterSize = cSize;
            windowSize = wSize;
            health = 100;
            wasHit = false;
<<<<<<< HEAD:MathInfection/MathInfection/MathInfection/Character-Player.cs
            startBoost = false;
=======
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
>>>>>>> 2b6ae63cf727e98dd6fcaf91ff4d7e699c286703:MathInfection/MathInfection/MathInfection/GamePlay/Character-Player.cs
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
            set
            {
                score = value;
            }
            get
            {
                return windowSize;
            }
        }

        public bool StartBoost
        {
            set
            {
                startBoost = value;
            }
        }

<<<<<<< HEAD:MathInfection/MathInfection/MathInfection/Character-Player.cs
        public bool IsAlive()
        {
            return health > 0;
        }

        public void update()
        {
            if (wasHit)
            {
                health--;
                wasHit = false;
=======
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
>>>>>>> 2b6ae63cf727e98dd6fcaf91ff4d7e699c286703:MathInfection/MathInfection/MathInfection/GamePlay/Character-Player.cs
            }

<<<<<<< HEAD:MathInfection/MathInfection/MathInfection/Character-Player.cs
            if(IsAlive())
=======
        public bool IsAlive()
        {
            return health > 0;
        }

        public void update(Vector2 dummy)
        {
            Vector2 speed = velocity;
            if(startBoost)
>>>>>>> 2b6ae63cf727e98dd6fcaf91ff4d7e699c286703:MathInfection/MathInfection/MathInfection/GamePlay/Character-Player.cs
            {
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
<<<<<<< HEAD:MathInfection/MathInfection/MathInfection/Character-Player.cs
    }
}
=======
    }
}
>>>>>>> 2b6ae63cf727e98dd6fcaf91ff4d7e699c286703:MathInfection/MathInfection/MathInfection/GamePlay/Character-Player.cs
