using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Stonephonia.Effects;

namespace Stonephonia.Screens
{
    public class TestScreen : GameScreen
    {
        //Pusher pusher;
        //Rock[] rock;
        //TextFader textFader;

        public override void LoadAssets()
        {
            
        }

        public override void UnloadAssests()
        {
        }

        public override void Update(GameTime gameTime)
        {
            //textFader.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds);
            //pusher.Update(gameTime, rock);
            ScreenManager.pusher.Update(gameTime, ScreenManager.rock);

            if (InputManager.KeyPressed(Keys.Back))
            {
                ScreenManager.ChangeScreen(new TestScreen(), new ScreenStage01());
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //textFader.Draw(spriteBatch, new Vector2(10, 65), 6, true, Color.White);

            foreach (Rock rock in ScreenManager.rock)
            {
                rock.Draw(spriteBatch);
                //spriteBatch.Draw(ScreenManager.pixel, rock.mCollisionRect, Color.Blue * 0.5f);
            }

            ScreenManager.pusher.Draw(spriteBatch);
            ScreenManager.pusher.Draw(gameTime, spriteBatch); // debug stats

            spriteBatch.DrawString(ScreenManager.font, $"mCurrentRock: {Array.IndexOf(ScreenManager.rock, ScreenManager.pusher.mCurrentRock)}", new Vector2(600, 0), Color.Red);
            //spriteBatch.DrawString(ScreenManager.font, $"mVelocity: {player.mVelocity}", new Vector2(0, 15), Color.White);
            //spriteBatch.DrawString(ScreenManager.font, $"mPushVelocity: {player.mPushVelocity}", new Vector2(0, 30), Color.Red);
        }

    }
}
