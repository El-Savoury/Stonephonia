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
        public int mMaxSpeed;
        public int mCollisionOffset;

        public Rectangle mCollisionRect
        {
            get
            {
                return new Rectangle(
                    (int)(mPosition.X + 0.5f) + mCollisionOffset, (int)mPosition.Y,
                    mSprite.mFrameSize.X - (mCollisionOffset * 2), mSprite.mFrameSize.Y);
            }
        }

        public Entity(Vector2 position, int collisionOffset, int maxSpeed, float acceleration = 0)
        {
            mPosition = position;
            mMaxSpeed = maxSpeed;
            mCollisionOffset = collisionOffset;
            mAcceleration = acceleration;
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
