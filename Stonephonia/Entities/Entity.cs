using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stonephonia
{
    public class Entity
    {
        public Sprite mSprite;
        public Vector2 mPosition;
        public float mVelocity;
        public float mAcceleration;
        public int mMaxSpeed = 0;
        public int mCollisionOffset = 0;

        public Rectangle mCollisionRect
        {
            get
            {
                return new Rectangle(
                    (int)(mPosition.X + 0.5f) + mCollisionOffset,
                    (int)mPosition.Y,
                    mSprite.mFrameSize.X - mCollisionOffset,
                    mSprite.mFrameSize.Y);
            }
        }

        public Entity(float xPos, float yPos)
        {
            mPosition = new Vector2(xPos, yPos);
        }

        public virtual void Update(GameTime gameTime)
        {
            mSprite.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            mSprite.Draw(spriteBatch, mPosition);
        }
    }
}
