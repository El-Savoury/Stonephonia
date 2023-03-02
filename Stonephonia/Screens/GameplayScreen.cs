using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Stonephonia.Managers;

namespace Stonephonia.Screens
{
    public class GameplayScreen : Screen
    {
        Timer mRoomTimer;
        LeafManager mLeafManager;
        ScreenTransition mScreenTransition;
        Texture2D[] mBackgroundTextures, mForegroundTextures, mTreeTextures;
        Texture2D mTreeTexture;
        TextPrompt[] mTextPrompts;
        TextPromptManager mTextPromptManager;
        SoundManager.SFXType[] mAgeSounds;
        public static Rock[] mRocks;
        float mTextureAlpha = 1.0f;
        bool mInputDetected = false;
        int mRoomEndTime = 15;
        int mCounter;

        int mPlayerRockLayer = 0;

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
            mTreeTexture = ScreenManager.contentMgr.Load<Texture2D>("Sprites/background_trees");

            mAgeSounds = new SoundManager.SFXType[]
            {
                SoundManager.SFXType.ageSquare,
                SoundManager.SFXType.ageVamp,
                SoundManager.SFXType.agePlinks,
                SoundManager.SFXType.ageBass
            };

            mTreeTextures = new Texture2D[]
            {
                ScreenManager.contentMgr.Load<Texture2D>("Sprites/trees/trees_stage_2"),
                ScreenManager.contentMgr.Load<Texture2D>("Sprites/trees/trees_stage_3"),
                ScreenManager.contentMgr.Load<Texture2D>("Sprites/trees/trees_stage_4")
            };

            mBackgroundTextures = new Texture2D[]
            {
              mTreeTexture,
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
                if (!mInputDetected)
                {
                    SoundManager.StopAmbientTrack();
                    ScreenManager.ChangeScreen(this, new SplashScreen());
                    ScreenManager.pusher.Reset();
                }
                else if (WinConditionMet())
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

        private void CheckInput()
        {
            mCounter++;
            if (mCounter > 600)
            {
                mInputDetected = false;
            }

            if (InputManager.AnyKeyInputDetected() || InputManager.AnyPadInputDetected())
            {
                mInputDetected = true;
                mCounter = 0;
            }
           
        }

        private void UpdateBackground(Texture2D[] treeTextures)
        {
            if (mRoomTimer.mCurrentTime >= 14.99 && Array.IndexOf(treeTextures, mTreeTexture) < mTreeTextures.Length - 1)
            {
                mTreeTexture = treeTextures[Array.IndexOf(treeTextures, mTreeTexture) + 1];
                mBackgroundTextures[0] = mTreeTexture;
                SoundManager.PlaySFX(mAgeSounds[Array.IndexOf(treeTextures, mTreeTexture)], 1.0f);
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

        public bool CheckRockOcclusion(float x, int rockIndex)
        {
            for (int i = rockIndex + 1; i < mRocks.Length; i++)
            {
                float rockLeft = mRocks[i].mPosition.X;
                float rockRight = mRocks[i].mPosition.X + mRocks[i].mSprite.mFrameSize.X;

                if (x > rockLeft && x < rockRight) { return true; }
            }

            return false;
        }

        public override void Update(GameTime gameTime)
        {
            mRoomTimer.Update(gameTime);
            InputManager.Update(gameTime);
            CheckInput();

            foreach (Rock rock in mRocks)
            {
                rock.Update(gameTime, ScreenManager.pusher);
            }

            ScreenManager.pusher.Update(gameTime, mRocks, mRoomTimer, this, 0, 0);
            mLeafManager.Update(gameTime, ScreenManager.pusher);
            mTextPromptManager.Update(gameTime);

            UpdateBackground(mTreeTextures);
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
