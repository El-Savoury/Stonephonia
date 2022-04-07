using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Stonephonia
{
    class Character : Entity
    {
        public Character(Texture2D texture)
        : base(texture)
        {
        }

        public override void Update(GameTime gameTime, Entity entity)
        {
            Move(entity);
        }

        private void Move(Entity entity)
        {
            int inputDir = 0;

            if (InputManager.KeyHeld(Keys.Right) || InputManager.KeyHeld(Keys.D))
            {
                inputDir += 1;
            }
            if (InputManager.KeyHeld(Keys.Left) || InputManager.KeyHeld(Keys.A))
            {
                inputDir -= 1;
            }
            mVelocity = inputDir * mMoveSpeed;

            Collide(entity);

            mPosition.X += mVelocity;

            ClampToScreen();
        }

        // TODO: PASS IN ROCKS
        private void Collide(Entity entity)
        {
            if (mVelocity > 0 && CollideWithLeft(entity))
            {
                while (mCollisionRect.Right < entity.mCollisionRect.Left)
                {
                    mPosition.X += Math.Sign(mVelocity);
                }
                mVelocity = 0;
            }

            if (mVelocity < 0 && CollideWithRight(entity))
            {
                while (mCollisionRect.Left > entity.mCollisionRect.Right)
                {
                    mPosition.X += Math.Sign(mVelocity);
                }
                mVelocity = 0;
            }
        }

        private void ClampToScreen()
        {
            if (mCollisionRect.X < GamePort.renderSurface.Bounds.X)
            {
                mPosition.X = GamePort.renderSurface.Bounds.X - mCollisionOffset;
            }

            if (mCollisionRect.Right > GamePort.renderSurface.Bounds.Right)
            {
                mPosition.X = GamePort.renderSurface.Bounds.Right - mCollisionRect.Width;
            }
        }
    }
}
