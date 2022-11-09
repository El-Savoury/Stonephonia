using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stonephonia.Effects;

namespace Stonephonia.Managers
{
    public class FaderManager
    {
        private Fader[] mFaders;
        private int mCurrentFader = 0;

        public FaderManager(Fader[] faders)
        {
            mFaders = faders;
        }

        private void FadeInAndOut()
        {

        }

        public void Update(GameTime gameTime)
        {
            //mFaders[mCurrentFader].SetActive(true);
            //mFaders[mCurrentFader].Update(gameTime);

            //if (mFaders[mCurrentFader].mComplete && mCurrentFader < mFaders.Length)
            //{
            //    mCurrentFader++;
            //}
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            mFaders[mCurrentFader].DrawSprite(spriteBatch);
        }

    }
}
