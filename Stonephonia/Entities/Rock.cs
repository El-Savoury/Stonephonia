using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Stonephonia
{
    public class Rock : Entity
    {
        private bool mIsColliding = false;
        //private int mCounter = 150;
        private int mInterval;
        private Reflection mReflection;
        private SoundManager.SFXType mSound;
        private float mVolume = 0.0f;
        private bool mPlaying = false;
        private Timer mTimer = new Timer();
        
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
            rock[0] = new Rock(new Vector2(150, 452), 32, 4, 0.03f)
            {
                mSprite = new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/rock_sheet"),
                new Point(100, 84), new Point(0, 0), new Point(2, 2), 200, Color.White),

                mReflection = new Reflection(new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/tiny_rock_reflection"),
               new Point(100, 40), new Point(3, 0), new Point(4, 1), 150, Color.White), new Vector2(150, ScreenManager.pusher.mPosition.Y + 112)),

                mInterval = 200,
                //mCounter = 181,
                mSound = SoundManager.SFXType.plinks
            };
            rock[1] = new Rock(new Vector2(300, 452), 20, 3, 0.02f)
            {
                mSprite = new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/rock_zero_sheet"),
                new Point(96, 84), new Point(0, 0), new Point(2, 2), 200, Color.White),

                mReflection = new Reflection(new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/tiny_rock_reflection"),
               new Point(100, 40), new Point(0, 0), new Point(4, 1), 150, Color.White), Vector2.Zero),

                mInterval = 200,
                //mCounter = 0,
                mSound = SoundManager.SFXType.vamp
            };
            rock[2] = new Rock(new Vector2(400, 424), 12, 2, 0.008f)
            {
                mSprite = new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/rock_tall_sheet"),
                new Point(96, 112), new Point(0, 0), new Point(2, 2), 200, Color.White),

                mReflection = new Reflection(new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/tiny_rock_reflection"),
               new Point(100, 40), new Point(0, 0), new Point(4, 1), 150, Color.White), Vector2.Zero),

                mInterval = 200,
                // mCounter = 181,
                mSound = SoundManager.SFXType.square
            };
            rock[3] = new Rock(new Vector2(500, 452), 0, 1, 0.005f)
            {
                mSprite = new Sprite(ScreenManager.pixel, new Point(96, 84), new Point(0, 0), new Point(1, 1), 15, Color.LightBlue),

                mReflection = new Reflection(new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/tiny_rock_reflection"),
               new Point(100, 40), new Point(0, 0), new Point(4, 1), 150, Color.White), Vector2.Zero),

                mInterval = 200,
                //mCounter = 181,
                mSound = SoundManager.SFXType.bass
            };
            return rock;
        }

        private void ChangeState()
        {
            switch (mCurrentState)
            {
                case State.inactive:
                    ResetRock();
                    break;

                case State.active:
                    //mCounter++;
                    //Sing();
                    break;

                case State.pushed:
                    //mCounter++;
                    Sing();
                    break;

                default:
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

        private void ActivateNearPlayer(Pusher pusher)
        {
            Rock self = this;
            mIsColliding = CollideWithPlayer(pusher);

            if (pusher.mCurrentState == Pusher.State.push && self == pusher.mCurrentRock)
            {
                mCurrentState = State.pushed;
            }
            else if (mIsColliding)
            {
                //mCurrentState = State.active;
            }
            else if (!mIsColliding && mSprite.mAnimationComplete)
            {
                mCurrentState = State.inactive;
            }
        }

        private void Sing()
        {
            if (mSprite.mAnimationComplete)
            {
                mSprite.ResetAnimation(new Point(0, 0));
            }

            //FadeVoulme(true, 0.03f);
        }

        //private void playSound()
        //{
        //    if (mCounter > 10) //mInterval)
        //    {
        //        SoundManager.PlaySFX(mSound, mVolume);
        //        mCounter = 0;
        //        mSprite.ResetAnimation(new Point(0, 1));
        //    }
        //}



        private void LoopSound()
        {
            if (mTimer.mCurrentTime > 7.0588)
            {
                SoundManager.PlaySFX(mSound, 1.0f);
                mTimer.Reset();
            }
        }


        private void FadeVoulme(bool fadeUp, float fadeAmount)
        {
            if (fadeUp) { mVolume += fadeAmount; }
            else { mVolume -= fadeAmount; }
            mVolume = Math.Clamp(mVolume, 0.0f, 1.0f);
        }

        private void ResetRock()
        {
            FadeVoulme(false, 0.1f);
            //mCounter = mInterval + 1;
            if (mSprite.mAnimationComplete) { mSprite.ResetAnimation(new Point(0, 0)); }
        }

        public void Update(GameTime gameTime, Pusher pusher)
        {
            mTimer.Update(gameTime);
            LoopSound();

            UpdateReflection(gameTime);

            if (pusher.mCurrentState != Pusher.State.dead)
            {
                ActivateNearPlayer(pusher);
                ChangeState();
            }
            mSprite.Update(gameTime, false);

            base.Update(gameTime);
        }

        public void UpdateReflection(GameTime gameTime)
        {
            mReflection.Update(gameTime, mPosition.X);
        }

        public void DrawReflection(SpriteBatch spriteBatch)
        {
            mReflection.Draw(spriteBatch);
        }
    }
}
