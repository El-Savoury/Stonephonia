using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Stonephonia.Managers;

namespace Stonephonia.Screens
{
    public class GameplayScreen : Screen
    {
        Timer mRoomTimer;
        LeafManager mLeafManager;
        Texture2D[] mBackgroundTextures, mForegroundTextures;
        TextPrompt[] mTextPrompts;
        TextPromptManager mTextPromptManager;
        Rock[] mRocks;
        ScreenTransition mScreenTransition;
        float mTextureAlpha = 1.0f;
        bool mInputDetected = false;

        public GameplayScreen()
        {
            OnActivate();
        }

        public override void LoadAssets()
        {
            ScreenManager.pusher.Load(ScreenManager.contentMgr);
            mRocks = Rock.Load();
            mTextPrompts = TextPrompt.Load();
            mRoomTimer = new Timer();
            mLeafManager = new LeafManager();
            mTextPromptManager = new TextPromptManager(mTextPrompts);
            mScreenTransition = new ScreenTransition();

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

        private void OnActivate()
        {
            SoundManager.PlayMusic(SoundManager.MusicType.AmbientTrack, 0.5f);
        }

        private bool WinConditionMet()
        {
            if (mRocks[3].mPosition.X < mRocks[2].mPosition.X &&
                mRocks[2].mPosition.X < mRocks[1].mPosition.X &&
                mRocks[1].mPosition.X < mRocks[0].mPosition.X)
            {
                return true;
            }
            else { return false; }
        }

        private void ChangeScreen(GameTime gameTime, float timeLimit)
        {
            if (mRoomTimer.mCurrentTime > timeLimit)
            {
                //if (!mInputDetected) { ScreenManager.ChangeScreen(new GameplayScreen(), new SplashScreen()); }
                if (WinConditionMet()) { ScreenManager.ChangeScreen(new GameplayScreen(), new WinScreen()); }
                else if (!WinConditionMet()) { GotoLoseScreen(gameTime, ScreenManager.pusher, timeLimit + 3); }
            }
        }

        private void GotoLoseScreen(GameTime gameTime, Pusher pusher, float timeLimit)
        {
            float fadeIn = 0.008f;
            float fadeOut = 0.008f;

            pusher.KillPlayer(gameTime);

            if (mRoomTimer.mCurrentTime > timeLimit)
            {
                mScreenTransition.FadeToNextScreen(fadeIn, fadeOut, new GameplayScreen(), new LoseScreen());
                FadeOutAssets(pusher, fadeOut);
            }
        }

        private void FadeOutAssets(Pusher pusher, float fadeAmount)
        {
            mTextureAlpha -= fadeAmount;
            mLeafManager.FadeOutLeaves(fadeAmount);
            pusher.FadeOutReflection(fadeAmount);
            //mTextPromptManager.
        }

        public override void Update(GameTime gameTime)
        {
            mRoomTimer.Update(gameTime);

            foreach (Rock rock in mRocks)
            {
                rock.Update(gameTime, ScreenManager.pusher);
            }

            ScreenManager.pusher.Update(gameTime, mRoomTimer, mRocks);
            mLeafManager.Update(gameTime, ScreenManager.pusher);
            mTextPromptManager.Update(gameTime);

            ChangeScreen(gameTime, 10);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Texture2D background in mBackgroundTextures)
            {
                spriteBatch.Draw(background, new Rectangle(0, 0, 800, 800), Color.White);
            }

            foreach (Rock rock in mRocks)
            {
                rock.Draw(spriteBatch);
            }

            mScreenTransition.Draw(spriteBatch);
            ScreenManager.pusher.Draw(spriteBatch);
            mLeafManager.Draw(spriteBatch);

            foreach (Texture2D foreground in mForegroundTextures)
            {
                spriteBatch.Draw(foreground, new Rectangle(0, 0, 800, 800), Color.White * mTextureAlpha);
            }

            //ScreenManager.pusher.DrawDebug(gameTime, spriteBatch);

            mTextPromptManager.Draw(spriteBatch);

            spriteBatch.DrawString(ScreenManager.font, $"mRoomTimer: {mRoomTimer.mCurrentTime}", new Vector2(0, 15), Color.White);
            //spriteBatch.DrawString(ScreenManager.font, $"input: {mInputDetected}", new Vector2(300, 300), Color.White);
        }

    }
}
