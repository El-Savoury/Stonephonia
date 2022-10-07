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
        private Point mSheetSize; // Number of rows and collumns needed to be shown from spritesheet.
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
                    //++mCurrentFrame.Y;

                    //if (mCurrentFrame.Y >= mSheetSize.Y) { mCurrentFrame.Y /= mSheetSize.X; }
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

            //spriteBatch.Draw(mTexture, position, new Rectangle(mCurrentFrame.X * mFrameSize.X, mCurrentFrame.Y * mFrameSize.Y, mFrameSize.X, mFrameSize.Y),
            //mColour * mAlpha, 0.0f, Vector2.Zero, Vector2.One, mSpriteDirection, 0.0f);
        }
    }
}
