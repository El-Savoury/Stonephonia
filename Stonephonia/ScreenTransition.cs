using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stonephonia.Screens;

namespace Stonephonia
{
    public class ScreenTransition
    {
        private Rectangle mBlackSquare, mWhiteSquare;
        private float mBlackSquareAlpha, mWhiteSquareAlpha;
        public bool mFadingIn = true;

        public ScreenTransition()
        {
            mBlackSquare = new Rectangle(0, 0, 800, 800);
            mWhiteSquare = new Rectangle(0, 0, 800, 800);
            mBlackSquareAlpha = 0.0f;
            mWhiteSquareAlpha = 0.0f;
        }

        public void FadeToCutscene(float fadeIn, float fadeOut, Screen currentScreen, Screen nextScreen)
        {
            if (mFadingIn)
            {
                mWhiteSquareAlpha += fadeIn;

                if (mWhiteSquareAlpha >= 1.0f)
                {
                    mBlackSquareAlpha = 1.0f;
                    mFadingIn = false;
                }
            }

            else
            {
                mWhiteSquareAlpha -= fadeOut;
                if (mWhiteSquareAlpha <= 0.0f) { ScreenManager.ChangeScreen(currentScreen, nextScreen); }
            }
        }


        public void FadeToGamePlay(float fadeIn, float fadeOut, Screen currentScreen)
        {
            if (mFadingIn)
            {
                mWhiteSquareAlpha += fadeIn;

                if (mWhiteSquareAlpha >= 1.0f)
                {
                    mFadingIn = false;
                }
            }

            else
            {
                mWhiteSquareAlpha -= fadeOut;
                if (mWhiteSquareAlpha <= 0.0f) { ScreenManager.ChangeScreen(currentScreen, new GameplayScreen()); }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ScreenManager.pixel, mBlackSquare, Color.Black * mBlackSquareAlpha);
            spriteBatch.Draw(ScreenManager.pixel, mWhiteSquare, Colours.lightBlue * mWhiteSquareAlpha);
        }

    }
}
