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
        float mBlackSquareAlpha = 1.0f;
        Vector2 mPlayerPos = ScreenManager.pusher.mPosition;

        int fairySpawn = 3;
        int fairyDespawn = 24;
        int playerRockSpawn = 19;
        int playerRockDespawn = 1000000;
        int whiteSquareFade = 28;
        int changeScreen = 48;

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
             new Fader(ScreenManager.contentMgr.Load<Texture2D>("Sprites/fairy_fader"), new Vector2(mFairyPos.X,mFairyPos.Y + 4), Color.White));

            mPlayerRock = new CutsceneSprite(mPlayerPos, playerRockSpawn, playerRockDespawn, CutsceneSprite.State.inactive,
             new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/player_rock"),
             new Point(60, 84), new Point(0, 0), new Point(1, 2), 200, Color.White, false),
             new Fader(ScreenManager.contentMgr.Load<Texture2D>("Sprites/player_rock_fader"), mPlayerPos, Color.White));
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
            mTextFader.FadeInAndOut(mText[0], 0.02f, 0.03f, 7, 4); 
            mTextFader.FadeInAndOut(mText[1], 0.02f, 0.03f, 13, 4); 
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

        public override void Update(GameTime gameTime)
        {
            mRoomTimer.Update(gameTime);
            mSquareFader.Update(gameTime);
            mTextFader.Update(gameTime);
            mFairy.Update(gameTime, true);
            mPlayerRock.Update(gameTime, true);
            ScreenManager.pusher.Update(gameTime, mRoomTimer, mRocks);
            SetSpriteDirection();
            HidePlayer();
            FadeWhiteSquare();
            ShowText();

            if (mRoomTimer.mCurrentTime > changeScreen) { ScreenManager.ChangeScreen(this, new SplashScreen()); }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mDefaultbg, Vector2.Zero, Color.White);

            foreach (Rock rock in mRocks)
            {
                rock.Draw(spriteBatch);
            }

            spriteBatch.Draw(ScreenManager.blackSquare, Vector2.Zero, Color.White * mBlackSquareAlpha);
            mTextFader.DrawString(spriteBatch);
            mFairy.Draw(spriteBatch);
            mSquareFader.Draw(spriteBatch);
            ScreenManager.pusher.Draw(spriteBatch);
            mPlayerRock.Draw(spriteBatch);
        }
    }
}
