using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stonephonia
{
    public class Particle
    {
        //public Rectangle mBounds;
        public Vector2 mPosition;
        private Vector2 mVelocity;
        private Texture2D mTexture;

        public Particle(Vector2 position, Vector2 velocity, Texture2D texture)
        {
            mPosition = position;
            mVelocity = velocity;
            mTexture = texture;
        }

        public void Update()
        {
            mPosition.X += mVelocity.X;
            mPosition.Y += mVelocity.Y;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(mTexture, mPosition, Color.White);
            spriteBatch.Draw(mTexture, mPosition, new Rectangle(0, 0, mTexture.Width, mTexture.Height), Color.White, 0.0f, Vector2.Zero, 2.0f, SpriteEffects.None, 1.0f);
        }
    }
}
