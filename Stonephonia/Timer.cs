using Microsoft.Xna.Framework;

namespace Stonephonia
{
    public class Timer
    {
        public float mCurrentTime = 0.0f;

        public Timer()
        {
        }

        public void Update(GameTime gameTime)
        {
            mCurrentTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000;
        }

        public void Reset()
        {
            mCurrentTime = 0.0f;
        }

    }
}
