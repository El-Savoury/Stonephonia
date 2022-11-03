using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Stonephonia.Effects;

namespace Stonephonia
{
    public class TimedTextPrompt
    {
        private Fader mFader;
        private Vector2 mPosition;
        private Timer mTimer;
        private int mTimeLimit;
        private bool mInputReceived;
        public string mText;

        public TimedTextPrompt(Vector2 position, Timer timer, int timeLimit, string text)
        {
            mPosition = position;
            mTimer = timer;
            mTimeLimit = timeLimit;
            mText = text;
            mFader = new Fader(ScreenManager.font, mText, position);
        }

        public bool CheckInput(params Keys[] keys)
        {
            bool inputReceived = false;

            foreach (Keys key in keys)
            {
                if (InputManager.SpecificInputDetected(key))
                {
                    mInputReceived = true;
                    inputReceived = true;
                }
                else { inputReceived = false; }
            }
            return inputReceived;
        }

        public void TextPromptUserInput(params Keys[] keys)
        {
            CheckInput(keys);

            if (mTimer.mCurrentTime > mTimeLimit && !mInputReceived)
            {
                ShowPrompt(true, 0.03f);
            }
            else
            {
                ShowPrompt(false, 0.05f);
            }
        }

        private void ShowPrompt(bool visible, float fadeInSpeed)
        {
            mFader.SmoothFade(visible, fadeInSpeed);
        }

        public void Update(GameTime gameTime)
        {
            mFader.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            mFader.DrawString(spriteBatch, true);
        }

    }
}
