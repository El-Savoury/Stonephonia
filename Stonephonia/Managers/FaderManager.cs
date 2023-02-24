using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stonephonia.Effects;

namespace Stonephonia.Managers
{
    public class FaderManager
    {
        private Timer mTimer;
        public Fader mFader;
        public Fader[] mFaders;

        public FaderManager(Fader[] faders)
        {
            mTimer = new Timer();
            mFaders = faders;
        }

        public void FadeInAndOut(Fader fader, float speed1, float speed2, int activationTime, int delay)
        {
            int deactivationTime = activationTime + delay;

            if (mTimer.mCurrentTime > deactivationTime)
            {
                fader.SmoothFade(false, speed2);
            }
            else if (mTimer.mCurrentTime > activationTime)
            {
                fader.SmoothFade(true, speed1);
            }
        }

        public void Update(GameTime gameTime)
        {
            mTimer.Update(gameTime);
            foreach (Fader fader in mFaders)
            {
                fader.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Fader fader in mFaders)
            {
                fader.DrawSprite(spriteBatch);
            }
        }

        public void DrawString(SpriteBatch spriteBatch)
        {
            foreach (Fader fader in mFaders)
            {
                fader.DrawString(spriteBatch, true);
            }
        }

    }
}
