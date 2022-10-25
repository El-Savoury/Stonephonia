using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Stonephonia
{
    public static class InputManager
    {
        private static KeyboardState mCurrentKeyState, mPrevKeyState;
        private static Timer mInputTimer = new Timer();

        private static bool InputDetected()
        {
            if (Keyboard.GetState().GetPressedKeyCount() > 0)
            {
                return true;
            }
            else { return false; }
        }

        public static void NoInputTimeOut(int timeLimit)
        {
            InputDetected();

            if (!InputDetected() && mInputTimer.mCurrentTime < timeLimit)
            {
               // Game1.self.Exit();
            }
            else { mInputTimer.Reset(); }
        }

        public static bool KeyPressed(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (mCurrentKeyState.IsKeyDown(key) && mPrevKeyState.IsKeyUp(key))
                    return true;
            }
            return false;
        }

        public static bool KeyReleased(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (mCurrentKeyState.IsKeyUp(key) && mPrevKeyState.IsKeyDown(key))
                    return true;
            }
            return false;
        }

        public static bool KeyHeld(params Keys[] keys)
        {
            foreach (Keys key in keys)
            {
                if (mCurrentKeyState.IsKeyDown(key))
                    return true;
            }
            return false;
        }

        public static void Update(GameTime gameTime)
        {
            mInputTimer.Update(gameTime);
            NoInputTimeOut(15);
            mPrevKeyState = mCurrentKeyState;
            mCurrentKeyState = Keyboard.GetState();

        }
    }
}
