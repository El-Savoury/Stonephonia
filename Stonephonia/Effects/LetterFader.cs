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
        private float mTextOpacity;

        public LetterFader(bool enabled, char letter, float fadeSpeed, float textOpacity)
        {
            mEnabled = enabled;
            mLetter = letter;
            mFadeSpeed = fadeSpeed;
            mTextOpacity = textOpacity;
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

        public void Update(float elapsedTime)
        {
            if (mEnabled)
            {
                mTextOpacity += elapsedTime / 1000.0f * mFadeSpeed;
            }
            else
            {
                mTextOpacity -= elapsedTime / 1000.0f * mFadeSpeed;
            }
            mTextOpacity = Math.Clamp(mTextOpacity, 0.0f, 1.0f);
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font, Vector2 position, Color colour)
        {
            spriteBatch.DrawString(font, mLetter.ToString(), position, colour * mTextOpacity);
            //spriteBatch.DrawString(font, mLetter.ToString(), position, colour * mTextOpacity, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.0f);
        }
    }
}
