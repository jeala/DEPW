using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace MathInfection
{
    public enum ScreenState
    {
        TransitionOn,
        Active,
        TransitionOff,
        Hidden,
    }

    public abstract class GameScreen
    {
        private bool isPopup = false;
        public bool IsPopup
        {
            get
            {
                return isPopup;
            }
            protected set
            {
                isPopup = value;
            }
        }

        private TimeSpan transitionOnTime = TimeSpan.Zero;
        public TimeSpan TransitionOnTime
        {
            get
            {
                return transitionOnTime;
            }
            protected set
            {
                transitionOnTime = value;
            }
        }

        private TimeSpan transitionOffTime = TimeSpan.Zero;
        public TimeSpan TransitionOffTime
        {
            get
            {
                return transitionOffTime;
            }
            protected set
            {
                transitionOffTime = value;
            }
        }

        private float transitionPosition = 1;
        public float TransitionPosition
        {
            get
            {
                return transitionPosition;
            }
            protected set
            {
                transitionPosition = value;
            }
        }

        public float TransitionAlpha
        {
            get
            {
                return 1f - transitionPosition;
            }
        }

        private ScreenState screenState = ScreenState.TransitionOn;
        public ScreenState ScreenState
        {
            get
            {
                return screenState;
            }
            protected set
            {
                screenState = value;
            }
        }

        private bool isExiting = false;
        public bool IsExiting
        {
            get
            {
                return isExiting;
            }
            protected internal set
            {
                isExiting = value;
            }
        }

        private bool otherScreenHasFocus;
        public bool IsActive
        {
            get
            {
                return !otherScreenHasFocus &&
                       (screenState == ScreenState.TransitionOn ||
                        screenState == ScreenState.Active);
            }
        }

        private ScreenManager screenManager;
        public ScreenManager ScreenManager
        {
            get
            {
                return screenManager;
            }
            internal set
            {
                screenManager = value;
            }
        }

        private PlayerIndex? controllingPlayer;
        public PlayerIndex? ControllingPlayer
        {
            get
            {
                return controllingPlayer;
            }
            internal set
            {
                controllingPlayer = value;
            }
        }

        public virtual void LoadContent() { }

        public virtual void UnloadContent() { }

        public virtual void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                      bool coveredByOtherScreen)
        {
            this.otherScreenHasFocus = otherScreenHasFocus;

            if(isExiting)
            {
                screenState = ScreenState.TransitionOff;

                if(!UpdateTransition(gameTime, transitionOffTime, 1))
                {
                    ScreenManager.RemoveScreen(this);
                }
            }
            else if(coveredByOtherScreen)
            {
                if(UpdateTransition(gameTime, transitionOffTime, 1))
                {
                    screenState = ScreenState.TransitionOff;
                }
                else
                {
                    screenState = ScreenState.Hidden;
                }
            }
            else
            {
                if(UpdateTransition(gameTime, transitionOnTime, -1))
                {
                    screenState = ScreenState.TransitionOn;
                }
                else
                {
                    screenState = ScreenState.Active;
                }
            }
        }

        private bool UpdateTransition(GameTime gameTime, TimeSpan time, int direction)
        {
            float transitionDelta;
            if(time == TimeSpan.Zero)
            {
                transitionDelta = 1;
            }
            else
            {
                transitionDelta = (float)(gameTime.ElapsedGameTime.TotalMilliseconds /
                                          time.TotalMilliseconds);
            }

            transitionPosition += transitionDelta * direction;

            if(((direction < 0) && (transitionPosition <= 0)) ||
               ((direction > 0) && (transitionPosition >= 1)))
            {
                transitionPosition = MathHelper.Clamp(transitionPosition, 0, 1);
                return false;
            }
            return true;
        }

        public virtual  void HandleInput(InputState input) { }

        public virtual void Draw(GameTime gameTime) { }

        public void ExitScreen()
        {
            if(transitionOffTime == TimeSpan.Zero)
            {
                screenManager.RemoveScreen(this);
            }
            else
            {
                isExiting = true;
            }
        }
    }
}
