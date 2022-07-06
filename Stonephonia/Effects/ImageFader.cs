using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stonephonia.Effects
{
    public class ImageFader
    {
        private Texture2D mTexture;
        private Vector2 mPosition;
        private float mAlpha;
        private Timer mFadeTimer = new Timer();

        public ImageFader(Texture2D texture, Vector2 position)
        {
            mTexture = texture;
            mPosition = position;
            mAlpha = 0.0f;
        }

        public void SmoothFade(bool enabled, float fadeAmount)
        {
            mAlpha = enabled ? mAlpha += fadeAmount : mAlpha -= fadeAmount;
        }

        public void StagedFade(bool enabled, float fadeAmount, float fadeTime)
        {
            if (mFadeTimer.mCurrentTime > fadeTime)
            {
                if (enabled)
                {
                    mAlpha += fadeAmount;
                }
                else
                {
                    mAlpha -= fadeAmount;
                }
                mFadeTimer.Reset();
            }
        }

        public void GlowFade(bool enabled, float fadeAmount, float fadeMod, float fadeTime, float interval)
        {
            if (mAlpha < 1)
            {
                if (mFadeTimer.mCurrentTime < fadeTime)
                {
                    mAlpha += fadeAmount;
                }
                else if (mFadeTimer.mCurrentTime > fadeTime && mFadeTimer.mCurrentTime < fadeTime + interval)
                {
                    mAlpha -= fadeMod;
                }
                else if (mFadeTimer.mCurrentTime > fadeTime + interval)
                {
                    mFadeTimer.Reset();
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            mFadeTimer.Update(gameTime);
            mAlpha = Math.Clamp(mAlpha, 0.0f, 1.0f);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mTexture, mPosition, Color.White * mAlpha);
        }
    }
}
