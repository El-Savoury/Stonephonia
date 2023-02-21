using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stonephonia.Managers;

namespace Stonephonia.Screens
{
    public class LoseScreen : Screen
    {
        Timer mRoomTimer;

        public override void LoadAssets()
        {
            mRoomTimer = new Timer();
        }

        public override void UnloadAssests()
        {
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ScreenManager.blackSquare, Vector2.Zero, Color.White);
            ScreenManager.pusher.Draw(spriteBatch);
        }
    }
}
