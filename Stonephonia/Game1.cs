﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Stonephonia
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;

        public static Game1 self;

        public Game1()
        {
            self = this;
            graphics = new GraphicsDeviceManager(this);
        }
        
        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
