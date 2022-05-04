using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stonephonia
{
    public class Sprite
    {
        public Texture2D mTexture;
        public Color mColour = Color.White;
        public Point mFrameSize;
        private Point mCurrentFrame;
        private Point mSheetSize;
        private int mTimeSinceLastFrame = 0;
        private int mTimePerFrame;

        public int direction { get; } // Allow subclasses to define behaviour based on movement direction.

        public Sprite(Texture2D texture, Point frameSize, Point currentFrame,
            Point sheetSize, int timePerFrame, Color colour)
        {
            mTexture = texture;
            mFrameSize = frameSize;
            mCurrentFrame = currentFrame;
            mSheetSize = sheetSize;
            mTimePerFrame = timePerFrame;
            mColour = colour;
        }

        public void Update(GameTime gameTime) // Animate sprite.
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

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            spriteBatch.Draw(mTexture, position, new Rectangle(mCurrentFrame.X * mFrameSize.X, mCurrentFrame.Y * mFrameSize.Y, mFrameSize.X, mFrameSize.Y),
                mColour, rotation: 0, origin: Vector2.Zero, scale: 1f, SpriteEffects.None, layerDepth: 0);
        }
    }
}
