using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Stonephonia.Managers
{
    public class TextManager
    {
        private TimedText[] mtextArray;
        private Timer mTimer = new Timer();
        private bool mCollision = false;

        public TextManager(TimedText[] text)
        {
            mtextArray = text;
        }

        private void EnablePromptOnCollision()
        {
            foreach (Rock rock in ScreenManager.rock)
            {
                if (rock.CollideWithPlayer(ScreenManager.pusher))
                {
                    mCollision = true;
                }
            }
        }

        private void DisplayPrompts() // Kind of bodged hard coded way to show input prompts
        {
            EnablePromptOnCollision();

            if (!mtextArray[0].mTextComplete)
            {
                mtextArray[0].PromptMove(mTimer, Keys.Left, Keys.Right);
            }
            else if (!mtextArray[1].mTextComplete)
            {
                if (mtextArray[0].mTextComplete && mCollision)
                {
                    mtextArray[1].PromptPush(mTimer, ScreenManager.pusher);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            mTimer.Update(gameTime);
            DisplayPrompts();

            foreach (TimedText text in mtextArray)
            {
                text.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (TimedText text in mtextArray)
            {
                text.Draw(spriteBatch);
            }
        }

    }
}
