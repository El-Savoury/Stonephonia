using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Stonephonia.Screens;

namespace Stonephonia
{
    public static class InputManager
    {
        private static KeyboardState mCurrentKeyState, mPrevKeyState;
        private static Timer mInputTimer = new Timer();
        private static int mStickDirection = 0;

        static GamePadState mCurrentGamePadState;
        static GamePadState mPrevGamePadState;

        public static bool AnyKeyInputDetected()
        {
            if (Keyboard.GetState().GetPressedKeyCount() > 0) { return true; }
            else { return false; }
        }

        public static bool AnyPadInputDetected(params Buttons[] buttons)
        {
            bool inputDetected = false;

            foreach (Buttons button in buttons)
            {
                if (PadPressed(button)) { inputDetected = true; }
                else { inputDetected = false; }
            }
            return inputDetected;
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

        public static bool SpecificInputDetected(params Buttons[] buttons)
        {
            bool inputDetected = false;

            foreach (Buttons button in buttons)
            {
                if (PadPressed(button)) { inputDetected = true; }
                else { inputDetected = false; }
            }

            return inputDetected;
        }

        public static void NoInputTimeOut(GameTime gameTime, int timeLimit, Screen currentScreen, Screen nextScreen)
        {
            mInputTimer.Update(gameTime);
            bool recentInput = AnyKeyInputDetected();

            if (mInputTimer.mCurrentTime > timeLimit)
            {
                mInputTimer.Reset();

                if (!recentInput) { ScreenManager.ChangeScreen(currentScreen, nextScreen); }
            }
        }

        // GampPad Controls
        public static GamePadState GetPadState()
        {
            mPrevGamePadState = mCurrentGamePadState;
            mCurrentGamePadState = GamePad.GetState(PlayerIndex.One);

            return mCurrentGamePadState;
        }

        public static void GetGamePadInput()
        {
            GamePadCapabilities capabilities = GamePad.GetCapabilities(PlayerIndex.One);

            if (capabilities.IsConnected)
            {
                GamePadState state = GetPadState();


                // left stick
                if (capabilities.HasLeftXThumbStick)
                {
                    CheckStickDirection(state);
                }
            }
        }

        private static void CheckStickDirection(GamePadState state)
        {
            if (state.ThumbSticks.Left.X < -0.5f) { mStickDirection = -1; }
            else if (state.ThumbSticks.Left.X > 0.5f) { mStickDirection = 1; }
            else { mStickDirection = 0; }
        }

        public static bool PadPressed(params Buttons[] buttons)
        {
            foreach (Buttons button in buttons)
            {
                if (mCurrentGamePadState.IsButtonDown(button) && mPrevGamePadState.IsButtonUp(button))
                    return true;
            }
            return false;
        }

        public static bool PadReleased(params Buttons[] buttons)
        {
            foreach (Buttons button in buttons)
            {
                if (mCurrentGamePadState.IsButtonUp(button) && mPrevGamePadState.IsButtonDown(button))
                    return true;
            }
            return false;
        }

        public static bool PadHeld(params Buttons[] buttons)
        {
            foreach (Buttons button in buttons)
            {
                if (mCurrentGamePadState.IsButtonDown(button))
                    return true;
            }
            return false;
        }

        // Keyboard Controls
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
            GetPadState();
        }
    }
}
