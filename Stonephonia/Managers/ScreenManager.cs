using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Stonephonia.Screens;

namespace Stonephonia
{
    class ScreenManager : Game
    {
        public static GraphicsDeviceManager graphicsDeviceMgr;
        public static SpriteBatch spriteBatch;
        public static List<Screen> screenList;
        public static ContentManager contentMgr;
        public static ParticleManager particleManager;
        public static SpriteFont font;
        public static Texture2D pixel, blackSquare;
        public static Pusher pusher;

        private readonly int windowWidth = 1280;
        private readonly int windowHeight = 720;
        private readonly int nativeResWidth = 800;
        private readonly int nativeResHeight = 720;

        public static void Main()
        {
            using ScreenManager manager = new ScreenManager();
            manager.Run();
        }

        public ScreenManager()
        {
            graphicsDeviceMgr = new GraphicsDeviceManager(this);
            this.TargetElapsedTime = new TimeSpan(333333);
            Window.AllowUserResizing = false;
            IsMouseVisible = false;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            GamePort.renderSurface = new RenderTarget2D(GraphicsDevice, nativeResWidth, nativeResHeight);

            graphicsDeviceMgr.PreferredBackBufferWidth = windowWidth;
            graphicsDeviceMgr.PreferredBackBufferHeight = windowHeight;
            graphicsDeviceMgr.IsFullScreen = false;
            graphicsDeviceMgr.ApplyChanges();

            Window.ClientSizeChanged += (sender, args) => GamePort.KeepAspectRatio(Window);
            GamePort.KeepAspectRatio(Window);
            graphicsDeviceMgr.ToggleFullScreen();

            particleManager = new ParticleManager();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            contentMgr = Content;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            SoundManager.LoadContent(Content);

            font = Content.Load<SpriteFont>("Font");
            blackSquare = contentMgr.Load<Texture2D>("Sprites/black_square");

            // Create 1x1 white pixel texture
            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new Color[] { Color.White });

            pusher = new Pusher(new Vector2(20, 452), 0, 4)
            {
                mSprite = new Sprite(Content.Load<Texture2D>("Sprites/player_stage_one"),
                new Point(60, 84), new Point(0, 0), new Point(2, 1), 200, Color.White, true),
            };

            particleManager.LoadAssets();

            // AddScreen(new GameplayScreen());
            // AddScreen(new IntroCutscene());
             AddScreen(new SplashScreen());
        }

        protected override void UnloadContent()
        {
            foreach (var screen in screenList)
            {
                screen.UnloadAssests();
            }
            screenList.Clear();
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            //InputManager.Update(gameTime);
            UserControlWindow();

            int startIndex = screenList.Count - 1;
            for (int i = startIndex; i < screenList.Count; i++)
            {
                screenList[i].Update(gameTime);
            }

            particleManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(GamePort.renderSurface);
            GraphicsDevice.Clear(Colours.lightBlue);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            particleManager.Draw(spriteBatch);
            int startIndex = screenList.Count - 1;
            for (int i = startIndex; i < screenList.Count; i++)
            {
                screenList[i].Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(GamePort.renderSurface, GamePort.renderArea, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public static void AddScreen(Screen gameScreen)
        {
            if (screenList == null)
            {
                screenList = new List<Screen>();
            }
            screenList.Add(gameScreen);
            gameScreen.LoadAssets();
        }

        public static void RemoveScreen(Screen gameScreen)
        {
            gameScreen.UnloadAssests();
            screenList.Remove(gameScreen);

            if (screenList.Count < 1)
            {
                AddScreen(new Screen());
            }
        }

        public static void ChangeScreen(Screen currentScreen, Screen nextScreen)
        {
            AddScreen(nextScreen);
            RemoveScreen(currentScreen);
        }

        private void UserControlWindow()
        {
            ToggleFullScreen();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
        }

        private void ToggleFullScreen()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.F11))
            {
                if (graphicsDeviceMgr.IsFullScreen)
                {
                    graphicsDeviceMgr.IsFullScreen = false;
                }
                else
                {
                    graphicsDeviceMgr.IsFullScreen = true;
                }
                graphicsDeviceMgr.ApplyChanges();
            }
        }
    }
}
