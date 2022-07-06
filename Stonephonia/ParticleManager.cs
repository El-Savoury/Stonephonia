using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stonephonia
{
    public class ParticleManager
    {
        private Timer mParticleTimer;
        private Random mRandom;
        private List<Particle> mSmallParticles;
        private List<Rectangle> mLargeParticles;

        public ParticleManager()
        {
            mParticleTimer = new Timer();
            mRandom = new Random();
            mSmallParticles = new List<Particle>();
            mLargeParticles = new List<Rectangle>();
        }

        private void SpawnSmallParticles()
        {
            int spawnTime = mRandom.Next(1,3);
            Point smallParticlePos = new Point(mRandom.Next(-30, 200), 0);
            Point smallParticleSize = new Point(1, 1);
            Point smallParticleVel = new Point(1, 2);

            if (mParticleTimer.mCurrentTime > spawnTime)
            {
                mSmallParticles.Add(new Particle(smallParticlePos, smallParticleSize));
                mParticleTimer.Reset();
            }

            if (mSmallParticles != null)
            {
                for (int i = 0; i < mSmallParticles.Count; i++)
                {
                    mSmallParticles[i].mBounds.X += smallParticleVel.X;
                    mSmallParticles[i].mBounds.Y += smallParticleVel.Y;

                    // Unload if particle leaves screen
                    if (mSmallParticles[i].mBounds.X > 200 ||
                        mSmallParticles[i].mBounds.Y > 180)
                    {
                        mSmallParticles[i] = null;
                        mSmallParticles.Remove(mSmallParticles[i]);
                    }
                }
            }

        }


        public void Update(GameTime gameTime)
        {
            mParticleTimer.Update(gameTime);
            SpawnSmallParticles();
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            if (mSmallParticles != null)
            {
                foreach (Particle particle in mSmallParticles)
                {
                    particle.Draw(spriteBatch, texture);
                }
            }
        }
    }
}
