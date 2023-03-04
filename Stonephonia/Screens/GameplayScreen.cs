using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        float mRoomEndTime = ScreenManager.pusher.mAgeTime;
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
                else if (!WinConditionMet())
                {
                    mPlayerRockLayer = 3;
                    pusher.mCurrentState = Pusher.State.dead;
                    pusher.mMaxSpeed = 0;
                    ScreenTransition(new WinScreen(), timeLimit, pusher);
                }
                else if (!WinConditionMet())
                {
                    mPlayerRockLayer = 3;
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
            if (mRoomTimer.mCurrentTime >= mRoomEndTime - 0.01f && Array.IndexOf(treeTextures, mTreeTexture) < mTreeTextures.Length - 1)
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
                float rockLeft = mRocks[i].mCollisionRect.Left;
                float rockRight = mRocks[i].mCollisionRect.Right;

                if (x > rockLeft && x < rockRight) { return true; }
            }

            return false;
        }

        private void SetDepth(GameTime gameTime, Pusher pusher, Rock[] rocks)
        {
            if (pusher.HoldingRock()) { mPlayerRockLayer = pusher.ReturnRockIndex(rocks); }
            else { TryPopPlayerLayerUp(pusher, rocks); }
        }

        private bool CheckRockIntersectsPlayer(Pusher pusher, Rock rock)
        {
            float rockLeft = rock.mCollisionRect.Left;
            float rockRight = rock.mCollisionRect.Right;
            float pusherLeft = pusher.mPosition.X;
            float pusherRight = pusher.mPosition.X + pusher.mSprite.mFrameSize.X;

            if ((rockLeft < pusherRight && pusherRight < rockRight) ||
                (rockLeft < pusherLeft && pusherLeft < rockRight) ||
                (pusherLeft < rockRight && rockRight < pusherRight) ||
                (pusherLeft < rockLeft && rockLeft < pusherRight))
            {
                return true;
            }
            return false;
        }

        private void TryPopPlayerLayerUp(Pusher pusher, Rock[] rocks)
        {
            // Start from the heighest rock and go down the layers.
            for (int i = rocks.Length - 1; i > mPlayerRockLayer; i--)
            {
                if (!CheckRockIntersectsPlayer(pusher, rocks[i]))
                {
                    // We can pop the player layer up
                    mPlayerRockLayer = i;
                    break; // Found highest layer for player.
                }
            }
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

            ScreenManager.pusher.Update(gameTime, mRocks, mRoomTimer, this);
            mLeafManager.Update(gameTime, ScreenManager.pusher);
            mTextPromptManager.Update(gameTime);
            SetDepth(gameTime, ScreenManager.pusher, mRocks);
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

            //Draw rocks and player

            for (int i = 0; i < mRocks.Length; i++)
            {
                mRocks[i].Draw(spriteBatch);
                mRocks[i].DrawReflection(spriteBatch);

            

                if (i == mPlayerRockLayer)
                {
                    mScreenTransition.Draw(spriteBatch);
                    ScreenManager.pusher.Draw(spriteBatch);
                }
            }

           ;

            mLeafManager.Draw(spriteBatch);

            foreach (Texture2D foreground in mForegroundTextures)
            {
                spriteBatch.Draw(foreground, new Rectangle(0, 0, 800, 800), Color.White * mTextureAlpha);
            }

            //ScreenManager.pusher.DrawDebug(gameTime, spriteBatch);

            mTextPromptManager.Draw(spriteBatch);

            //spriteBatch.DrawString(ScreenManager.font, $"mRoomTimer: {mRoomTimer.mCurrentTime}", new Vector2(0, 15), Color.White);
            //spriteBatch.DrawString(ScreenManager.font, $"input: {mInputDetected}", new Vector2(300, 300), Color.White);
        }

    }
}
