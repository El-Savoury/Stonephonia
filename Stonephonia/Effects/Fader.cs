using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stonephonia.Effects
{
    public class Fader
    {
        private Timer mFadeTimer = new Timer();
        private Texture2D mTexture;
        private SpriteFont mFont;
        private Color mColour;
        public Vector2 mPosition;
        private string mText;
        public float mAlpha;
        private bool mVisible;

        public Fader(Texture2D texture, Vector2 position, Color colour, float alpha = 0.0f)
        {
            mTexture = texture;
            mPosition = position;
            mAlpha = alpha;
            mColour = colour;
        }

        public Fader(SpriteFont font, string text, Vector2 position, Color colour, float alpha = 0.0f)
        {
            mFont = font;
            mText = text;
            mPosition = position;
            mAlpha = alpha;
            mColour = colour;
        }

        public void Flash(float opaque, float transparent, float fadeAmount)
        {
            if (mAlpha == opaque) { mVisible = true; }
            else if (mAlpha <= transparent) { mVisible = false; }
            mAlpha = mVisible ? mAlpha -= fadeAmount : mAlpha += fadeAmount;
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

        public void GlowFade(float fadeAmount, float fadeMod, float fadeTime, float interval)
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

        public void DrawSprite(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mTexture, mPosition, mColour * mAlpha);
        }

        public void DrawString(SpriteBatch spriteBatch, bool centered)
        {
            if (centered)
            {
                mPosition.X = GamePort.renderSurface.Bounds.Width / 2 - mFont.MeasureString(mText).X / 2;
            }

            spriteBatch.DrawString(mFont, mText, mPosition, mColour * mAlpha);
        }
    }
}
