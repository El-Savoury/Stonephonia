using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Stonephonia
{
    public class Pusher : Entity
    {
        public Rock mCurrentRock;
        private float mPushVelocity = 0.0f;
        private bool mDirection = true;
        private float mStopSpeed;

        private Texture2D[] playerTextures = new Texture2D[3]
            {
                ScreenManager.contentMgr.Load<Texture2D>("Sprites/player_stage_two_sheet"),
                ScreenManager.contentMgr.Load<Texture2D>("Sprites/player_stage_three_sheet"),
                ScreenManager.contentMgr.Load<Texture2D>("Sprites/player_stage_four_sheet")
            };

        public Pusher(Vector2 position, int collisionOffset, int maxSpeed)
                : base(position, collisionOffset, maxSpeed)
        {
        }

        public enum State
        {
            idle,
            walk,
            push
        }

        public State mCurrentState = State.idle;

        private void ChangeState(State state)
        {
            mCurrentState = state;
        }

        private void SetAnimation(GameTime gameTime)
        {
            switch (mCurrentState)
            {
                case State.idle:
                    if (mDirection) { mSprite.mCurrentFrame.Y = 0; }
                    else { mSprite.mCurrentFrame.Y = 1; }
                    break;

                case State.walk:
                    if (mDirection) { mSprite.mCurrentFrame.Y = 2; }
                    else { mSprite.mCurrentFrame.Y = 3; }
                    break;

                case State.push:
                    if (mDirection) { mSprite.mCurrentFrame.Y = 4; }
                    else { mSprite.mCurrentFrame.Y = 5; }

                    //// TODO: MAKE PUSH ANIMATION SPEED PERCENTAGE BASED
                    //if (mVelocity < 1) { mSprite.mTimePerFrame = 400; }
                    //else if (mVelocity < 1.5) { mSprite.mTimePerFrame = 300; }
                    //else if (mVelocity < 2) { mSprite.mTimePerFrame = 200; }
                    break;
            }
            mSprite.Update(gameTime, true);
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

            if (mVelocity > 0) { mDirection = true; }
            else if (mVelocity < 0) { mDirection = false; }

            // Prevent player from moving when no keys are pressed or both are held
            if (InputManager.KeyReleased(Keys.Right) && InputManager.KeyReleased(Keys.Left) ||
                InputManager.KeyHeld(Keys.Right) && InputManager.KeyHeld(Keys.Left))
            {
                mVelocity = 0.0f;
            }
            mVelocity = Math.Clamp(mVelocity, -mMaxSpeed, mMaxSpeed);
        }

        private void Move()
        {
            mPosition.X += mVelocity;

            if (mVelocity == 0 && mCurrentState != State.push)
            {
                ChangeState(State.idle);
            }
            else if (mVelocity != 0 && mCurrentState != State.push)
            {
                ChangeState(State.walk);
            }

            KeepEntityOnScreen();
        }

        private void TargetClosestRock(Rock[] rock)
        {
            if (mCurrentState != State.push)
            {
                for (int i = 0; i < rock.Length; i++)
                {
                    if (!Collision(0, rock[i]) && Collision(mVelocity, rock[i]))
                    {
                        mCurrentRock = rock[i];
                    }
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
                if (mCurrentRock.mMaxSpeed > mMaxSpeed)
                {
                    mPushVelocity = Math.Clamp(mPushVelocity, -mMaxSpeed, mMaxSpeed);
                }
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
                    mCurrentRock.mPosition.X = /*mPosition.X*/ (mCollisionRect.Left + mCollisionRect.Width) - mCurrentRock.mCollisionOffset;
                }
                else if (mVelocity < 0)
                {
                    mCurrentRock.mPosition.X = (mCollisionRect.Left - mCurrentRock.mCollisionRect.Width) - mCurrentRock.mCollisionOffset;
                }
            }
        }

        private void ReleaseRock()
        {
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
            if (mCollisionRect.X <= GamePort.renderSurface.Bounds.X)
            {
                mPosition.X = GamePort.renderSurface.Bounds.X - mCollisionOffset;
                mCurrentState = State.idle;
            }
            if (mCollisionRect.Right >= GamePort.renderSurface.Bounds.Right)
            {
                mPosition.X = GamePort.renderSurface.Bounds.Right - mCollisionRect.Width - mCollisionOffset;
                mCurrentState = State.idle;
            }
        }

        private float CalcStopSpeed(float rockSide, int stopPoint, int startPoint)
        {
            float v = mCurrentRock.mMaxSpeed;
            float x = rockSide;
            int d1 = stopPoint;
            int d2 = startPoint;

            float sum1 = v / (d2 - d1) * x;
            float sum2 = v * (d1 / (d2 - d1));

            return sum1 - sum2;
        }

        private void StopRock(int leftStop, int leftStart, int rightStop, int rightStart)
        {
            if (mCurrentState == State.push && mVelocity < 0 && mCurrentRock.mPosition.X < leftStart)
            {
                mStopSpeed = -CalcStopSpeed(mCurrentRock.mPosition.X, leftStop, leftStart);
                mVelocity = Math.Clamp(mVelocity, mStopSpeed, 0);
            }
            else if (mCurrentState == State.push && mVelocity > 0 && mCurrentRock.mPosition.X + mCurrentRock.mSprite.mFrameSize.X > rightStart)
            {
                mStopSpeed = CalcStopSpeed(mCurrentRock.mPosition.X, rightStop, rightStart);
                //mVelocity = Math.Clamp(mVelocity, mStopSpeed, mMaxSpeed);
                //mVelocity = mStopSpeed;/*Math.Clamp(CalcStopSpeed(mPosition.X + mCurrentRock.mSprite.mFrameSize.X, rightStop, rightStart), 0, mMaxSpeed);*/
            }
        }

        private void AgePlayer(Texture2D[] playerTextures, Timer gameTimer, int timeInterval)
        {
            if (gameTimer.mCurrentTime > timeInterval && Array.IndexOf(playerTextures, mSprite.mTexture) < playerTextures.Length - 1)
            {
                mSprite.mTexture = playerTextures[Array.IndexOf(playerTextures, mSprite.mTexture) + 1];
                mSprite.mTimePerFrame += 25;
                mMaxSpeed--;
                gameTimer.Reset();
            }
        }

        public void Update(GameTime gameTime, Timer gameTimer, Rock[] rock)
        {
            CalculateMovement();
            TargetClosestRock(rock);
            CollideWithRock();
            GetRockSpeed();
            StopRock(80, 150, 720, 600);
            Move();
            PushRock();
            AgePlayer(playerTextures, gameTimer, 45);
            SetAnimation(gameTime);

            base.Update(gameTime);
        }

        public void DrawDebug(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(ScreenManager.pixel, new Rectangle((int)mPosition.X, (int)mPosition.Y, mCollisionRect.Width, mCollisionRect.Height), Color.Red * 0.3f);

            spriteBatch.DrawString(ScreenManager.font, $"mPosition: {mPosition.X}", new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(ScreenManager.font, $"currentState: {mCurrentState}", new Vector2(300, 0), Color.White);
            spriteBatch.DrawString(ScreenManager.font, $"mVelocity: {mVelocity}", new Vector2(0, 20), Color.White);
            spriteBatch.DrawString(ScreenManager.font, $"mPushVelocity: {mPushVelocity}", new Vector2(300, 20), Color.White);
            spriteBatch.DrawString(ScreenManager.font, $"stopspeed: {mStopSpeed}", new Vector2(600, 20), Color.White);
        }
    }
}
