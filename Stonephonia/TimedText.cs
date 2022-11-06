using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Stonephonia.Effects;

namespace Stonephonia
{
    public class TimedText
    {
        private Fader mFader;
        private int mTimeLimit;
        private bool mInputReceived = false;
        public bool mTextComplete;
        public float mAlpha;

        public TimedText(Vector2 position, int timeLimit, string text, Color colour)
        {
            mTimeLimit = timeLimit;
            mFader = new Fader(ScreenManager.font, text, position, colour);
        }

        public void CheckInput(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (InputManager.SpecificInputDetected(key))
                {
                    mInputReceived = true;
                }
            }
        }

        public void PromptMove(Timer timer, params Keys[] keys)
        {
            CheckInput(keys);

            if (mInputReceived)
            {
                HideText(timer);
            }
            else if (timer.mCurrentTime > mTimeLimit && timer.mCurrentTime < mTimeLimit * 2 && !mInputReceived)
            {
                ShowText();
            }
            else if (timer.mCurrentTime > mTimeLimit * 2 && !mInputReceived)
            {
                FlashText();
            }
        }

        public void PromptPush(Timer timer, Pusher pusher)
        {
            if (pusher.mCurrentState == Pusher.State.push)
            {
                HideText(timer);
            }
            else if (timer.mCurrentTime > mTimeLimit && timer.mCurrentTime < mTimeLimit * 3)
            {
                ShowText();
            }
            else if (timer.mCurrentTime > mTimeLimit * 3 && pusher.mCurrentState != Pusher.State.push)
            {
                FlashText();
            }
        }

        private void ShowText()
        {
            mFader.SmoothFade(true, 0.03f);
        }

        private void FlashText()
        {
            mFader.Flash(0.04f);
        }

        private void HideText(Timer timer)
        {
            mFader.SmoothFade(false, 0.05f);
            if (mAlpha <= 0.0f)
            {
                mTextComplete = true;
                timer.Reset();
            }
        }

        public void Update(GameTime gameTime)
        {
            mAlpha = mFader.mAlpha;
            mFader.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            mFader.DrawString(spriteBatch, true);
        }

    }
}
