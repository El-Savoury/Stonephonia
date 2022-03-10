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
        public static SpriteBatch sprites;
        //public static Dictionary<string, Texture2D> Textures2D;
        //public static Dictionary<string, SpriteFont> Fonts;
        public static List<GameScreen> screenList;
        public static ContentManager contentMgr;

        public static void Main()
        {
            using ScreenManager manager = new ScreenManager();
            manager.Run();
        }

        public ScreenManager()
        {
            graphicsDeviceMgr = new GraphicsDeviceManager(this);

            this.TargetElapsedTime = new TimeSpan(333333);
            Window.AllowUserResizing = true;
            IsMouseVisible = true;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            GamePort.renderSurface = new RenderTarget2D(GraphicsDevice, 240, 180);

            graphicsDeviceMgr.PreferredBackBufferWidth = 480;
            graphicsDeviceMgr.PreferredBackBufferHeight = 360;

            graphicsDeviceMgr.IsFullScreen = false;

            graphicsDeviceMgr.ApplyChanges();

            Window.ClientSizeChanged += (sender, args) => GamePort.KeepAspectRatio(Window);
            GamePort.KeepAspectRatio(Window);

            //Textures2D = new Dictionary<string, Texture2D>();
            //Fonts = new Dictionary<string, SpriteFont>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            contentMgr = Content;
            sprites = new SpriteBatch(GraphicsDevice);

            // TODO: Load global game assests here

            AddScreen(new TestScreen());
        }

        protected override void UnloadContent()
        {
            foreach (var screen in screenList)
            {
                screen.UnloadAssests();
            }

            //Textures2D.Clear();
            //Fonts.Clear();
            screenList.Clear();

            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            // TODO: Remove temp code & add input manager
            if (Keyboard.GetState().IsKeyDown(Keys.Back)) { Exit(); }

            int startIndex = screenList.Count - 1;

            while (screenList[startIndex].isActive)
            {
                startIndex--;
            }

            for (int i = startIndex; i < screenList.Count; i++)
            {
                screenList[i].Update(gameTime);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            int startIndex = screenList.Count - 1;

            while (screenList[startIndex].isActive)
            {
                startIndex--;
            }

            for (int i = startIndex; i < screenList.Count; i++)
            {
                screenList[i].Draw(gameTime);
            }

            base.Draw(gameTime);
        }

        public static void AddScreen(GameScreen gameScreen)
        {
            if (screenList == null)
            {
                screenList = new List<GameScreen>();
            }

            screenList.Add(gameScreen);
            gameScreen.LoadAssets();
        }

        public static void RemoveScreen(GameScreen gameScreen)
        {
            gameScreen.UnloadAssests();
            screenList.Remove(gameScreen);
            
            if (screenList.Count < 1)
            {
                AddScreen(new TestScreen());
            }
        }

        public static void ChangeScreen(GameScreen currentScreen, GameScreen nextScreen)
        {
            RemoveScreen(currentScreen);
            AddScreen(nextScreen);
        } 
    }
}
