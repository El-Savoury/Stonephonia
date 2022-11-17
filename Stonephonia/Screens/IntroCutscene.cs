using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Stonephonia.Effects;
using Stonephonia.Managers;

namespace Stonephonia.Screens
{
    public class IntroCutscene : Screen
    {
        Timer mRoomTimer;
        AnimatedTextFader mTextFader;
        Sprite mPlayerSprite, mFairySprite; /*playerFadeIn, fairyFadeIn;*/
        Fader mPlayerFader, mFairyFader;
        FaderManager mSpriteFaderManager;
        Texture2D[] mBackgroundTextures;
        Rectangle mBlackSquare, mWhiteSquare;
        float mWhiteSquareAlpha = 0.0f;
        float mBlackSquareAlpha = 1.0f;
        Vector2 mFairyPosition = new Vector2(600, 300);

        public override void LoadAssets()
        {
            mRoomTimer = new Timer();
            mPlayerSprite = ScreenManager.pusher.mSprite;
            mFairySprite = new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/fairy_sheet"),
                           new Point(128, 128), new Point(0, 0), new Point(4, 1), 200, Color.White, false);
            mTextFader = new AnimatedTextFader(ScreenManager.font, "You must document the passing of time", 100f, 0.5f, 0f);
            mFairyFader = new Fader(ScreenManager.contentMgr.Load<Texture2D>("Sprites/fairy_fader"), mFairyPosition, Color.White);
            mPlayerFader = new Fader(ScreenManager.contentMgr.Load<Texture2D>("Sprites/player_fader"), ScreenManager.pusher.mPosition, Color.White);
            mSpriteFaderManager = new FaderManager(new Fader[2] { mFairyFader, mPlayerFader });

            mBlackSquare = new Rectangle(0, 0, 800, 800);
            mWhiteSquare = mBlackSquare;

            mBackgroundTextures = new Texture2D[]
            {
                ScreenManager.contentMgr.Load<Texture2D>("Sprites/background_trees"),
                ScreenManager.contentMgr.Load<Texture2D>("Sprites/background_bushes"),
                ScreenManager.canopy
        };

            //playerFadeIn = new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/player_fade_in"),
            //             new Point(60, 84), new Point(0, 0), new Point(12, 1), 100, Color.White, 1.0f);
            //playerFadeIn.SetVisible(false);
            //fairyFadeIn = new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/fairy_fade_sheet"),
            //             new Point(128, 128), new Point(0, 0), new Point(12, 1), 100, Color.White, 1.0f);
            //fairyFadeIn.SetVisible(false);
        }

        public override void UnloadAssests()
        {
        }

        private void SpawnSpriteAnimation(GameTime gameTime, Fader fader, Sprite sprite, float speed1, float speed2, int spawnTime)
        {
            mSpriteFaderManager.FadeInAndOut(fader, speed1, speed2, spawnTime, 2);

            if (fader.mAlpha >= 1.0f)
            {
                sprite.SetVisible(true);
            }
            else if (mRoomTimer.mCurrentTime > spawnTime + 4 && fader.mAlpha <= 0.0f)
            {
                sprite.Update(gameTime, true);
            }
        }

        private void AnimateFairySprite(GameTime gameTime)
        {
            int fairySpawnTime = 2;
            int fairyDespawnTime = 18;
            float fadeSpeed1 = 0.03f;
            float fadeSpeed2 = 0.02f;

            if (mRoomTimer.mCurrentTime >= fairyDespawnTime)
            {
                mFairyFader.mPosition.Y = mFairyPosition.Y;
                fairySpawnTime = fairyDespawnTime;
                fadeSpeed1 = 0.02f;
                fadeSpeed2 = 0.03f;

                if (mFairyFader.mAlpha >= 1.0f) { mFairySprite.SetVisible(false); }
            }
            SpawnSpriteAnimation(gameTime, mFairyFader, mFairySprite, fadeSpeed1, fadeSpeed2, fairySpawnTime);
        }

        private void FadeToWhite()
        {
            if (mRoomTimer.mCurrentTime > 20 && mBlackSquareAlpha <= 0.0f)
            {
                mWhiteSquareAlpha -= 0.008f;
            }
            else if (mRoomTimer.mCurrentTime > 20)
            {
                if (mWhiteSquareAlpha < 1.0f)
                {
                    mWhiteSquareAlpha += 0.04f;
                }
                else if (mWhiteSquareAlpha >= 1.0f)
                {
                    mBlackSquareAlpha = 0.0f;
                }
            }
        }

        //private void AnimatedSpriteFadeIns()
        //{
        //if (mRoomTimer.mCurrentTime > 3)
        //{
        //    fairyFadeIn.SetVisible(true);
        //    fairyFadeIn.Update(gameTime, false);

        //    if (fairyFadeIn.mAnimationComplete)
        //    {
        //        fairyFadeIn.SetVisible(false);
        //        mFairySprite.SetVisible(true);
        //        mFairySprite.Update(gameTime, true);
        //    }
        //}

        //if (mRoomTimer.mCurrentTime > 8)
        //{
        //    playerFadeIn.SetVisible(true);
        //    playerFadeIn.Update(gameTime, false);

        //    if (playerFadeIn.mAnimationComplete)
        //    {
        //        playerFadeIn.SetVisible(false);
        //        mPlayerSprite.SetVisible(true);
        //        mPlayerSprite.Update(gameTime, true);

        //    }
        //}
        //}

        private void NextScreen()
        {
            if (mRoomTimer.mCurrentTime > 21 && mWhiteSquareAlpha <= 0.0f ||
                InputManager.KeyPressed(Keys.Enter))
            {
                ScreenManager.ChangeScreen(new IntroCutscene(), new GameplayScreen());
            }
        }

        public override void Update(GameTime gameTime)
        {
            mRoomTimer.Update(gameTime);
            mSpriteFaderManager.Update(gameTime);
            mTextFader.Update(gameTime, (float)gameTime.ElapsedGameTime.TotalMilliseconds);
            AnimateFairySprite(gameTime);
            SpawnSpriteAnimation(gameTime, mPlayerFader, mPlayerSprite, 0.03f, 0.02f, 8);
            FadeToWhite();
            NextScreen();
        }

        public override void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            foreach (Texture2D background in mBackgroundTextures)
            {
                spriteBatch.Draw(background, new Rectangle(0, 0, 800, 800), Color.White);
            }

            ScreenManager.rock[0].Draw(spriteBatch);

            spriteBatch.Draw(ScreenManager.pixel, mBlackSquare, Color.Black * mBlackSquareAlpha);
            spriteBatch.Draw(ScreenManager.pixel, mWhiteSquare, ScreenManager.lightBlue * mWhiteSquareAlpha);

            mPlayerSprite.Draw(spriteBatch, ScreenManager.pusher.mPosition);
            mFairySprite.Draw(spriteBatch, mFairyPosition);
            mSpriteFaderManager.Draw(spriteBatch);
            mTextFader.Draw(spriteBatch, new Vector2(0, 600), 10, true, ScreenManager.lightBlue);

            //playerFadeIn.Draw(spriteBatch, ScreenManager.pusher.mPosition);
            //fairyFadeIn.Draw(spriteBatch, mFairyPosition);

            // DEBUG STATS
            spriteBatch.DrawString(ScreenManager.font, $"mCurrentTime = {mRoomTimer.mCurrentTime}", Vector2.Zero, Color.White);
        }
    }
}
