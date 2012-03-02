<<<<<<< HEAD
﻿using System;
using Microsoft.Xna.Framework;

namespace MathInfection
{
    class PlayerIndexEventArgs : EventArgs
    {
        private PlayerIndex playerIndex;

        public PlayerIndexEventArgs(PlayerIndex playerIndex)
        {
            this.playerIndex = playerIndex;
        }

        public PlayerIndex PlayerIndex
        {
            get
            {
                return playerIndex;
            }
        }
    }
}
=======
﻿using System;
using Microsoft.Xna.Framework;

namespace MathInfection
{
    class PlayerIndexEventArgs : EventArgs
    {
        private PlayerIndex playerIndex;

        public PlayerIndexEventArgs(PlayerIndex playerIndex)
        {
            this.playerIndex = playerIndex;
        }

        public PlayerIndex PlayerIndex
        {
            get
            {
                return playerIndex;
            }
        }
    }
}
>>>>>>> 2b6ae63cf727e98dd6fcaf91ff4d7e699c286703
