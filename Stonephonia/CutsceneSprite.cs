using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Stonephonia.Effects;
using Stonephonia.Managers;

namespace Stonephonia
{
    public class CutsceneSprite
    {
        public Sprite mSprite;
        private Fader mMask;
        private FaderManager mMaskManager;
        float mFade1 = 0.04f;
        float mFade2 = 0.03f;
        int mStartTime, mStopTime;
        int mDelay = 2;
        private Vector2 mPosition;
        Timer mTimer = new Timer();

        public enum State
        {
            inactive,
            activated,
            deactivated,
        }

        public State mCurrentState;
        
        public CutsceneSprite(Vector2 postion, int startTime, int StopTime, State state, Sprite sprite, Fader mask)
        {
            mPosition = postion;
            mStartTime = startTime;
            mStopTime = StopTime;
            mCurrentState = state;
            mSprite = sprite;
            mMask = mask;
            mMaskManager = new FaderManager(new Fader[1] { mMask });
        }

        public void FadeMaskIn()
        {
            mMaskManager.FadeInAndOut(mMask, mFade1, mFade2, mStartTime, mDelay);
            if (mMask.mAlpha >= 1.0f) { mSprite.SetVisible(true); }
            else if (mMask.mAlpha <= 0.0f && mSprite.mAlpha >= 1.0f) { mCurrentState = State.activated; }
        }

        public void FadeMaskOut()
        {
            mMaskManager.FadeInAndOut(mMask, mFade2, mFade1, mStopTime, mDelay);
            if (mMask.mAlpha >= 1.0f) { mSprite.SetVisible(false); }
        }

        public void Update(GameTime gameTime, bool loop)
        {
            mTimer.Update(gameTime);
            mMaskManager.Update(gameTime);

            if (mCurrentState == State.inactive)
            {
                FadeMaskIn();
            }
            else if (mCurrentState == State.activated)
            {
                if (mTimer.mCurrentTime > mStopTime) { mCurrentState = State.deactivated; }
                mSprite.Update(gameTime, loop);
            }
            else if (mCurrentState == State.deactivated)
            {
                FadeMaskOut();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            mSprite.Draw(spriteBatch, mPosition);
            mMask.DrawSprite(spriteBatch);
        }
    }
}
