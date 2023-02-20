using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stonephonia
{
    public class Sprite
    {
        public Texture2D mTexture;
        public Color mColour = Color.White;
        public Point mFrameSize; // Size of frame containing sprite to be shown.
        public Point mCurrentFrame; // The row and column numbers of the desired frame to animate from on the spritesheet. 
        public Point mSheetSize; // Number of rows and columns needed to be shown from spritesheet.
        public bool mAnimationComplete = false;
        public int mTimePerFrame;
        private int mTimeSinceLastFrame = 0;
        public float mAlpha;

        public Sprite(Texture2D texture, Point frameSize, Point currentFrame,
            Point sheetSize, int timePerFrame, Color colour, bool visible = true)
        {
            mTexture = texture;
            mFrameSize = frameSize;
            mCurrentFrame = currentFrame;
            mSheetSize = sheetSize;
            mTimePerFrame = timePerFrame;
            mColour = colour;
            SetVisible(visible);
        }

        public void SetVisible(bool visible)
        {
            mAlpha = visible ? 1.0f : 0.0f;
        }

        public void ResetAnimation(Point currentFrame)
        {
            mCurrentFrame = currentFrame;
            mTimeSinceLastFrame = 0;
            mAnimationComplete = false;
        }

        private void AnimateOnce(GameTime gameTime)
        {
            mTimeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (!mAnimationComplete && mTimeSinceLastFrame > mTimePerFrame)
            {
                mTimeSinceLastFrame = 0;
                mCurrentFrame.X++;

                if (mCurrentFrame.X >= mSheetSize.X)
                {
                    //mCurrentFrame = new Point(0, 0);
                    mCurrentFrame.X = mSheetSize.X - 1;
                    mAnimationComplete = true;
                }
            }
        }

        private void AnimateLoop(GameTime gameTime)
        {
            mTimeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (mTimeSinceLastFrame > mTimePerFrame)
            {
                mTimeSinceLastFrame = 0;
                ++mCurrentFrame.X;

                if (mCurrentFrame.X >= mSheetSize.X)
                {
                    mCurrentFrame.X = 0;
                }
            }
        }

        public void Update(GameTime gameTime, bool loop) // Animate sprite.
        {
            if (loop) { AnimateLoop(gameTime); }
            else { AnimateOnce(gameTime); }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(mTexture, position, new Rectangle(mCurrentFrame.X * (mFrameSize.X + 4), mCurrentFrame.Y * mFrameSize.Y, mFrameSize.X, mFrameSize.Y),
                mColour * mAlpha);
        }

        public void DrawScaled(SpriteBatch spriteBatch, Vector2 position, float scale)
        {
            spriteBatch.Draw(mTexture, position, new Rectangle(mCurrentFrame.X * mFrameSize.X, mCurrentFrame.Y * mFrameSize.Y, mFrameSize.X, mFrameSize.Y),
            mColour * mAlpha, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
        }
    }
}
