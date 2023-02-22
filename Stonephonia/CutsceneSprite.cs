using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Stonephonia
{
    public class CutsceneSprite
    {
        private Sprite mSprite;
        private Texture2D mMask;
        float mFade1, mFade2;
        float mMaskAlpha;
        float mDelayCounter = 0;
        bool mVisible = false;
        Vector2 mPosition;

        public CutsceneSprite(Sprite sprite, Texture2D mask, float fade1, float fade2)
        {
            mSprite = sprite;
            mMask = mask;
            mFade1 = fade1;
            mFade2 = fade2;
        }

        public void FadeInMask()
        {
            if (mMaskAlpha < 1.0f)
            {
                mMaskAlpha += mFade1;
                if (mMaskAlpha >= 1.0f) { mSprite.SetVisible(true); }
            }
            else 
            {

            }
        }

        public void Fade(float delayTime)
        {
            if (!mVisible)
            {
                mMaskAlpha += mFade1;
                if (mMaskAlpha >= 1.0f) 
                {
                    mDelayCounter++;
                    mSprite.SetVisible(true);
                    mVisible = true;
                }

            }
        }

        public void FadeOut()
        {

        }

        public void Update(GameTime gameTime)
        {

            if (mMaskAlpha <= 0.0f && mVisible) { mSprite.Update(gameTime, true); }


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            mSprite.Draw(spriteBatch, mPosition);
            spriteBatch.Draw(mMask, mPosition, Color.White * mMaskAlpha);
        }
    }
}
