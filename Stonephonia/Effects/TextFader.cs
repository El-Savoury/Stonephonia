using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stonephonia.Effects
{
    public class TextFader
    {
        private List<LetterFader> mLetters;
        private SpriteFont mFont;
        private float mTextXPos;
        private float mTimeInterval;
        private float mTotalTime = 0.0f;

        public TextFader(SpriteFont font, string text, float timeInterval, float fadeSpeed, float textOpacity)
        {
            mFont = font;
            mTimeInterval = timeInterval;

            char[] letters = text.ToCharArray();
            mLetters = new List<LetterFader> { };

            for (int i = 0; i < letters.Length; i++)
            {
                mLetters.Add(new LetterFader(false, letters[i], fadeSpeed, textOpacity));
            }

            // Get length of original string and set position of first letter to center text on screen 
            mTextXPos = GamePort.renderSurface.Bounds.Width / 2 - font.MeasureString(text).X / 2;
        }

        public void Update(float elapsedTime)
        {
            mTotalTime += elapsedTime;

            int enabledIndex = (int)(mTotalTime / mTimeInterval);
            for (int i = 0; i < mLetters.Count; i++)
            {
                if (i < enabledIndex)
                {
                    mLetters[i].SetEnabled(true);
                }
                else
                {
                    mLetters[i].SetEnabled(false);
                }
            }

            foreach (LetterFader letter in mLetters)
            {
                letter.Update(elapsedTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, int spacing, bool centered, Color colour)
        {
            if (centered)
            {
                position.X = mTextXPos;
            }

            foreach (LetterFader letter in mLetters)
            {
                letter.Draw(spriteBatch, mFont, position, colour);

                // Fix kerning issues to monospace letters
                Vector2 letterSize = mFont.MeasureString(letter.mLetter.ToString());

                switch (letterSize.X)
                {
                    case 2:
                        position.X += spacing - 3;
                        break;

                    case 4:
                        position.X += spacing - 1;
                        break;

                    case 6:
                        position.X += spacing + 1;
                        break;

                    default:
                        position.X += spacing;
                        break;
                }
            }
        }

    }
}
