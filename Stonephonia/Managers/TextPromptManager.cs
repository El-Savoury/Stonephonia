﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Stonephonia.Managers
{
    public class TextPromptManager
    {
        private TextPrompt[] mtextArray;
        private Timer mTimer = new Timer();

        public TextPromptManager(TextPrompt[] text)
        {
            mtextArray = text;
        }

        Buttons[] mButtons = new Buttons[]
        {
            Buttons.DPadRight,
            Buttons.DPadLeft
        };

        private void DisplayPrompts() // Kind of bodged hard coded way to show input prompts
        {
            if (!mtextArray[0].mTextComplete)
            {
                mtextArray[0].PromptInput(false, mTimer, mButtons, Keys.Left, Keys.Right);
            }
            else if (!mtextArray[1].mTextComplete)
            {
                if (mtextArray[0].mTextComplete)
                {
                    mtextArray[1].PromptAction(mTimer, ScreenManager.pusher);
                }
            }
        }

        public void FadeOutPrompt(float fadeAmount)
        {
            foreach (TextPrompt text in mtextArray)
            {
                text.mFader.mAlpha -= fadeAmount;
            }
        }

        public void Update(GameTime gameTime)
        {
            mTimer.Update(gameTime);
            DisplayPrompts();

            foreach (TextPrompt text in mtextArray)
            {
                text.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (TextPrompt text in mtextArray)
            {
                text.Draw(spriteBatch);
            }
        }

    }
}
