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
        private List<Particle> mParticleList;
        private Texture2D mSmallTexture, mLargeTexture;

        public ParticleManager()
        {
            mParticleTimer = new Timer();
            mRandom = new Random();
            mParticleList = new List<Particle>();
        }

        public void LoadAssets()
        {
            mSmallTexture = ScreenManager.contentMgr.Load<Texture2D>("Sprites/particle_small");
            mLargeTexture = ScreenManager.contentMgr.Load<Texture2D>("Sprites/particle_large");
        }

        private void SpawnParticles()
        {
            int spawnTime = mRandom.Next(0, 15);
            int spawnChance = mRandom.Next(1, 10);

            // Randomise position along top of screen
            Vector2 particleSpawnPos = new Vector2(mRandom.Next(-400, 800), 0);

            if (mParticleTimer.mCurrentTime > spawnTime)
            {
                if (spawnChance == 9)
                {
                    mParticleList.Add(new Particle(particleSpawnPos, new Vector2(1, 2), mLargeTexture));
                }
                else
                {
                    mParticleList.Add(new Particle(particleSpawnPos, new Vector2(1, 1), mSmallTexture));
                }
                mParticleTimer.Reset();
            }
        }

        private void MoveParticles()
        {
            SpawnParticles();

            if (mParticleList != null)
            {
                for (int i = 0; i < mParticleList.Count; i++)
                {
                    mParticleList[i].Update();
                    UnloadParticle(i); // Unload if particle leaves screen
                }
            }
        }

        private void UnloadParticle(int i)
        {
            if (mParticleList[i].mPosition.X > GamePort.renderSurface.Width ||
                mParticleList[i].mPosition.Y > GamePort.renderSurface.Height)
            {
                mParticleList[i] = null;
                mParticleList.Remove(mParticleList[i]);
            }
        }

        public void Update(GameTime gameTime)
        {
            mParticleTimer.Update(gameTime);
            MoveParticles();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (mParticleList != null)
            {
                foreach (Particle particle in mParticleList)
                {
                    particle.Draw(spriteBatch);
                }
            }
        }
    }
}
