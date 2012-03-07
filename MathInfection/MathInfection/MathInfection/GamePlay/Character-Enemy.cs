﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MathInfection
{
    public class Enemy : ICharacter
    {
        private readonly IMoverStrategy mover;
        private readonly Helper_Animation anim;
        private Vector2 position;
        private readonly Vector2 characterSize;
        private readonly Vector2 windowSize;
        private Vector2 playerPosition;
        private int health;
        private readonly float resizeRatio;

        public Enemy(int moverId, Texture2D tex, Vector2 pos, Vector2 cSize,
                     Vector2 wSize, int hp, float resize)
        {
            mover = SetMover(moverId);
            position = pos;
            characterSize = cSize;
            windowSize = wSize;
            playerPosition = Vector2.Zero;
            health = hp;
            resizeRatio = resize;
            anim = new Helper_Animation(tex, pos, 2, 400, 0, 0, 64, 64);
        }

        public  Vector2 Position
        {
            get
            {
                return position;
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

        public Vector2 PlayerPosition
        {
            get
            {
                return playerPosition;
            }
        }

        public int Health
        {
            set
            {
                health = value;
            }
        }

        public float ResizeRation
        {
            get
            {
                return resizeRatio;
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

        public void update(Vector2 playerPos, GameTime gametime)
        {
            if (health > 0)
            {
                playerPosition = playerPos;
                position = mover.update(position);
                anim.Update(gametime, position);
            }
        }

        public void draw(SpriteBatch sb)
        {
            anim.Draw(sb, resizeRatio);
        }

        private IMoverStrategy SetMover(int moverId)
        {
            switch(moverId)
            {
                case 0:
                    return new VelocityMover(this, RandomGenerator.RandomVelocity());
                case 1:
                    return new LerpMover(this, RandomGenerator.RandomPosition(windowSize,
                                                               characterSize),
                                               RandomGenerator.RandomPosition(windowSize,
                                                               characterSize),
                                               RandomGenerator.RandomLerpSpeed());
                case 2:
                    return new CatmullRomMover(this,
                               RandomGenerator.RandomPosition(windowSize, characterSize),
                               RandomGenerator.RandomPosition(windowSize, characterSize),
                               RandomGenerator.RandomPosition(windowSize, characterSize),
                               RandomGenerator.RandomPosition(windowSize, characterSize),
                               RandomGenerator.RandomCatSpeed());
                default:
                    return new VelocityMover(this, RandomGenerator.RandomVelocity());
            }
        }
        // endof SetMover()
    }
}