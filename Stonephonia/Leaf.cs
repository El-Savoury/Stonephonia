using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stonephonia
{
    public class Leaf
    {
        private Sprite mSprite;
        public Vector2 mPosition;
        private Random mRandom;
        private int mSpeed;
        private bool mDirection;

        private enum State
        {
            falling,
            floating
        }

        private State mCurrentState = State.falling;

        public Leaf(Vector2 position)
        {
            mSprite = new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/leaf_sheet"),
                new Point(64, 64), new Point(0, 0), new Point(4, 1), 180, Color.White);
            mPosition = position;
            mRandom = new Random();
            mSpeed = mRandom.Next(3, 6);
            mDirection = SetSpriteDirection();
        }

        public bool SetSpriteDirection()
        {

            int randomNum = mRandom.Next(0, 2);
            bool direction = randomNum < 1;
            mSprite.mCurrentFrame.Y = randomNum;
            return direction;
        }

        private void FallVertically()
        {
            mPosition.Y += mSpeed;
            if (mPosition.Y > mRandom.Next(550, 800)) { mCurrentState = State.floating; }
        }

        private void MoveHorizontally()
        {
            if (mDirection) { mSprite.mCurrentFrame.Y = 2; }
            else { mSprite.mCurrentFrame.Y = 3; }
            mPosition.X += 9;
        }

        public void Update(GameTime gameTime)
        {
            mSprite.AnimateLoop(gameTime);

            if (mCurrentState == State.falling)
            {
                FallVertically();
            }
            else if (mCurrentState == State.floating)
            {
                MoveHorizontally();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            mSprite.Draw(spriteBatch, mPosition);
        }
    }
}
