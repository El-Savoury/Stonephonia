using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stonephonia.Managers
{
    public class LeafManager
    {
        private List<Leaf> mLeafList;
        private Random mRandom;

        public LeafManager()
        {
            mLeafList = new List<Leaf>();
            mRandom = new Random();
        }

        private void IdleLeaf()
        {
            if (mRandom.Next(0, 10) == 5 && mLeafList.Count < 1)
            {
                mLeafList.Add(new Leaf(new Vector2(mRandom.Next(64, 750), -64)));
            }

            for (int i = 0; i < mLeafList.Count; i++)
            {
                if (mLeafList[i].mPosition.Y > GamePort.renderSurface.Height)
                {
                    mLeafList[i] = null;
                    mLeafList.Remove(mLeafList[i]);
                }
            }

        }

        public void Update(GameTime gameTime)
        {
            IdleLeaf();

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
        }

    }
}
