﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MathInfection
{
    public class Bullet : ICharacter
    {
        private readonly Helper_Animation anim;
        private Vector2 position;
        private Vector2 characterSize;
        private readonly Vector2 windowSize;
        private Vector2 playerPosition;
        private readonly int bulletSpeed;
        private readonly int bulletDamage;
        private bool isValid;

        public Bullet(Texture2D tex, Vector2 pos, Vector2 cSize, Vector2 wSize,
                      Vector2 playerPos, int bSpeed, int bDamage)
        {
            position = pos;
            characterSize = cSize;
            windowSize = wSize;
            playerPosition = playerPos;
            bulletSpeed = bSpeed;
            bulletDamage = bDamage;
            isValid = true;
            anim = new Helper_Animation(tex, pos, 2, 100, 0, 0, 10, 10);
        }

        public Vector2 Position
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

        public int Damage
        {
            get
            {
                return bulletDamage;
            }
        }

        public bool IsValid
        {
            set
            {
                isValid = value;
            }
            get
            {
                return isValid;
            }
        }

        public void update(Vector2 playerPos, GameTime gametime, int dummy)
        {
            playerPosition = playerPos;
            position.Y -= bulletSpeed;
            if(position.Y + characterSize.Y <= 0)
            {
                isValid = false;
            }
            anim.Update(gametime, position);
        }

        public void draw(SpriteBatch sb)
        {
            anim.Draw(sb);
        }
    }
}
