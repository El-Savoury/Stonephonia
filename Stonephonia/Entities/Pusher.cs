using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Stonephonia
{
    class Pusher : Entity
    {
        public Rock mCurrentRock;
        private float mPushVelocity = 0.0f;

        public Pusher(float xPos, float yPos)
            : base(xPos, yPos)
        {
        }

        private enum State
        {
            idle,
            walk,
            push
        }

        private State mCurrentState = State.idle;

        private void ChangeState(State state)
        {
            mCurrentState = state;
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

            // Prevent player from moving when no keys are pressed
            if (!InputManager.KeyHeld(Keys.Right) && !InputManager.KeyHeld(Keys.Left))
            {
                mVelocity = 0.0f;
            }
        }

        private void Move()
        {
            mVelocity = Math.Clamp(mVelocity, -mMaxSpeed, mMaxSpeed);
            mPosition.X += mVelocity;

            KeepEntityOnScreen();
        }


        private void TargetClosestRock(Rock[] rock)
        {
            if (mCurrentState != State.push)
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
        }

        private void CollideWithRock()
        {
            if (mCurrentRock != null && !Collision(0, mCurrentRock) && Collision(mVelocity, mCurrentRock) && InputManager.KeyHeld(Keys.Space))
            {
                if (CollisionRight(mVelocity, mCurrentRock))
                {
                    while (mCollisionRect.Right < mCurrentRock.mCollisionRect.Left)
                    {
                        mPosition.X++;
                    }
                }
                else if (CollisionLeft(mVelocity, mCurrentRock))
                {
                    while (mCollisionRect.Left > mCurrentRock.mCollisionRect.Right)
                    {
                        mPosition.X--;
                    }
                }
                ChangeState(State.push);
            }
            // If player has a target rock but is not colliding with it, clear current target.
            else if (mCurrentRock != null && !Collision(mVelocity, mCurrentRock) && InputManager.KeyHeld(Keys.Space))
            {
                ReleaseRock();
            }
        }

        private void GetRockSpeed()
        {
            if (mCurrentState == State.push && mVelocity != 0 && InputManager.KeyHeld(Keys.Space))
            {
                mPushVelocity += Math.Sign(mVelocity) * mCurrentRock.mAcceleration;
                mPushVelocity = Math.Clamp(mPushVelocity, -mCurrentRock.mMaxSpeed, mCurrentRock.mMaxSpeed);
                mVelocity = mPushVelocity;
            }
            else if (mCurrentState == State.push && !InputManager.KeyHeld(Keys.Space))
            {
                ReleaseRock();
            }
        }

        private void PushRock()
        {
            if (mCurrentState == State.push && InputManager.KeyHeld(Keys.Space))
            {
                if (mVelocity > 0)
                {
                    mCurrentRock.mPosition.X = mPosition.X + mCollisionRect.Width;
                }
                else if (mVelocity < 0)
                {
                    mCurrentRock.mPosition.X = mPosition.X - mCollisionRect.Width;
                }
            }
        }

        private void ReleaseRock()
        {
            //AllignRocktoGrid();
            ChangeState(State.walk);
            mCurrentRock = null;
            mPushVelocity = 0.0f;
        }

        // AABB Collision
        private bool Collision(float amountToMove, Rock rock)
        {
            return (mCollisionRect.Right + amountToMove > rock.mCollisionRect.Left &&
                   mCollisionRect.Left < rock.mCollisionRect.Left) ||
                   (mCollisionRect.Left + amountToMove < rock.mCollisionRect.Right &&
                   mCollisionRect.Right > rock.mCollisionRect.Right);
        }

        private bool CollisionRight(float amountToMove, Rock rock)
        {
            return (mCollisionRect.Right + amountToMove > rock.mCollisionRect.Left &&
                   mCollisionRect.Left < rock.mCollisionRect.Left);
        }

        private bool CollisionLeft(float amountToMove, Rock rock)
        {
            return (mCollisionRect.Left + amountToMove < rock.mCollisionRect.Right &&
                   mCollisionRect.Right > rock.mCollisionRect.Right);
        }
        private void KeepEntityOnScreen()
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
        private void AllignRocktoGrid()
        {
            // Round X position values to ints to realign player and rock to pixel grid
            //if (mCurrentRock.mPosition.X > mPosition.X)
            //{
            //mPosition.X = (int)(mPosition.X + 0.5f);
            mCurrentRock.mPosition.X = (int)(mCurrentRock.mPosition.X + 0.5f);
            //}
            //else
            //{
            //    mPosition.X = (int)(mPosition.X - 0.5f);
            //    mCurrentRock.mPosition.X = (int)(mCurrentRock.mPosition.X - 0.5f);
            //}
        }

        public void Update(GameTime gameTime, Rock[] rock)
        {
            CalculateMovement();
            TargetClosestRock(rock);
            CollideWithRock();
            GetRockSpeed();
            Move();
            PushRock();
            
            base.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ScreenManager.pixel, mCollisionRect, Color.Red * 0.3f);

            spriteBatch.DrawString(ScreenManager.font, $"mPosition: {mPosition.X}", new Vector2(300, 0), Color.White);
            spriteBatch.DrawString(ScreenManager.font, $"playerVel: {mVelocity}", new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(ScreenManager.font, $"mPushVelocity: {mPushVelocity}", new Vector2(100, 0), Color.White);
            spriteBatch.DrawString(ScreenManager.font, $"currentState: {mCurrentState}", new Vector2(0, 15), Color.Red);
        }
    }
}
