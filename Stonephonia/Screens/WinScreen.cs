using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stonephonia.Managers;
using Stonephonia.Effects;

namespace Stonephonia.Screens
{
    public class WinScreen : Screen
    {
        Timer mRoomTimer;
        Texture2D mDefaultbg;
        Fader mWhiteSquare;
        Fader[] mText;
        FaderManager mTextFader, mSquareFader;
        Rock[] mRocks = GameplayScreen.mRocks;
        CutsceneSprite mFairy, mPlayerRock;
        Vector2 mFairyPos = new Vector2(600, 300);
        Vector2 mPlayerPos = ScreenManager.pusher.mPosition;

        float mBlackSquareAlpha = 1.0f;
        int fairySpawn = 2;
        int fairyDespawn = 19;
        int playerRockSpawn = 15;
        int playerRockDespawn = 1000000;
        int whiteSquareFade = 21;
        int changeScreen = 37;
        int textOne = 4;
        int textTwo = 9;

        public override void LoadAssets()
        {
            mRoomTimer = new Timer();
            mDefaultbg = ScreenManager.contentMgr.Load<Texture2D>("Sprites/default_bg");
            mWhiteSquare = new Fader(ScreenManager.contentMgr.Load<Texture2D>("Sprites/white_square"), Vector2.Zero, Color.White);
            mSquareFader = new FaderManager(new Fader[1] { mWhiteSquare });

            mText = new Fader[]
            {
                new Fader(ScreenManager.font, "\"Well done\"", new Vector2(0, 600), Colours.lightBlue),
                new Fader(ScreenManager.font, "\"Now you may rest\"", new Vector2(0, 600), Colours.lightBlue)
            };
            mTextFader = new FaderManager(mText);

            mFairy = new CutsceneSprite(mFairyPos, fairySpawn, fairyDespawn, CutsceneSprite.State.inactive,
             new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/fairy_sheet"),
             new Point(128, 128), new Point(0, 0), new Point(4, 1), 200, Color.White, false),
             new Fader(ScreenManager.contentMgr.Load<Texture2D>("Sprites/fairy_fader"), new Vector2(mFairyPos.X, mFairyPos.Y + 4), Color.White));
            mFairy.mSound = SoundManager.SFXType.fairy;
            mFairy.mVolume = 0.3f;

            mPlayerRock = new CutsceneSprite(mPlayerPos, playerRockSpawn, playerRockDespawn, CutsceneSprite.State.inactive,
             new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/player_rock"),
             new Point(60, 84), new Point(0, 0), new Point(1, 2), 200, Color.White, false),
             new Fader(ScreenManager.contentMgr.Load<Texture2D>("Sprites/player_rock_fader"), mPlayerPos, Color.White));
            mPlayerRock.mSound = SoundManager.SFXType.rhodes;
            mPlayerRock.mVolume = 0.3f;
        }

        public override void UnloadAssests()
        {
        }

        private void FadeWhiteSquare()
        {
            mSquareFader.FadeInAndOut(mWhiteSquare, 0.03f, 0.02f, whiteSquareFade, 2);
            if (mWhiteSquare.mAlpha >= 1f) { mBlackSquareAlpha = 0.0f; }
        }

        private void ShowText()
        {
            mTextFader.FadeInAndOut(mText[0], 0.02f, 0.03f, textOne, 3);
            mTextFader.FadeInAndOut(mText[1], 0.02f, 0.03f, textTwo, 4);
        }

        private void HidePlayer()
        {
            if (mPlayerRock.mSprite.mAlpha >= 1.0f) { ScreenManager.pusher.mSprite.SetVisible(false); }
            else if (mRoomTimer.mCurrentTime > playerRockSpawn) { ScreenManager.pusher.mSprite.mCurrentFrame.X = 0; }
        }

        private void SetSpriteDirection()
        {
            if (mRoomTimer.mCurrentTime > 3) { ScreenManager.pusher.mDirection = true; }
        }

        private void ChangeScreen()
        {
            if (mRoomTimer.mCurrentTime > changeScreen)
            {
                SoundManager.StopAmbientTrack();
                ScreenManager.pusher.Reset();
                ScreenManager.ChangeScreen(this, new SplashScreen());
            }
        }

        public override void Update(GameTime gameTime)
        {
            mRoomTimer.Update(gameTime);
            mSquareFader.Update(gameTime);
            mTextFader.Update(gameTime);
            if (mRoomTimer.mCurrentTime > fairyDespawn) { mFairy.mMask.mPosition.Y = mFairyPos.Y; }
            mFairy.Update(gameTime, true);
            mPlayerRock.Update(gameTime, true);
            ScreenManager.pusher.Update(gameTime, mRocks, mRoomTimer, null);

            if (mRoomTimer.mCurrentTime > whiteSquareFade) 
            { 
                SoundManager.FadeAmbientTrack(true, 0.002f);
                ScreenManager.pusher.mReflection.mSprite.mTexture = ScreenManager.contentMgr.Load<Texture2D>("Sprites/player_rock_reflection");
                ScreenManager.pusher.mReflection.Update(gameTime, ScreenManager.pusher.mPosition.X);
            }

            SetSpriteDirection();
            HidePlayer();
            FadeWhiteSquare();
            ShowText();

            foreach (Rock rock in mRocks)
            {
                rock.UpdateReflection(gameTime);
            }

            ChangeScreen();
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mDefaultbg, Vector2.Zero, Color.White);

            foreach (Rock rock in mRocks)
            {
                rock.Draw(spriteBatch);
                rock.DrawReflection(spriteBatch);
            }
            ScreenManager.pusher.mReflection.Draw(spriteBatch);
            spriteBatch.Draw(ScreenManager.blackSquare, Vector2.Zero, Color.White * mBlackSquareAlpha);
            mTextFader.DrawString(spriteBatch);
            mFairy.Draw(spriteBatch);
            mSquareFader.Draw(spriteBatch);
            ScreenManager.pusher.Draw(spriteBatch);
            mPlayerRock.Draw(spriteBatch);
        }
    }
}
