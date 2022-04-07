using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stonephonia
{
    public class Entity
    {
        protected Texture2D mTexture;
        public Vector2 mPosition;
        public Color mColour = Color.White;
        public int mVelocity;
        public int mMoveSpeed = 0;
        public int mCollisionOffset = 0;

        public Rectangle mCollisionRect
        {
            get
            {
                return new Rectangle(
                    (int)mPosition.X + mCollisionOffset, (int)mPosition.Y,
                    mTexture.Width - mCollisionOffset, mTexture.Height);
            }
        }

        public Entity(Texture2D texture)
        {
            mTexture = texture;
        }

        public virtual void Update(GameTime gameTime, Entity entity)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mTexture, mPosition, mColour);
        }

        // AABB Collision
        protected bool CollideWithLeft(Entity entity)
        {
            return mCollisionRect.Right + mVelocity > entity.mCollisionRect.Left &&
                   mCollisionRect.Left < entity.mCollisionRect.Left;
        }

        protected bool CollideWithRight(Entity entity)
        {
            return mCollisionRect.Left + mVelocity < entity.mCollisionRect.Right &&
                   mCollisionRect.Right > entity.mCollisionRect.Right;
        }
    }
}
