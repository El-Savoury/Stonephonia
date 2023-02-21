using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stonephonia.Screens
{
    public class WinScreen : Screen
    {
        public override void LoadAssets()
        {
        }

        public override void UnloadAssests()
        {
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(ScreenManager.font, "YOU WIN", new Vector2(300, 300), Color.White);
        }
    }
}
