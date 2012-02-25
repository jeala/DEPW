using Microsoft.Xna.Framework;

namespace MathInfection
{
    public class Main : Game
    {
        private GraphicsDeviceManager graphics;
        private ScreenManager screenManager;

        private static readonly string[] preloadAssets = {"gradient"};

        public Main()
        {
            Content.RootDirectory = "Content";

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth  = 1000;
            graphics.PreferredBackBufferHeight = 660;

            screenManager = new ScreenManager(this);
            Components.Add(screenManager);

            screenManager.AddScreen(new BackgroundScreen(), null);
            screenManager.AddScreen(new MainMenuScreen(), null);
        }

        protected override void LoadContent()
        {
            foreach(string asset in preloadAssets)
            {
                Content.Load<object>(asset);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }
    }
}