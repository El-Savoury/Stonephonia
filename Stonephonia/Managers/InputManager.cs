using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Stonephonia.Screens;

namespace Stonephonia
{
    public static class InputManager
    {
        private static KeyboardState mCurrentKeyState, mPrevKeyState;
        private static Timer mInputTimer = new Timer();

        private static bool AnyInputDetected()
        {
            if (Keyboard.GetState().GetPressedKeyCount() > 0) { return true; }
            else { return false; }
        }

        public static bool SpecificInputDetected(params Keys[] keys)
        {
            bool inputDetected = false;

            foreach (Keys key in keys)
            {
                if (KeyPressed(keys)) { inputDetected = true; }
                else { inputDetected = false; }
            }
            return inputDetected;
        }

        public static void NoInputTimeOut(GameTime gameTime, int timeLimit, Screen currentScreen, Screen nextScreen)
        {
            mInputTimer.Update(gameTime);
            bool recentInput = AnyInputDetected();

            if (mInputTimer.mCurrentTime > timeLimit)
            {
                if (!recentInput) { ScreenManager.ChangeScreen(currentScreen, nextScreen); }
            }
            else if (recentInput) { mInputTimer.Reset(); }
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
            mPrevKeyState = mCurrentKeyState;
            mCurrentKeyState = Keyboard.GetState();

        }
    }
}
