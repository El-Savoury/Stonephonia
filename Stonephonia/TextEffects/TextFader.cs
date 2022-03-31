using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stonephonia.TextEffects
{
    class TextFader
    {
        List<LetterFader> mLetters;
        float mTotalTime;
        float mTimeInterval;

        public TextFader(string text, float totalTime, float timeInterval,
            float fadeSpeed, float textOpacity)
        {
            char[] letters = text.ToCharArray();
            mLetters = new List<LetterFader> { };

            for (int i = 0; i < letters.Length; i++)
            {
                mLetters.Add(new LetterFader(false, letters[i], fadeSpeed, textOpacity));
            }

            mTotalTime = totalTime;
            mTimeInterval = timeInterval;
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

        public void Draw(SpriteBatch spriteBatch, SpriteFont font, Vector2 position, int spacing, Color colour)
        {
            foreach (LetterFader letter in mLetters)
            {
                letter.Draw(spriteBatch, font, position, colour);
                position.X += spacing;
            }
        }

    }
}
