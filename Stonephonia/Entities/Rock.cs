using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Stonephonia.Screens;

namespace Stonephonia
{
    public class Rock : Entity
    {
        private bool mIsColliding = false;
        //private int mCounter = 150;
        private int mInterval;
        private Reflection mReflection;
        private SoundManager.SFXType mSound;
        private SoundEffectInstance mInstance;
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
        private State mPreviousState;

        public Rock(Vector2 position, int collisionOffset, int maxSpeed, float acceleration)
            : base(position, collisionOffset, maxSpeed, acceleration)
        {
            OnActivate();
        }

        private void OnActivate()
        {
            mTimer.mCurrentTime = 8;
        }

        public static Rock[] Load()
        {
            Rock[] rock = new Rock[4];
            rock[0] = new Rock(new Vector2(500, 432), 16, 1, 0.005f)
            {
                mSprite = new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/rock_4"),
                new Point(128, 104), new Point(0, 0), new Point(2, 2), 200, Color.White),

                mReflection = new Reflection(new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/rock_4_reflection"),
             new Point(136, 84), new Point(3, 0), new Point(4, 1), 150, Color.White), new Vector2(500, ScreenManager.pusher.mPosition.Y + 112)),

                mInterval = 200,
                //mCounter = 181,
                mSound = SoundManager.SFXType.bass
            };
            rock[1] = new Rock(new Vector2(300, 452), 20, 3, 0.02f)
            {
                mSprite = new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/rock_zero_sheet"),
                   new Point(96, 84), new Point(0, 0), new Point(2, 2), 200, Color.White),

                mReflection = new Reflection(new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/rock_test_reflection"),
                  new Point(96, 64), new Point(0, 0), new Point(4, 1), 150, Color.White), new Vector2(300, ScreenManager.pusher.mPosition.Y + 112)),

                mInterval = 200,
                //mCounter = 0,
                mSound = SoundManager.SFXType.vamp
            };
            rock[2] = new Rock(new Vector2(400, 424), 20, 2, 0.008f)
            {
                mSprite = new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/rock_tall_sheet"),
                new Point(92, 112), new Point(0, 0), new Point(2, 2), 200, Color.White),

                mReflection = new Reflection(new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/rock_tall_reflection"),
               new Point(120, 120), new Point(2, 0), new Point(4, 1), 150, Color.White), new Vector2(400, ScreenManager.pusher.mPosition.Y + 112)),

                mInterval = 200,
                // mCounter = 181,
                mSound = SoundManager.SFXType.square
            };
            rock[3] = new Rock(new Vector2(150, 452), 32, 4, 0.03f)
            {
                mSprite = new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/rock_sheet"),
             new Point(96, 84), new Point(0, 0), new Point(2, 2), 200, Color.White),

                mReflection = new Reflection(new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/tiny_rock_reflection"),
            new Point(100, 40), new Point(3, 0), new Point(4, 1), 150, Color.White), new Vector2(150, ScreenManager.pusher.mPosition.Y + 112)),

                mInterval = 200,
                //mCounter = 181,
                mSound = SoundManager.SFXType.plinks
            };

            return rock;
        }

        private void ChangeState()
        {
            switch (mCurrentState)
            {
                case State.inactive:
                    ResetRock();
                    mPlaying = false;
                    break;

                case State.active:
                    mPlaying = true;
                    Sing();
                    break;

                case State.pushed:
                    mPlaying = true;
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

        private bool CollideWithRock(Rock[] rocks)
        {
            foreach (Rock rock in rocks)
            {
                if (rock != this)
                {
                    float left = mCollisionRect.Left;
                    float right = mCollisionRect.Right;
                    float rockLeft = rock.mPosition.X;
                    float rockRight = rock.mPosition.X + rock.mSprite.mFrameSize.X;

                    if (rockLeft < right && rockRight > right ||
                        rockRight > left && rockLeft < left ||
                        rockLeft >= left && rockRight <= right)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //private void ActivateCollidingRocks(Rock[] rocks)
        //{
        //    if (mCurrentState == State.active)
        //    {
        //        foreach (Rock rock in rocks)
        //        {
        //            if (CollideWithRock(rock))
        //            {
        //                rock.mCurrentState = State.active;
        //            }
        //        }
        //    }
        //}


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
                mCurrentState = State.active;
            }
            else if (!mIsColliding)
            {
                mCurrentState = State.inactive;
            }
        }

        private void Sing()
        {
            if (mPlaying) { mSprite.mCurrentFrame.Y = 1; }



            //if (mSprite.mAnimationComplete)
            //{
            //    mSprite.ResetAnimation(new Point(0, 0));
            //}

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

        private void TogglePlaying()
        {
            if (mCurrentState == State.active && mPreviousState == State.inactive)
            {
                if (!mPlaying) { mPlaying = true; }
                else if (mPlaying)
                {
                    mPlaying = false;
                    ResetRock();
                }
            }
        }

        private void LoopSound()
        {
            if (mPlaying && mVolume < 1) { FadeVolume(true, 0.04f); }
            else if (!mPlaying && mVolume > 0) { FadeVolume(false, 0.03f); }

            if (mTimer.mCurrentTime > 7.0588)
            {
                mInstance = SoundManager.mSFX[mSound].CreateInstance();
                mInstance.Play();
                mTimer.Reset();
            }
        }

        private void FadeVolume(bool playing, float fadeAmount)
        {
            if (playing) { mVolume += fadeAmount; }
            else { mVolume -= fadeAmount; }
            mVolume = Math.Clamp(mVolume, 0.0f, 1.0f);
        }

        private void ResetRock()
        {
            // if (!mPlaying) { mSprite.ResetAnimation(new Point(0, 0)); }
            mSprite.ResetAnimation(new Point(0, 0));
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
            mSprite.Update(gameTime, true);
            mPreviousState = mCurrentState;
            mInstance.Volume = mVolume;

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
