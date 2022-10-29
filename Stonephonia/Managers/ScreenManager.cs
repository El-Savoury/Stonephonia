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
        public static Texture2D pixel;

        public static Pusher pusher;
        public static Rock[] rock;

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
            IsMouseVisible = true;
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

            particleManager = new ParticleManager();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            contentMgr = Content;
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("Font");

            // Create 1x1 white pixel texture
            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new Color[] { Color.White });

            pusher = new Pusher(new Vector2(300, 450), 0, 4)
            {
                mSprite = new Sprite(contentMgr.Load<Texture2D>("Sprites/player_stage_one_sheet"),
                new Point(60, 84), new Point(0, 0), new Point(2, 1), 200, Color.White),
            };

            rock = new Rock[4];
            rock[0] = new Rock(new Vector2(150, 450), 16, 3, 0.03f)
            {
                mSprite = new Sprite(contentMgr.Load<Texture2D>("Sprites/rock_zero_sheet"),
                new Point(92, 84), new Point(0, 0), new Point(2, 1), 200, Color.White),
                mSoundInterval = 4
            };

            rock[1] = new Rock(new Vector2(300, 450), 15, 3, 0.02f)
            {
                mSprite = new Sprite(pixel, new Point(48, 84), new Point(0, 0), new Point(1, 1), 15, Color.Pink),
            };

            rock[2] = new Rock(new Vector2(400, 450), 0, 2, 0.008f)
            {
                mSprite = new Sprite(pixel, new Point(64, 84), new Point(0, 0), new Point(1, 1), 15, Color.Orange),
            };

            rock[3] = new Rock(new Vector2(500, 450), 0, 1, 0.005f)
            {
                mSprite = new Sprite(pixel, new Point(96, 84), new Point(0, 0), new Point(1, 1), 15, Color.LightBlue),
            };

            particleManager.LoadAssets();

            AddScreen(new GampelayScreen());
            //AddScreen(new IntroCutscene());
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
            InputManager.Update(gameTime);
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            int startIndex = screenList.Count - 1;
            for (int i = startIndex; i < screenList.Count; i++)
            {
                screenList[i].Draw(gameTime, spriteBatch);
            }
            particleManager.Draw(spriteBatch);
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
                AddScreen(new GampelayScreen());
            }
        }

        public static void ChangeScreen(Screen currentScreen, Screen nextScreen)
        {
            RemoveScreen(currentScreen);
            AddScreen(nextScreen);
        }

        private void UserControlWindow()
        {
            ToggleFullScreen();

            if (InputManager.KeyPressed(Keys.Escape))
            {
                Exit();
            }
        }

        private void ToggleFullScreen()
        {
            if (InputManager.KeyHeld(Keys.LeftAlt) && InputManager.KeyHeld(Keys.Enter))
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
