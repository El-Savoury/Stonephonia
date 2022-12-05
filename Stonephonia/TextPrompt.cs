using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Stonephonia.Effects;

namespace Stonephonia
{
    public class TextPrompt
    {
        public Fader mFader;
        private int mTimeLimit;
        public bool mInputReceived = false;
        public bool mTextComplete;
        public float mAlpha;

        public TextPrompt(Vector2 position, int timeLimit, string text, Color colour)
        {
            mTimeLimit = timeLimit;
            mFader = new Fader(ScreenManager.font, text, position, colour);
        }

        public static TextPrompt[] Load()
        {
            TextPrompt[] textPrompts = new TextPrompt[]
            {
                new TextPrompt(new Vector2(0, 600), 3, "Arrow keys to move", Colours.darkBlue),
                new TextPrompt(new Vector2(0, 600), 3, "Hold space to push", Colours.darkBlue)
            };
            return textPrompts;
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

        public void PromptInput(Timer timer, params Keys[] keys)
        {
            CheckInput(keys);

            if (mInputReceived)
            {
                HideText(timer);
            }
            else if (timer.mCurrentTime > mTimeLimit && timer.mCurrentTime < mTimeLimit * 3 && !mInputReceived)
            {
                ShowText();
            }
            else if (timer.mCurrentTime > mTimeLimit * 3 && !mInputReceived)
            {
                FlashText();
            }
        }

        public void PromptAction(Timer timer, Pusher pusher)
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
            mFader.SmoothFade(true, 0.02f);
        }

        private void FlashText()
        {
            mFader.Flash(1.0f, 0.3f, 0.04f);
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
