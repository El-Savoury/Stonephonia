﻿using System;
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
        int mInterval;
        int mCounter = 0;

        Buttons[] mButtons = new Buttons[] {Buttons.A, Buttons.B, Buttons.X, Buttons.Y,
                                Buttons.DPadUp, Buttons.DPadDown, Buttons.DPadLeft, Buttons.DPadRight };

        SoundManager.SFXType[] mSounds = new SoundManager.SFXType[]
        {
            SoundManager.SFXType.ageVamp,
            SoundManager.SFXType.agePlinks,
            SoundManager.SFXType.ageSquare,
            SoundManager.SFXType.ageBass
        };

        public override void LoadAssets()
        {
            mInterval = mRandom.Next(15 * 30, 40 * 30);
            mbackground = new Rectangle(0, 0, GamePort.renderSurface.Width, GamePort.renderSurface.Height);
            mBlackFader = new Fader(ScreenManager.blackSquare, Vector2.Zero, Color.White, 1.0f);
            mTitleSprite = new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/title_sheet"),
                new Point(90, 20), new Point(0, 0), new Point(5, 1), 100, Color.White);
            mTitleSprite.mAnimationComplete = true;
            mTitlePosition = new Vector2((GamePort.renderSurface.Width / 2) - (mTitleSprite.mFrameSize.X * 3),
                                        (GamePort.renderSurface.Height / 2) - (mTitleSprite.mFrameSize.Y * 6));
            mPressSpacePrompt = new TextPrompt(new Vector2(0, 600), 0, "Press Any Button", Colours.lightBlue);

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
                else { ScreenManager.ChangeScreen(this, new IntroCutscene()); }
            }
        }

        private void FlashTitle()
        {
            mCounter++;

            if (mCounter > mInterval)
            {
                SoundManager.PlaySFX(mSounds[mRandom.Next(0, 4)], 1.0f);
                mTitleSprite.ResetAnimation(new Point(0, 0));
                mTitleSprite.mAnimationComplete = false;
                mCounter = 0;
                mInterval = mRandom.Next(15 * 30, 40 * 30);
            }
        }

        public override void Update(GameTime gameTime)
        {
            InputManager.Update(gameTime);
            mBlackFader.SmoothFade(false, 0.03f);
            mRoomTimer.Update(gameTime);
            mPressSpacePrompt.PromptInput(true, mRoomTimer, mButtons, Keys.Space);
            mPressSpacePrompt.Update(gameTime);
            mTitleSprite.Update(gameTime, false);
            FlashTitle();
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
