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
        Texture2D mDefaultbg;
        Fader mWhiteSquare;
        Fader[] mText;
        FaderManager mTextFader, mSquareFader;
        CutsceneSprite mFairy, mPlayer;
        float mBlackSquareAlpha = 1.0f;
        Vector2 mPlayerPos = ScreenManager.pusher.mPosition;
        Vector2 mFairyPos = new Vector2(600, 300);
        ScreenTransition mScreenTransition;
        Rock[] mRocks;

        int fairySpawn = 3;
        int fairyDespawn = 24;
        int playerSpawn = 9;
        int changeScreen = 28;
        int textOne = 7;
        int textTwo = 16;

        public IntroCutscene()
        {
            SoundManager.StartAmbientTrack();
        }

        public override void LoadAssets()
        {
            ScreenManager.pusher.Load(ScreenManager.contentMgr);
            mRocks = Rock.Load();
            mRoomTimer = new Timer();
            mDefaultbg = ScreenManager.contentMgr.Load<Texture2D>("Sprites/default_bg");
            mWhiteSquare = new Fader(ScreenManager.contentMgr.Load<Texture2D>("Sprites/white_square"), Vector2.Zero, Color.White);
            mSquareFader = new FaderManager(new Fader[1] { mWhiteSquare });
            mScreenTransition = new ScreenTransition();

            mText = new Fader[]
            {
                new Fader(ScreenManager.font, "\"Here we are\"", new Vector2(0, 600), Colours.lightBlue),
                new Fader(ScreenManager.font, "\"You must document the passing of time\"", new Vector2(0, 600), Colours.lightBlue)
            };
            mTextFader = new FaderManager(mText);

            mFairy = new CutsceneSprite(mFairyPos, fairySpawn, fairyDespawn, CutsceneSprite.State.inactive,
             new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/fairy_sheet"),
             new Point(128, 128), new Point(0, 0), new Point(4, 1), 200, Color.White, false),
             new Fader(ScreenManager.contentMgr.Load<Texture2D>("Sprites/fairy_fader"), new Vector2(mFairyPos.X, mFairyPos.Y + 4), Color.White));
            mFairy.mSound = SoundManager.SFXType.fairy;
            mFairy.mVolume = 1.0f;

            mPlayer = new CutsceneSprite(mPlayerPos, playerSpawn, 1000000, CutsceneSprite.State.inactive,
            new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/player_stage_one"),
            new Point(60, 84), new Point(0, 0), new Point(2, 1), 200, Color.White, false),
            new Fader(ScreenManager.contentMgr.Load<Texture2D>("Sprites/player_fader"), mPlayerPos, Color.White));
            mPlayer.mSound = SoundManager.SFXType.pad;
            mPlayer.mVolume = 0.5f;
        }

        public override void UnloadAssests()
        {
        }
        private void ShowText()
        {
            mTextFader.FadeInAndOut(mText[0], 0.02f, 0.03f, textOne, 6);
            mTextFader.FadeInAndOut(mText[1], 0.02f, 0.03f, textTwo, 8);
        }

        public override void Update(GameTime gameTime)
        {
            mRoomTimer.Update(gameTime);
            mSquareFader.Update(gameTime);
            mTextFader.Update(gameTime);
            mFairy.Update(gameTime, true);
            mPlayer.Update(gameTime, true);
            ShowText();

            float fadeIn = 0.008f;
            float fadeOut = 0.04f;

            if (mRoomTimer.mCurrentTime > changeScreen)
            {
                mScreenTransition.FadeToGamePlay(fadeIn, fadeOut, this);
                if (!mScreenTransition.mFadingIn) { mBlackSquareAlpha = 0.0f; }
            }

            foreach (Rock rock in mRocks)
            {
                rock.UpdateReflection(gameTime);
            }

            ScreenManager.pusher.mReflection.Update(gameTime, ScreenManager.pusher.mPosition.X - 12);
        }

        public override void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mDefaultbg, Vector2.Zero, Color.White);
            ScreenManager.pusher.mReflection.Draw(spriteBatch);

            foreach (Rock rock in mRocks)
            {
                rock.Draw(spriteBatch);
                rock.DrawReflection(spriteBatch);
            }
            spriteBatch.Draw(ScreenManager.blackSquare, Vector2.Zero, Color.White * mBlackSquareAlpha);
            mTextFader.DrawString(spriteBatch);
            mFairy.Draw(spriteBatch);
            mSquareFader.Draw(spriteBatch);
            mScreenTransition.Draw(spriteBatch);
            mPlayer.Draw(spriteBatch);

        }
    }
}
