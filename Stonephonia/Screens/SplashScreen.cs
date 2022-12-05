using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Stonephonia.Effects;

namespace Stonephonia.Screens
{
    class SplashScreen : Screen
    {
        Timer mRoomTimer = new Timer();
        Random mRandom = new Random();
        Rectangle mbackground;
        Sprite mTitleSprite;
        Fader mBlackFader;
        Vector2 mTitlePosition;
        TextPrompt mPressSpacePrompt;
        int mTitleSpeed = 4;
        int mRandomTime;

        public override void LoadAssets()
        {
            mRandomTime = mRandom.Next(15, 40);
            mbackground = new Rectangle(0, 0, GamePort.renderSurface.Width, GamePort.renderSurface.Height);
            mBlackFader = new Fader(ScreenManager.blackSquare, Vector2.Zero, Color.White, 1.0f);
            mTitleSprite = new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/title_sheet"),
                new Point(90, 20), new Point(0, 0), new Point(4, 1), 100, Color.White);
            mTitlePosition = new Vector2((GamePort.renderSurface.Width / 2) - (mTitleSprite.mFrameSize.X * 3),
                                        (GamePort.renderSurface.Height / 2) - (mTitleSprite.mFrameSize.Y * 6));
            mPressSpacePrompt = new TextPrompt(new Vector2(0, 600), 0, "Press space", Colours.lightBlue);

            base.LoadAssets();
        }

        public override void UnloadAssests()
        {
        }

        private void StartGame()
        {
            if (mPressSpacePrompt.mInputReceived)
            {
                mPressSpacePrompt.mFader.mAlpha = 0.0f;
                if (mTitleSprite.mAlpha > 0.0f) { mTitleSprite.mAlpha -= 0.03f; }
                else { ScreenManager.ChangeScreen(new SplashScreen(), new IntroCutscene()); }
            }
        }

        private void FlashTitle(GameTime gameTime)
        {
            if (mRoomTimer.mCurrentTime > mRandomTime)
            {
                if (!mTitleSprite.mAnimationComplete) { mTitleSprite.Update(gameTime, false); }
                else if (mTitleSprite.mAnimationComplete)
                {
                    mTitleSprite.ResetAnimation(new Point(0, 0));
                    mRandomTime = mRandom.Next(15, 40);
                    mRoomTimer.Reset();
                }
            }
        }

        //private void MoveTitle()
        //{
        //    if (mRoomTimer.mCurrentTime > 0.2f)
        //    {
        //        if (mTitlePosition.Y >= 240 ||
        //            mTitlePosition.Y <= 225)
        //        {
        //            mTitleSpeed *= -1;
        //        }
        //        mTitlePosition.Y += mTitleSpeed;
        //        mRoomTimer.Reset();
        //    }
        //}

        public override void Update(GameTime gameTime)
        {
            mBlackFader.SmoothFade(false, 0.03f );
            mRoomTimer.Update(gameTime);
            mPressSpacePrompt.PromptInput(mRoomTimer, Keys.Space);
            mPressSpacePrompt.Update(gameTime);

            //if (mRoomTimer.mCurrentTime < 0.5)
            //{
            //    mPressSpaceFader.mAlpha = 1;
            //}
            //else if (mRoomTimer.mCurrentTime < 1)
            //{
            //    mPressSpaceFader.mAlpha = 0;
            //}
            //else
            //{
            //    mRoomTimer.Reset();
            //}


            // MoveTitle();
            FlashTitle(gameTime);
            StartGame();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ScreenManager.pixel, mbackground, Color.Black);
            mPressSpacePrompt.Draw(spriteBatch);
            mTitleSprite.DrawScaled(spriteBatch, mTitlePosition, 6.0f);
            mBlackFader.DrawSprite(spriteBatch);
        }
    }
}
