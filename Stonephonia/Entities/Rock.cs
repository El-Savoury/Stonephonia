using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
            pushed
        }

        private State mCurrentState = State.inactive;

        public Rock(Vector2 position, int collisionOffset, int maxSpeed, float acceleration)
            : base(position, collisionOffset, maxSpeed, acceleration)
        {
        }

        public static Rock[] Load()
        {
            Rock[] rock = new Rock[4];
            rock[0] = new Rock(new Vector2(150, 452), 16, 3, 0.03f)
            {
                mSprite = new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/rock_zero_sheet"),
                new Point(92, 84), new Point(0, 0), new Point(2, 1), 200, Color.White),
                mSoundInterval = 4
            };
            rock[1] = new Rock(new Vector2(300, 452), 15, 3, 0.02f)
            {
                mSprite = new Sprite(ScreenManager.pixel, new Point(48, 84), new Point(0, 0), new Point(1, 1), 15, Color.Pink),
                mSoundInterval = 2
            };
            rock[2] = new Rock(new Vector2(400, 452), 0, 2, 0.008f)
            {
                mSprite = new Sprite(ScreenManager.pixel, new Point(64, 84), new Point(0, 0), new Point(1, 1), 15, Color.Orange),
                mSoundInterval = 2
            };
            rock[3] = new Rock(new Vector2(500, 452), 0, 1, 0.005f)
            {
                mSprite = new Sprite(ScreenManager.pixel, new Point(96, 84), new Point(0, 0), new Point(1, 1), 15, Color.LightBlue),
                mSoundInterval = 2
            };
            return rock;
        }

        private void PerformCurrentStateAction(GameTime gameTime)
        {
            switch (mCurrentState)
            {
                case State.active:
                    Sing(gameTime);
                    //mSprite.mColour = Color.Red;
                    break;

                case State.inactive:
                    ResetDefaultSprite();
                    //mSprite.mColour = Color.White;
                    break;

                case State.pushed:
                    Sing(gameTime);
                    //mSprite.mColour = Color.Green;
                    break;
            }
        }

        public bool CollideWithPlayer(Pusher pusher)
        {
            float rockLeft = mCollisionRect.Left;
            float rockRight = mCollisionRect.Right;
            float pusherLeft = pusher.mPosition.X;
            float pusherRight = pusher.mPosition.X + pusher.mSprite.mFrameSize.X;

            if (pusherLeft < rockRight && pusherRight > rockRight ||
                pusherRight > rockLeft && pusherLeft < rockLeft ||
                pusherLeft >= rockLeft && pusherRight <= rockRight)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ActivateNearPlayer(GameTime gameTime, Pusher pusher)
        {
            Rock self = this;

            if (pusher.mCurrentState == Pusher.State.push && self == pusher.mCurrentRock)
            {
                mCurrentState = State.pushed;
            }
            else if (CollideWithPlayer(pusher) && !mSprite.mAnimationComplete)
            {
                mCurrentState = State.active;
            }
            else if (!CollideWithPlayer(pusher) && mSprite.mAnimationComplete)
            {
                mCurrentState = State.inactive;
            }
        }

        private void Sing(GameTime gameTime)
        {
            mSoundTimer.Update(gameTime);

            if (mSoundTimer.mCurrentTime < mSoundInterval && !mSprite.mAnimationComplete)
            {
                mSprite.mCurrentFrame.Y = 1;
                mSprite.Update(gameTime, false);
            }
            else if (mSoundTimer.mCurrentTime < mSoundInterval && mSprite.mAnimationComplete)
            {
                mSprite.mCurrentFrame.Y = 0;
            }
            else
            {
                ResetDefaultSprite();
            }
        }

        private void ResetDefaultSprite()
        {
            mSprite.ResetAnimation(new Point(0, 0));
            mSoundTimer.Reset();
        }

        public void Update(GameTime gameTime, Pusher pusher)
        {
            ActivateNearPlayer(gameTime, pusher);
            PerformCurrentStateAction(gameTime);

            base.Update(gameTime);
        }
    }
}
