using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stonephonia.Managers;

namespace Stonephonia.Screens
{
    public class GameplayScreen : Screen
    {
        Timer mRoomTimer;
        LeafManager mLeafManager;
        Texture2D[] mBackgroundTextures, mForegroundTextures;
        Reflection[] mReflections;
        TextPrompt[] mTextPrompts;
        TextPromptManager mTextPromptManager;
        Rock[] mRocks;

        public override void LoadAssets()
        {
            mRocks = Rock.Load();
            mTextPrompts = TextPrompt.Load();
            mReflections = Reflection.Load();
            mRoomTimer = new Timer();
            mLeafManager = new LeafManager();
            mTextPromptManager = new TextPromptManager(mTextPrompts);

            mBackgroundTextures = new Texture2D[]
            {
                ScreenManager.contentMgr.Load<Texture2D>("Sprites/background_trees"),
                ScreenManager.contentMgr.Load<Texture2D>("Sprites/background_bushes"),
            };

            mForegroundTextures = new Texture2D[] { ScreenManager.contentMgr.Load<Texture2D>("Sprites/canopy") };
        }

        public override void UnloadAssests()
        {
        }

        public override void Update(GameTime gameTime)
        {
            //InputManager.NoInputTimeOut(gameTime, 10, new GameplayScreen(), new SplashScreen());

            mRoomTimer.Update(gameTime);

            foreach (Reflection reflection in mReflections)
            {
                reflection.Update(gameTime);
            }

            foreach (Rock rock in mRocks)
            {
                rock.Update(gameTime, ScreenManager.pusher);
            }

            ScreenManager.pusher.Update(gameTime, mRoomTimer, mRocks);
            mLeafManager.Update(gameTime, ScreenManager.pusher);
            mTextPromptManager.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Texture2D background in mBackgroundTextures)
            {
                spriteBatch.Draw(background, new Rectangle(0, 0, 800, 800), Color.White);
            }

            foreach (Reflection reflection in mReflections)
            {
                //reflection.Draw(spriteBatch);
            }

            foreach (Rock rock in mRocks)
            {
                rock.Draw(spriteBatch);
            }

            ScreenManager.pusher.Draw(spriteBatch);
            mLeafManager.Draw(spriteBatch);

            foreach (Texture2D foreground in mForegroundTextures)
            {
                spriteBatch.Draw(foreground, new Rectangle(0, 0, 800, 800), Color.White);
            }

            mTextPromptManager.Draw(spriteBatch);

            // Debug Stats
            //spriteBatch.DrawString(ScreenManager.font, $"Timer: {mRoomTimer.mCurrentTime}", new Vector2(600, 0), Color.Red);
            spriteBatch.DrawString(ScreenManager.font, $"mVelocity: {ScreenManager.pusher.mVelocity}", new Vector2(0, 15), Color.White);

            //ScreenManager.pusher.DrawDebug(gameTime, spriteBatch);
            // spriteBatch.Draw(ScreenManager.pixel, rock.mCollisionRect, Color.Blue * 0.5f);
            // ScreenManager.pusher.Draw(gameTime, spriteBatch); 
            //spriteBatch.DrawString(ScreenManager.font, $"mCurrentRock: {Array.IndexOf(ScreenManager.rock, ScreenManager.pusher.mCurrentRock)}", new Vector2(600, 0), Color.Red);
            // spriteBatch.DrawString(ScreenManager.font, $"mPushVelocity: {player.mPushVelocity}", new Vector2(0, 30), Color.Red);
        }

    }
}
