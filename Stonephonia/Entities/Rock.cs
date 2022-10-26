using Microsoft.Xna.Framework;

namespace Stonephonia
{
    public class Rock : Entity
    {
        private Timer mSoundTimer = new Timer();
        public int mSoundInterval;

        private enum State
        {
            active,
            inactive,
            moving
        }

        private State mCurrentState = State.inactive;

        public Rock(Vector2 position, int collisionOffset, int maxSpeed, float acceleration)
            : base(position, collisionOffset, maxSpeed, acceleration)
        {
        }

        private void PerformCurrentStateAction(GameTime gameTime)
        {
            switch (mCurrentState)
            {
                case State.active:
                    mSprite.mColour = Color.PowderBlue;
                    mSprite.mCurrentFrame.Y = 1;
                    mSoundTimer.Update(gameTime);
                    Sing(gameTime);
                    break;

                case State.inactive:
                    mSprite.mCurrentFrame.Y = 0;
                    mSoundTimer.Reset();
                    mSprite.mColour = Color.White;
                    break;

                case State.moving:
                    mSprite.mCurrentFrame.Y = 0;
                    mSprite.mColour = Color.Green;
                    break;
            }
        }

        private void ActivateNearPlayer(Pusher pusher)
        {
            float rockLeft = mCollisionRect.Left;
            float rockRight = mCollisionRect.Right;
            float pusherLeft = pusher.mPosition.X;
            float pusherRight = pusher.mPosition.X + pusher.mSprite.mFrameSize.X;

            if (pusher.mCurrentState == Pusher.State.push && mSprite.mTexture == pusher.mCurrentRock.mSprite.mTexture)
            {
                mCurrentState = State.moving;
            }
            else if (pusherLeft < rockRight && pusherRight > rockRight ||
                pusherRight > rockLeft && pusherLeft < rockLeft ||
                pusherLeft >= rockLeft && pusherRight <= rockRight)
            {
                mCurrentState = State.active;
            }
            else
            {
                mCurrentState = State.inactive;
            }
        }

        private void Sing(GameTime gameTime)
        {
            if (mSoundTimer.mCurrentTime < mSoundInterval)
            {
                mSprite.AnimateOnce(gameTime);
            }
            else
            {
                mSprite.ResetAnimation(new Point(0, 0));
                mSoundTimer.Reset();
            }
        }

        public void Update(GameTime gameTime, Pusher pusher)
        {
            ActivateNearPlayer(pusher);
            PerformCurrentStateAction(gameTime);

            base.Update(gameTime);
        }
    }
}
