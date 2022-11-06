using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Stonephonia.Effects;

namespace Stonephonia
{
    public class TimedTextPrompt
    {
        private Fader mFader;
        private Timer mTimer = new Timer();
        private int mTimeLimit;
        private bool mInputReceived;
        public bool mPromptComplete;
        public string mText;
        public float mAlpha;

        public TimedTextPrompt(Vector2 position, int timeLimit, string text)
        {
            mTimeLimit = timeLimit;
            mText = text;
            mFader = new Fader(ScreenManager.font, mText, position, ScreenManager.darkBlue);
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

        public void PromptMove(params Keys[] keys)
        {
            if (!mPromptComplete)
            {
                CheckInput(keys);

                if (mInputReceived)
                {
                    mPromptComplete = true;
                }
                else if (mTimer.mCurrentTime > mTimeLimit && mTimer.mCurrentTime < mTimeLimit * 3 && !mInputReceived)
                {
                    ShowPrompt();
                }
                else if (mTimer.mCurrentTime > mTimeLimit * 3 && !mInputReceived)
                {
                    FlashPrompt();
                }
            }
            else
            {
                HidePrompt();
            }
        }

        public void PromptPush(Pusher pusher)
        {
            if (!mPromptComplete)
            {
                if (pusher.mCurrentState == Pusher.State.push)
                {
                    mPromptComplete = true;
                }
                else if (mTimer.mCurrentTime > mTimeLimit && mTimer.mCurrentTime < mTimeLimit * 3)
                {
                    ShowPrompt();
                }
                else if (mTimer.mCurrentTime > mTimeLimit * 3 && pusher.mCurrentState != Pusher.State.push)
                {
                    FlashPrompt();
                }
            }
            else
            {
                HidePrompt();
            }
        }

        private void ShowPrompt()
        {
            mFader.SmoothFade(true, 0.03f);
        }

        private void FlashPrompt()
        {
            mFader.Flash(0.04f);
        }

        private void HidePrompt()
        {
            mFader.SmoothFade(false, 0.05f);
            if (mAlpha <= 0.0f)
            {
                mTimer.Reset();
            }
        }

        public void Update(GameTime gameTime)
        {
            mAlpha = mFader.mAlpha;
            mTimer.Update(gameTime);
            mFader.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            mFader.DrawString(spriteBatch, true);
        }

    }
}
