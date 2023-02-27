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
        public static Rock[] mRocks;
        ScreenTransition mScreenTransition;
        float mTextureAlpha = 1.0f;
        bool mInputDetected = false;
        int mRoomEndTime = 15;

        public GameplayScreen()
        {
            //OnActivate();
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

        private void ChangeScreen(GameTime gameTime, float timeLimit, Pusher pusher)
        {
            if (mRoomTimer.mCurrentTime > timeLimit)
            {
                SoundManager.StopMusic();
                //if (!mInputDetected) { ScreenManager.ChangeScreen(new GameplayScreen(), new SplashScreen()); }
                if (WinConditionMet())
                {
                    pusher.mCurrentState = Pusher.State.dead;
                    pusher.mMaxSpeed = 0;
                    ScreenTransition(new WinScreen(), timeLimit, pusher);
                }
                else if (!WinConditionMet())
                {
                    pusher.KillPlayer(gameTime);
                    ScreenTransition(new LoseScreen(), timeLimit + 3, pusher);
                }

                foreach (TextPrompt prompt in mTextPrompts)
                {
                    prompt.mFader.mAlpha -= 0.05f;
                }

            }
        }

        private void ScreenTransition(Screen nextScreen, float timeLimit, Pusher pusher)
        {
            float fadeIn = 0.008f;
            float fadeOut = 0.04f;

            if (mRoomTimer.mCurrentTime > timeLimit)
            {
                mScreenTransition.FadeToCutscene(fadeIn, fadeOut, this, nextScreen);
                FadeOutAssets(pusher, fadeIn);
            }
        }

        private void FadeOutAssets(Pusher pusher, float fadeAmount)
        {
            mTextureAlpha -= fadeAmount;
            mLeafManager.FadeOutLeaves(fadeAmount);
            pusher.FadeOutReflection(fadeAmount);
            mTextPromptManager.FadeOutPrompt(fadeAmount);
        }

        public override void Update(GameTime gameTime)
        {
            InputManager.Update(gameTime);
            mRoomTimer.Update(gameTime);

            foreach (Rock rock in mRocks)
            {
                rock.Update(gameTime, ScreenManager.pusher);
            }

            ScreenManager.pusher.Update(gameTime, mRoomTimer, mRocks);
            mLeafManager.Update(gameTime, ScreenManager.pusher);
            mTextPromptManager.Update(gameTime);

            ChangeScreen(gameTime, mRoomEndTime, ScreenManager.pusher);

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
                rock.DrawReflection(spriteBatch);
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
