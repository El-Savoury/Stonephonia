using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stonephonia
{
    abstract class Sprite
    {
        private Texture2D mTexture;
        protected Vector2 mPosition;
        protected Point mFrameSize;
        private Point mCurrentFrame;
        private Point mSheetSize;
        private int mCollisionOffset;
        private int mTimeSinceLastFrame = 0;
        private int mTimePerFrame;
        protected int mVelocity;

        // Allow subclasses to define behaviour based on movement direction
        public abstract int direction { get; }

        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                  (int)mPosition.X + mCollisionOffset,
                  (int)mPosition.Y + mCollisionOffset,
                  mFrameSize.X - (mCollisionOffset * 2),
                  mFrameSize.Y - (mCollisionOffset * 2));
            }
        }

        public Sprite(Texture2D texture, Vector2 position,
            Point frameSize, Point currentFrame, Point sheetSize,
            int collisionOffset, int timePerFrame, int velocity)
        {
            mTexture = texture;
            mPosition = position;
            mFrameSize = frameSize;
            mCurrentFrame = currentFrame;
            mSheetSize = sheetSize;
            mCollisionOffset = collisionOffset;
            mTimePerFrame = timePerFrame;
            mVelocity = velocity;
        }

        public virtual void Update(GameTime gameTime)
        {
            // Animate sprite
            mTimeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (mTimeSinceLastFrame > mTimePerFrame)
            {
                mTimeSinceLastFrame = 0;
                ++mCurrentFrame.X;

                if (mCurrentFrame.X >= mSheetSize.X)
                {
                    mCurrentFrame.X = 0;
                    ++mCurrentFrame.Y;

                    if (mCurrentFrame.Y >= mSheetSize.Y) { mCurrentFrame.Y = 0; }
                }
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mTexture, mPosition, new Rectangle(mCurrentFrame.X * mFrameSize.X, mCurrentFrame.Y * mFrameSize.Y, mFrameSize.X, mFrameSize.Y),
                Color.White, rotation: 0, origin: Vector2.Zero, scale: 1f, SpriteEffects.None, layerDepth: 0);
        }
    }
}
