using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Stonephonia
{
    public class Pusher : Entity
    {
        public Rock mCurrentRock;
        private float mPushVelocity = 0.0f;
        public bool mDirection = true;
        private float mStopSpeed;
        int mAgeTime = 1;

        private Texture2D[] mPlayerTextures;
        private Sprite mDeathSprite;
        public  Reflection mReflection;

        public Pusher(Vector2 position, int collisionOffset, int maxSpeed)
                : base(position, collisionOffset, maxSpeed)
        {
        }

        public void Load(ContentManager content)
        {
            mPlayerTextures = new Texture2D[4]
            {
                content.Load<Texture2D>("Sprites/player_stage_two"),
                content.Load<Texture2D>("Sprites/player_stage_three"),
                content.Load<Texture2D>("Sprites/player_stage_four"),
                content.Load<Texture2D>("Sprites/player_death")
            };

            mDeathSprite = new Sprite(mPlayerTextures[3],
                new Point(100, 84), new Point(0, 0), new Point(4, 2), 500, Color.White, false);

            mReflection = new Reflection(new Sprite(content.Load<Texture2D>("Sprites/player_reflection"),
               new Point(80, 80), new Point(0, 0), new Point(4, 1), 150, Color.White),
               new Vector2(ScreenManager.pusher.mPosition.X - 12, ScreenManager.pusher.mPosition.Y + 112));
        }

        public enum State
        {
            idle,
            walk,
            push,
            dead
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
                    if (mDirection) { mSprite.mCurrentFrame.Y = mCurrentRock.mPosition.X + mCurrentRock.mSprite.mFrameSize.X < 650 ? 4 : 6; }
                    else { mSprite.mCurrentFrame.Y = mCurrentRock.mPosition.X < 150 ? 7 : 5; }
                    break;

                case State.dead:
                    break;

                default:
                    break;
                    //// TODO: MAKE PUSH ANIMATION SPEED PERCENTAGE BASED
                    //if (mVelocity < 1) { mSprite.mTimePerFrame = 400; }
                    //else if (mVelocity < 1.5) { mSprite.mTimePerFrame = 300; }
                    //else if (mVelocity < 2) { mSprite.mTimePerFrame = 200; }
                    //break;
            }
            mSprite.Update(gameTime, true);
        }

        public void KillPlayer(GameTime gameTime)
        {
            ChangeState(State.dead);
            mSprite.SetVisible(false);
            mDeathSprite.SetVisible(true);
            mDeathSprite.Update(gameTime, false);
            if (mDirection) { mDeathSprite.mCurrentFrame.Y = 0; }
            else { mDeathSprite.mCurrentFrame.Y = 1; }
            mMaxSpeed = 0;
        }

        private void UpdateReflection(GameTime gameTime)
        {
            if (mCurrentState == State.push) { mReflection.mSprite.mTexture = ScreenManager.contentMgr.Load<Texture2D>("Sprites/push_reflection"); }
            else { mReflection.mSprite.mTexture = ScreenManager.contentMgr.Load<Texture2D>("Sprites/player_reflection"); }

            mReflection.mSprite.mCurrentFrame.Y = mDirection ? 0 : 1;
            mReflection.Update(gameTime, mPosition.X - 12);
        }

        public void FadeOutReflection(float fadeAmount)
        {
            mReflection.Fade(fadeAmount);
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
                    mCurrentRock.mPosition.X = (mCollisionRect.Left + mCollisionRect.Width) - mCurrentRock.mCollisionOffset;
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
                mStopSpeed = CalcStopSpeed(mCurrentRock.mPosition.X + mCurrentRock.mSprite.mFrameSize.X, rightStop, rightStart);
                mVelocity = Math.Clamp(mVelocity, 0, mStopSpeed);
            }
        }

        private void AgePlayer(Texture2D[] playerTextures, Timer gameTimer, int timeInterval)
        {
            if (gameTimer.mCurrentTime > timeInterval && Array.IndexOf(playerTextures, mSprite.mTexture) < playerTextures.Length - 2)
            {
                mSprite.mTexture = playerTextures[Array.IndexOf(playerTextures, mSprite.mTexture) + 1];
                mSprite.mTimePerFrame += 25;
                mMaxSpeed--;
                gameTimer.Reset();
            }
        }

        public void Reset()
        {
            mSprite = mSprite = new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/player_stage_one"),
                new Point(60, 84), new Point(0, 0), new Point(2, 1), 200, Color.White, true);
            mSprite.SetVisible(true);
            mPosition = new Vector2(20, 452);
            mMaxSpeed = 4;
            mDirection = true;
        }

        public void Update(GameTime gameTime, Timer gameTimer, Rock[] rock)
        {
            CalculateMovement();
            TargetClosestRock(rock);
            CollideWithRock();
            GetRockSpeed();
            StopRock(80, 150, 720, 650);
            Move();
            PushRock();
            AgePlayer(mPlayerTextures, gameTimer, mAgeTime);
            SetAnimation(gameTime);
            UpdateReflection(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            mReflection.Draw(spriteBatch);
            mDeathSprite.Draw(spriteBatch, new Vector2(mPosition.X - 15, mPosition.Y));
            base.Draw(spriteBatch);
        }


        public void DrawDebug(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(ScreenManager.pixel, new Rectangle((int)mPosition.X, (int)mPosition.Y, mCollisionRect.Width, mCollisionRect.Height), Color.Red * 0.3f);

            spriteBatch.DrawString(ScreenManager.font, $"mPosition: {mPosition.X}", new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(ScreenManager.font, $"mDirection: {mDirection}", new Vector2(300, 0), Color.Red);
            spriteBatch.DrawString(ScreenManager.font, $"mVelocity: {mVelocity}", new Vector2(0, 20), Color.White);
            spriteBatch.DrawString(ScreenManager.font, $"mPushVelocity: {mPushVelocity}", new Vector2(300, 20), Color.White);
            spriteBatch.DrawString(ScreenManager.font, $"state: {mCurrentState}", new Vector2(600, 20), Color.White);
        }
    }
}
