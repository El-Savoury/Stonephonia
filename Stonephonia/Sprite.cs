using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stonephonia
{
    public class Sprite
    {
        public Texture2D mTexture;
        public Color mColour = Color.White;
        public Point mFrameSize; // Size of frame containing sprite to be shown.
        private Point mCurrentFrame; // Top left coordinate of desired frame to begin animating from.
        private Point mSheetSize; // Number of rows and collumns on spritesheet.
        private int mTimeSinceLastFrame = 0;
        private int mTimePerFrame;
        private float mAlpha;

        public Sprite(Texture2D texture, Point frameSize, Point currentFrame,
            Point sheetSize, int timePerFrame, Color colour, float alpha = 1.0f)
        {
            mTexture = texture;
            mFrameSize = frameSize;
            mCurrentFrame = currentFrame;
            mSheetSize = sheetSize;
            mTimePerFrame = timePerFrame;
            mColour = colour;
            mAlpha = alpha;
        }

        public void SetVisible(bool visible)
        {
            mAlpha = visible ? 1.0f : 0.0f;
        }

        private void Animate(GameTime gameTime)
        {
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

        public void Update(GameTime gameTime) // Animate sprite.
        {
            Animate(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(mTexture, position, new Rectangle(mCurrentFrame.X * mFrameSize.X, mCurrentFrame.Y * mFrameSize.Y, mFrameSize.X, mFrameSize.Y),
                mColour * mAlpha);
        }
    }
}
