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

        private int numberMoveStrategies;
        private bool singleMode;
        private int numPlayers;

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
            // TODO: build constructors for all stuff below
            rand = new RandomGenerator();
            hud = new HeadsUpDisplay();
            player1 = new Player();
            enemyList = new List<Enemy>();
            bossList = new List<Boss>();
            enemyTexList = new List<Texture2D>();
            bossTexList = new List<Texture2D>();
            windowSize = new Vector2(Window.ClientBounds.Width, Window.ClientBounds.Height);
            numberMoveStrategies = 3;
            // TODO: find a way to determine game mode: single or versus. Use single for now.
            singleMode = true;
            numPlayers = singleMode ? 1 : 2;

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


            // TODO: use this.Content to load your game content here
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
