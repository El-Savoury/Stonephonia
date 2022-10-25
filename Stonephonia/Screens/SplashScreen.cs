using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stonephonia.Effects;

namespace Stonephonia.Screens
{
    class SplashScreen : Screen
    {
        AnimatedTextFader textFader;

        public override void LoadAssets()
        {
            textFader = new AnimatedTextFader(ScreenManager.font, "Please turn sound on", 1.0f, 0.3f, 0.0f);

            base.LoadAssets();
        }
        public override void UnloadAssests()
        {
        }

        public override void Update(GameTime gameTime)
        {
            textFader.Update((float) gameTime.ElapsedGameTime.TotalMilliseconds);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            textFader.Draw(spriteBatch, new Vector2(0, 70), 6, true, Color.White);
        }
    }
}
