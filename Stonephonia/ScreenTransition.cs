using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stonephonia.Screens;

namespace Stonephonia
{
    public class ScreenTransition
    {
        private Rectangle mBlackSquare, mWhiteSquare;
        private float mBlackSquareAlpha, mWhiteSquareAlpha;
        bool fadingIn = true;

        public ScreenTransition()
        {
            mBlackSquare = new Rectangle(0, 0, 800, 800);
            mWhiteSquare = new Rectangle(0, 0, 800, 800);
            mBlackSquareAlpha = 0.0f;
            mWhiteSquareAlpha = 0.0f;
        }

        public void FadeToNextScreen(float fadeIn, float fadeOut, Screen currentScreen, Screen nextScreen)
        {
            if (fadingIn)
            {
                mWhiteSquareAlpha += fadeIn;

                if (mWhiteSquareAlpha >= 1.0f)
                {
                    mBlackSquareAlpha = 1.0f;
                    fadingIn = false;
                }
            }

            else
            {
                mWhiteSquareAlpha -= fadeOut;
                if (mWhiteSquareAlpha <= 0.0f) { ScreenManager.ChangeScreen(currentScreen, nextScreen); }
            }
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ScreenManager.pixel, mBlackSquare, Color.Black * mBlackSquareAlpha);
            spriteBatch.Draw(ScreenManager.pixel, mWhiteSquare, Colours.lightBlue * mWhiteSquareAlpha);
        }

    }
}
