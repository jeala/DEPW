/*
 * 2/2/2012
 * Currently draw is not working properly. The code to make the
 * character sprite move is currently working in Game1.cs, but
 * it is commented out. When uncommented, it will need other minor
 * adjustments, such as removing the call functions to this class
 * in order to work properly.
 * -Jason
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MathInfection
{
    public class Player : Microsoft.Xna.Framework.Game
    {

        Texture2D mainChar;
        Vector2 userPosition;
        int userSpeed = 5;

        public Player()
        {
            //For loading images, don't need to type "Player\\", can just type asset name
            Content.RootDirectory = "Content\\Player";
        }

        public void init()
        {
            //Character Sprite to be drawn in middle of screen
            userPosition.X = Window.ClientBounds.Width / 2;
            userPosition.Y = Window.ClientBounds.Height / 2;
        }

        public void load()
        {
            //Load character
            mainChar = Content.Load<Texture2D>("CharacterSprite");
        }

        public void UpdatePlayer()
        {
            KeyboardState newState = Keyboard.GetState();
            //Check for Left key
            if (newState.IsKeyDown(Keys.Left))
            {
                userPosition.X -= userSpeed;
            }
            //Check for Right Key
            if (newState.IsKeyDown(Keys.Right))
            {
                userPosition.X += userSpeed;
            }
            //Check for Up Key
            if (newState.IsKeyDown(Keys.Up))
            {
                userPosition.Y -= userSpeed;
            }
            //Check for Down Key
            if (newState.IsKeyDown(Keys.Down))
            {
                userPosition.Y += userSpeed;
            }

        }

        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mainChar, new Vector2(userPosition.X, userPosition.Y), Color.White);

        }
    }
}
