using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stonephonia
{
    public class Particle
    {
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
            spriteBatch.Draw(mTexture, mPosition, Color.White);
        }
    }
}
