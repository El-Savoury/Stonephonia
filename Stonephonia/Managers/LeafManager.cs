using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stonephonia.Managers
{
    public class LeafManager
    {
        private Rectangle mEmitter;
        private List<Leaf> mLeafList;
        private Random mRandom;
        private Timer mTimer;

        public LeafManager()
        {
            mEmitter = new Rectangle(0, 0, 0, 0);
            mLeafList = new List<Leaf>();
            mRandom = new Random();
            mTimer = new Timer();
        }

        private void SpawnLeaf(Vector2 position)
        {
            mLeafList.Add(new Leaf(position));
            mTimer.Reset();
        }

        private void RandomlyDropLeaves(GameTime gameTime, int timeLimit)
        {
            mTimer.Update(gameTime);

            // Randomly spawn up to two leaves
            if (mRandom.Next(0, 700) == 5 && mLeafList.Count < 2)
            {
                SpawnLeaf(new Vector2(mRandom.Next(64, 750), -64));
            }

            // If no leaf has randomly spawned after X seconds spawn a leaf and reset timer
            if (mTimer.mCurrentTime > timeLimit && mLeafList.Count == 0)
            {
                SpawnLeaf(new Vector2(mRandom.Next(64, 750), -64));
            }
        }

        private void CreateEmitter(Point position, Point size)
        {
            mEmitter = new Rectangle(position, size);
        }

        private void DestroyEmitter()
        {
            mEmitter = new Rectangle(0, 0, 0, 0);
        }

        private void EmitterFollowsRock(Pusher pusher)
        {
            int rockCenter = (int)pusher.mCurrentRock.mPosition.X + pusher.mCurrentRock.mSprite.mFrameSize.X / 2;

            if (mEmitter.Width == 0)
            {
                CreateEmitter(new Point(rockCenter - 150, 100), new Point(300, 0));
            }

            mEmitter.X += (int)(pusher.mVelocity + (Math.Sign(pusher.mVelocity) * 0.5f));
        }

        private void EmitMultipleLeaves(Pusher pusher)
        {
            EmitterFollowsRock(pusher);

            if (mLeafList.Count < 3 && mRandom.Next(0, 45) == 5)
            {
                SpawnLeaf(new Vector2(mRandom.Next(mEmitter.X, mEmitter.X + mEmitter.Width), mEmitter.Y));
            }
        }

        private void DespawnLeaf()
        {
            for (int i = 0; i < mLeafList.Count; i++)
            {
                if (mLeafList[i].mPosition.X > GamePort.renderSurface.Width ||
                    mLeafList[i].mPosition.Y > GamePort.renderSurface.Height)
                {
                    mLeafList[i] = null;
                    mLeafList.Remove(mLeafList[i]);
                }
            }
        }

        public void FadeOutLeaves(float fadeAmount)
        {
            foreach (Leaf leaf in mLeafList)
            {
                leaf.FadeOut(fadeAmount);
            }
        }

        public void Update(GameTime gameTime, Pusher pusher)
        {
            if (pusher.mCurrentState == Pusher.State.push) { EmitMultipleLeaves(pusher); }
            else { DestroyEmitter(); }

            RandomlyDropLeaves(gameTime, 15);
            DespawnLeaf();

            foreach (Leaf leaf in mLeafList)
            {
                leaf.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Leaf leaf in mLeafList)
            {
                leaf.Draw(spriteBatch);
            }

            //spriteBatch.Draw(ScreenManager.pixel, mEmitter, Color.Blue);
        }

    }
}
