using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stonephonia
{
    public class Entity
    {
        protected Texture2D mTexture;
        public Vector2 mPosition;
        public Color mColour = Color.White;
        public float mVelocity;
        public float mSpeedModifier;
        public int mMaxSpeed = 0;
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

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Update(GameTime gameTime, Rock[] rock)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mTexture, mPosition, mColour);
        }
    }
}
