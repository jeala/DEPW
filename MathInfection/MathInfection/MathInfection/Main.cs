using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace MathInfection
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Main : Game
    {
        GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont hudFont;
        private GamePadState oldGamePadState;
        private KeyboardState oldKeyboardState;

        private RandomGenerator rand;
        private HeadsUpDisplay hud;

        private Player player1;
        private Player player2;
        private List<Enemy> enemyList;
        private List<Boss> bossList;
        private Texture2D player1Texture;
        private List<Texture2D> enemyTexList;
        private List<Texture2D> bossTexList;

        private Vector2 playerSize;
        private Vector2 windowSize;
        private Vector2 initialPlayerPosition;
        private Vector2 playerVelocity;

        private bool singleMode;
        private int numPlayers;
        private int numMoveStrategies;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            oldGamePadState = GamePad.GetState(PlayerIndex.One);
            rand = new RandomGenerator();
            hud = new HeadsUpDisplay();
            enemyList = new List<Enemy>();
            bossList = new List<Boss>();
            enemyTexList = new List<Texture2D>();
            bossTexList = new List<Texture2D>();
            windowSize = new Vector2(Window.ClientBounds.Width, Window.ClientBounds.Height);
            initialPlayerPosition = new Vector2(windowSize.X/2, windowSize.Y-30);
            playerVelocity = new Vector2(3, 3);
            // TODO: determine game mode: single or versus. Use single for now.
            singleMode = true;
            numPlayers = singleMode ? 1 : 2;
            numMoveStrategies = 3;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            hudFont = Content.Load<SpriteFont>("HUDFont");

            player1Texture = Content.Load<Texture2D>(@"CharacterImages/Player1");
            playerSize = new Vector2(player1Texture.Width, player1Texture.Height);
            if(!singleMode)
            {
                // TODO: use player1's texture for now, might make a different tex for player2 later.
                // player2Texture = Content.Load<Texture2D>(@"CharacterImages/Player2");
            }
            player1 = new Player(player1Texture, initialPlayerPosition, playerVelocity, playerSize, windowSize);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            CheckBoostKeyPress();

            if(player1.IsAlive())
            {
                player1.update();
            }
            else
            {
                // TODO: gameover implementation
            }

            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            if(player1.IsAlive())
            {
                player1.draw(spriteBatch);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void CheckBoostKeyPress()
        {
            GamePadState newGamePadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState newKeyboardState = Keyboard.GetState();
            // NOTE: assume user wouldn't switch between keyboard and gamepad while speeding up
            if (newGamePadState.IsButtonDown(Buttons.A) || newKeyboardState.IsKeyDown(Keys.LeftShift))
            {
                if(!oldGamePadState.IsButtonDown(Buttons.A) || !oldKeyboardState.IsKeyDown(Keys.LeftShift))
                {
                    player1.StartBoost = true;
                }
            }
            else if(oldGamePadState.IsButtonDown(Buttons.A) || oldKeyboardState.IsKeyDown(Keys.LeftShift))
            {
                player1.StartBoost = false;
            }
            oldGamePadState = newGamePadState;
            oldKeyboardState = newKeyboardState;
        }
    }
}
