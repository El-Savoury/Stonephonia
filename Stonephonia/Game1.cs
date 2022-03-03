using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Stonephonia
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Player player;
        Texture2D spriteTest;

        #region Constructor
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            this.TargetElapsedTime = new TimeSpan(333333);
            Window.AllowUserResizing = true;
        }
        #endregion

        #region Initialise
        protected override void Initialize()
        {
            GamePort.renderSurface = new RenderTarget2D(GraphicsDevice, 640/*480*/, 480/*360*/);

            graphics.PreferredBackBufferWidth = 640;
            graphics.PreferredBackBufferHeight = 480;

            graphics.IsFullScreen = false;

            graphics.ApplyChanges();

            Window.ClientSizeChanged += (sender, args) => GamePort.KeepAspectRatio(Window);
            GamePort.KeepAspectRatio(Window);

            player = new Player();

            base.Initialize();
        }
        #endregion

        #region LoadContent
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            spriteTest = Content.Load<Texture2D>("Sprites/sprite_test");

        }
        #endregion

        #region Update
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.Move();

            base.Update(gameTime);
        }
        #endregion

        #region Draw
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(GamePort.renderSurface);
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            spriteBatch.Draw(spriteTest, player.Bounds, Color.White);
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(GamePort.renderSurface, GamePort.renderArea, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
        #endregion
    }
}
