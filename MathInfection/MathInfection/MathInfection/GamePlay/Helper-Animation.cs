using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MathInfection
{
    public class Helper_Animation
    {
        public int frames, left, top, width, height, currentframe = 0;
        private TimeSpan frameduration;
        private TimeSpan prevframe = TimeSpan.Zero;
        private Vector2 Position;
        public Texture2D texture;
        bool isstopped = false;
        private List<Rectangle> rects = new List<Rectangle>();

        public Helper_Animation(Texture2D texture, Vector2 position, int framenum,
                           int millisec, int left, int top, int width, int height)
        {
            //Create new 
            this.frames = framenum;
            this.left = left;
            this.top = top;
            this.width = width;
            this.height = height;
            this.texture = texture;
            this.Position = position;

            //Find out how long each frame should last
            frameduration = TimeSpan.FromMilliseconds(millisec);

            for (int i = 0; i < frames; i++)
            {
                Rectangle r;
                if (i == 0)
                {
                    r = new Rectangle(left, top, width, height);
                }
                else r = new Rectangle(left + (width * i), top, width, height);
                rects.Add(r);
            }
        }

        public void StopAnimationAtEnd()
        {
            if (currentframe == (frames - 1)) isstopped = true;
        }

        public void RestartAnimation()
        {
            currentframe = 0;
            isstopped = false;
        }

        public void Update(GameTime Gametime, Vector2 NewPosition)
        {
                //Get position to draw animation
            Position = NewPosition;

            if (isstopped == false)
            {
                //Loop the frames
                if ((Gametime.TotalGameTime - prevframe) > frameduration)
                {
                    if (currentframe >= (frames - 1))
                    {
                        currentframe = 0;
                    }
                    else
                    {
                        currentframe++;
                    }
                    prevframe = Gametime.TotalGameTime;
                }
            }
        }

        /// <summary>
        /// Draw an animation without a scale
        /// </summary>
        /// <param name="sprite"></param>
        public void Draw(SpriteBatch sprite)
        {
            sprite.Draw(texture, Position, rects[currentframe], Color.White);
        }

        /// <summary>
        /// Draw an animation with a scale
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="resizeRatio"></param>
        public void Draw(SpriteBatch sprite, float resizeRatio)
        {
            sprite.Draw(texture, Position, rects[currentframe], Color.White, 0,
                              Vector2.Zero, resizeRatio, SpriteEffects.None, 0);
        }
    }
}