using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Stonephonia.Effects;

namespace Stonephonia.Managers
{
    public class TextPromptManager
    {
        private TimedTextPrompt[] mTextPrompts;
        private Pusher mPusher;

        public TextPromptManager(TimedTextPrompt[] prompts, Pusher pusher)
        {
            mTextPrompts = prompts;
            mPusher = pusher;
        }

        private void DisplayPrompts(GameTime gameTime, Pusher pusher)
        {
            if (mTextPrompts[0].mPromptComplete &&
                mTextPrompts[0].mAlpha <= 0.0f)
            {
                mTextPrompts[1].PromptPush(mPusher);
            }
            else
            {
                mTextPrompts[0].PromptMove(Keys.Left, Keys.Right);
            }
        }

        public void Update(GameTime gameTime)
        {
            DisplayPrompts(gameTime, mPusher);

            foreach (TimedTextPrompt prompt in mTextPrompts)
            {
                prompt.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (TimedTextPrompt prompt in mTextPrompts)
            {
                prompt.Draw(spriteBatch);
            }
        }

    }
}
