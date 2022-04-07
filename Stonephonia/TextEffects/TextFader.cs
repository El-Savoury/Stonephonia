﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stonephonia.TextEffects
{
    class TextFader
    {
        List<LetterFader> mLetters;
        SpriteFont mFont;
        float mTextXPos;
        float mTimeInterval;
        float mTotalTime = 0.0f;

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
                position.X += spacing;
            }
        }

    }
}
