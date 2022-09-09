using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Stonephonia
{
    class Pusher : Entity
    {
        private Rock mCurrentRock;
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

        private void Move(Rock rock)
        {
            CalculateMovement();
            CollideWithRock(rock);
            GetRockSpeed(rock);

            mVelocity = Math.Clamp(mVelocity, -mMaxSpeed, mMaxSpeed);
            mPosition.X += mVelocity;

            PushRock(rock);
            KeepEntityOnScreen();
        }

        private void CollideWithRock(Rock rock)
        {
            if (!Collision(0, rock) && Collision(mVelocity, rock) && InputManager.KeyHeld(Keys.Space))
            {
                if (CollisionRight(mVelocity, rock))
                {
                    while (mCollisionRect.Right < rock.mCollisionRect.Left)
                    {
                        mPosition.X++;
                    }
                }
                else if (CollisionLeft(mVelocity, rock))
                {
                    while (mCollisionRect.Left > rock.mCollisionRect.Right)
                    {
                        mPosition.X--;
                    }
                }

                ChangeState(State.push);
            }
        }

        private void GetRockSpeed(Rock rock)
        {
            if (mVelocity != 0 && mCurrentState == State.push && InputManager.KeyHeld(Keys.Space))
            {
                mPushVelocity += Math.Sign(mVelocity) * rock.mAcceleration;
                mPushVelocity = Math.Clamp(mPushVelocity, -rock.mMaxSpeed, rock.mMaxSpeed);
                mVelocity = mPushVelocity;
            }
            else
            {
                ReleaseRock();
            }
        }

        private void PushRock(Rock rock)
        {
            if (mCurrentState == State.push && InputManager.KeyHeld(Keys.Space))
            {
                if (mVelocity > 0)
                {
                    rock.mPosition.X = mPosition.X + mCollisionRect.Width;
                }
                else if (mVelocity < 0)
                {
                    rock.mPosition.X = mPosition.X - mCollisionRect.Width;
                }
            }
        }

        private void ReleaseRock()
        {
            ChangeState(State.walk);
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

        private void PositionToIntGrid(Rock rock)
        {
            // Round X position values to ints to realign player and rock to pixel grid
            if (rock.mPosition.X > mPosition.X)
            {
                mPosition.X = (int)(mPosition.X + 0.5f);
                rock.mPosition.X = (int)(rock.mPosition.X + 0.5f);
            }
            else
            {
                mPosition.X = (int)(mPosition.X - 0.5f);
                rock.mPosition.X = (int)(rock.mPosition.X - 0.5f);
            }
        }

        public void Update(GameTime gameTime, Rock rock)
        {
            Move(rock);
            base.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(ScreenManager.contentMgr.Load<Texture2D>("Sprites/sprite_test"), mCollisionRect, Color.Red * 0.3f);

            spriteBatch.DrawString(ScreenManager.font, $"pusherVel: {mVelocity}", new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(ScreenManager.font, $"currentState: {mCurrentState}", new Vector2(0, 15), Color.White);
            spriteBatch.DrawString(ScreenManager.font, $"mPushVelocity: {mPushVelocity}", new Vector2(0, 30), Color.Red);
        }
    }
}
