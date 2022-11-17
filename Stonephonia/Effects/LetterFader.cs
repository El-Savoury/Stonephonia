using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stonephonia.Effects
{
    public class LetterFader
    {
        public char mLetter;
        private bool mEnabled;
        private float mFadeSpeed;
        public float mAlpha;
        private Color[] mColour;
        private Timer mTimer;
        private int mColourIndex = 0;

        public LetterFader(bool enabled, char letter, float fadeSpeed, float textOpacity)
        {
            mEnabled = enabled;
            mLetter = letter;
            mFadeSpeed = fadeSpeed;
            mAlpha = textOpacity;
            mTimer = new Timer();

            mColour = new Color[] {Color.Black, ScreenManager.darkBlue, ScreenManager.greenBlue, ScreenManager.greyBlue, ScreenManager.lightBlue };
        }

        public void SetEnabled(bool enabled)
        {
            if (enabled)
            {
                mEnabled = true;
            }
            else
            {
                mEnabled = false;
            }
        }

        public void Update(GameTime gameTime, float elapsedTime)
        {
            mTimer.Update(gameTime);

            if (mEnabled)
            {
                //if (mTimer.mCurrentTime > 0.04f && mColourIndex < mColour.Length - 1)
                //{
                //    mColourIndex++;
                //    mTimer.Reset();
                //}

                mAlpha += elapsedTime / 1000.0f * mFadeSpeed;
            }
            else
            {
                mAlpha -= 0.03f; /*-= elapsedTime / 1000.0f * mFadeSpeed;*/
            }
            mAlpha = Math.Clamp(mAlpha, 0.0f, 1.0f);
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font, Vector2 position, Color colour)
        {

            // spriteBatch.DrawString(font, mLetter.ToString(), position, mColour[mColourIndex]);
             spriteBatch.DrawString(font, mLetter.ToString(), position, colour * mAlpha);
            //spriteBatch.DrawString(font, mLetter.ToString(), position, colour * mTextOpacity, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.0f);
        }
    }
}
