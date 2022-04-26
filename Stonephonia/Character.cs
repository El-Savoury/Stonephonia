using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Stonephonia
{
    class Character : Entity
    {
        public Rock mCurrentRock;
        public float mPushVelocity = 0.0f;

        public Character(Texture2D texture)
        : base(texture)
        {
        }

        public void Update(Rock[] rock)
        {
            CalculateMovement();
            PushRock(rock);
            Move(rock);
        }

        private void Move(Rock[] rock)
        {
            if (!InputManager.KeyHeld(Keys.Right) && !InputManager.KeyHeld(Keys.Left))
            {
                mVelocity = 0.0f;
            }
            mVelocity = Math.Clamp(mVelocity, -mMaxSpeed, mMaxSpeed);
            mPosition.X += mVelocity;

            //if (mCurrentRock != null)
            //{
            //    mCurrentRock.mPosition.X = mPosition.X + mTexture.Width;
            //}

            CollideWithScreenEdge();
        }

        private void CalculateMovement()
        {
            int inputDir = 0;
            if (InputManager.KeyHeld(Keys.Right))
            {
                inputDir += 1;
            }
            if (InputManager.KeyHeld(Keys.Left))
            {
                inputDir -= 1;
            }
            mVelocity = inputDir * mMaxSpeed;
        }

        // AABB Collision
        private bool Collision(float amountToMove, Rock rock)
        {
            return (mCollisionRect.Right + amountToMove > rock.mCollisionRect.Left &&
                   mCollisionRect.Left < rock.mCollisionRect.Left) ||
                   (mCollisionRect.Left + amountToMove < rock.mCollisionRect.Right &&
                   mCollisionRect.Right > rock.mCollisionRect.Right);
        }

        private void TargetClosestRock(Rock[] rock)
        {
            if (!Collision(0, rock[0]) && Collision(mVelocity, rock[0]))
            {
                mCurrentRock = rock[0];
            }
            else if (!Collision(0, rock[1]) && Collision(mVelocity, rock[1]))
            {
                mCurrentRock = rock[1];
            }
            else if (!Collision(0, rock[2]) && Collision(mVelocity, rock[2]))
            {
                mCurrentRock = rock[2];
            }
            else if (!Collision(0, rock[3]) && Collision(mVelocity, rock[3]))
            {
                mCurrentRock = rock[3];
            }
        }

        private void ReleaseRock()
        {
            PositionToIntGrid();
            mCurrentRock.mVelocity = 0.0f;
            mPushVelocity = 0.0f;
            mCurrentRock = null;
        }

        private void PushRock(Rock[] rock)
        {
            if (InputManager.KeyHeld(Keys.Space))
            {
                // If player has a target rock but is not colliding with it, clear current target.
                if (mCurrentRock != null && !Collision(Math.Sign(mVelocity), mCurrentRock))
                {
                    ReleaseRock();
                }
                // If no current rock, target closest rock that isn't already colliding with player
                if (mCurrentRock == null)
                {
                    TargetClosestRock(rock);
                }
                // Push targeted rock
                if (mCurrentRock != null)
                {
                    while (!Collision(Math.Sign(mVelocity), mCurrentRock))
                    {
                        mPosition.X += Math.Sign(mVelocity);
                    }
                    mPushVelocity += Math.Sign(mVelocity) * mCurrentRock.mAcceleration;
                    mPushVelocity = Math.Clamp(mPushVelocity, -mCurrentRock.mMaxSpeed, mCurrentRock.mMaxSpeed);

                    //KeepRockOnScreen();

                    mVelocity = mPushVelocity;
                    mCurrentRock.mPosition.X = mPosition.X + mTexture.Width;
                    



                    //mCurrentRock.mVelocity = mPushVelocity;
                }
            }
            if (mCurrentRock != null && InputManager.KeyReleased(Keys.Space))
            {
                ReleaseRock();
            }
        }

        private void KeepRockOnScreen()
        {
            // Deccelerate rock as it approaches screen edge
            float decceleration = 0.2f;

            if (mVelocity > 0.0f && mCurrentRock.mPosition.X > GamePort.renderSurface.Bounds.Right - 70)
            {
                mPushVelocity -= (Math.Sign(mVelocity) * mCurrentRock.mMaxSpeed) * decceleration;
                if (mPushVelocity <= 0.0f) { mPushVelocity = 0.0f; }
            }
            if (mVelocity < 0.0f && mCurrentRock.mPosition.X < GamePort.renderSurface.Bounds.X + 70)
            {
                mPushVelocity -= (Math.Sign(mVelocity) * mCurrentRock.mMaxSpeed) * decceleration;
                if (mPushVelocity <= 0.0f) { mPushVelocity = 0.0f; }
            }
        }

        private void PositionToIntGrid()
        {
            // Round position float values to ints to realign to int grid
            if (mCurrentRock.mPosition.X > mPosition.X)
            {
                mPosition.X = (int)(mPosition.X + 0.5f);
                mCurrentRock.mPosition.X = (int)(mCurrentRock.mPosition.X + 0.5f);
            }
            else
            {
                mPosition.X = (int)(mPosition.X - 0.5f);
                mCurrentRock.mPosition.X = (int)(mCurrentRock.mPosition.X - 0.5f);
            }
        }

        private void CollideWithScreenEdge()
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
