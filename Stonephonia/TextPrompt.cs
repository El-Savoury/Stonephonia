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
                new TextPrompt(new Vector2(0, 600), 3, "D-Pad to move", Colours.darkBlue),
                new TextPrompt(new Vector2(0, 600), 3, "Hold (A) to push", Colours.darkBlue)
            };
            return textPrompts;
        }

        public void CheckKeyInput(bool anyInput, params Keys[] keys)
        {
            if (anyInput)
            {
                if (InputManager.AnyKeyInputDetected()) { mInputReceived = true; }
            }

            foreach (Keys key in keys)
            {
                if (InputManager.SpecificInputDetected(key))
                {
                    mInputReceived = true;
                }
            }
        }

        public void CheckPadInput(bool anyInput, params Buttons[] buttons)
        {
            if (anyInput)
            {
                if (InputManager.AnyPadInputDetected(buttons)) { mInputReceived = true; }
            }

            foreach (Buttons button in buttons)
            {
                if (InputManager.SpecificInputDetected(button))
                {
                    mInputReceived = true;
                }
            }
        }

        public void PromptInput(bool anyInput, Timer timer, Buttons[] buttons, params Keys[] keys)
        {
            CheckKeyInput(anyInput, keys);
            CheckPadInput(anyInput, buttons);

            if (mInputReceived)
            {
                HideText(timer);
            }
            else if (timer.mCurrentTime > mTimeLimit && timer.mCurrentTime < mTimeLimit * 3 && !mInputReceived)
            {
                ShowText(true, 0.02f);
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
                ShowText(true, 0.02f);
            }
            else if (timer.mCurrentTime > mTimeLimit * 3 && pusher.mCurrentState != Pusher.State.push)
            {
                FlashText();
            }
        }

        public void ShowText(bool visible, float fadeAmount)
        {
            mFader.SmoothFade(visible, fadeAmount);
        }

        private void FlashText()
        {
            mFader.Flash(1.0f, 0.3f, 0.04f);
        }

        public void HideText(Timer timer)
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
