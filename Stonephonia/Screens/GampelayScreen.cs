using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stonephonia.Effects;

namespace Stonephonia.Screens
{
    public class GampelayScreen : Screen
    {
        Timer mRoomTimer;
        Fader mFader;
        string[] tutorialText;
        Texture2D[] backgroundTextures;

        public override void LoadAssets()
        {
            backgroundTextures = new Texture2D[]
            {
                ScreenManager.contentMgr.Load<Texture2D>("Sprites/background_trees"),
                ScreenManager.contentMgr.Load<Texture2D>("Sprites/background_bushes"),
            };

            mRoomTimer = new Timer();
            tutorialText = new string[2] { "Arrow keys to move", "Hold space to push" };
            mFader = new Fader(ScreenManager.font, tutorialText[0], new Vector2(0, 600), 0.0f);
        }

        public override void UnloadAssests()
        {
        }

        private void TutorialTextPrompt()
        {
            if (mRoomTimer.mCurrentTime > 5.0f)
            {
                mFader.SmoothFade(true, 0.03f);
            }
        }

        public override void Update(GameTime gameTime)
        {
            TutorialTextPrompt();

            mRoomTimer.Update(gameTime);
            mFader.Update(gameTime);

            foreach (Rock rock in ScreenManager.rock)
            {
                rock.Update(gameTime, ScreenManager.pusher);
            }

            ScreenManager.pusher.Update(gameTime, mRoomTimer, ScreenManager.rock);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < backgroundTextures.Length; i++)
            {
                spriteBatch.Draw(backgroundTextures[i], new Rectangle(0, 0, 800, 800), Color.White);
            }
            
            foreach (Rock rock in ScreenManager.rock)
            {
                rock.Draw(spriteBatch);
            }

            ScreenManager.pusher.Draw(spriteBatch);
            mFader.DrawString(spriteBatch, true);

            // Debug Stats
            //ScreenManager.pusher.DrawDebug(gameTime, spriteBatch);
            // spriteBatch.Draw(ScreenManager.pixel, rock.mCollisionRect, Color.Blue * 0.5f);
            // ScreenManager.pusher.Draw(gameTime, spriteBatch); 
            spriteBatch.DrawString(ScreenManager.font, $"Timer: {mRoomTimer.mCurrentTime}", new Vector2(600, 0), Color.Red);
            //spriteBatch.DrawString(ScreenManager.font, $"mCurrentRock: {Array.IndexOf(ScreenManager.rock, ScreenManager.pusher.mCurrentRock)}", new Vector2(600, 0), Color.Red);
            // spriteBatch.DrawString(ScreenManager.font, $"mVelocity: {player.mVelocity}", new Vector2(0, 15), Color.White);
            // spriteBatch.DrawString(ScreenManager.font, $"mPushVelocity: {player.mPushVelocity}", new Vector2(0, 30), Color.Red);
        }

    }
}
