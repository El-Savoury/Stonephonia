using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stonephonia.Effects;

namespace Stonephonia.Screens
{
    public class IntroCutscene : Screen
    {
        Timer mRoomTimer;
        AnimatedTextFader mTextFader;
        Sprite mPlayerSprite, mFairySprite;
        Fader mPlayerFader, mFairyFader, mBlackSquareFader;
        Texture2D[] mBackgroundTextures;

        Vector2 mFairyPosition = new Vector2(600, 300);

        public override void LoadAssets()
        {
            mRoomTimer = new Timer();
            mTextFader = new AnimatedTextFader(ScreenManager.font, "Here we are", 100f, 0.5f, 0f);

            mPlayerSprite = ScreenManager.pusher.mSprite;
            mFairySprite = new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/fairy_sheet"),
                         new Point(128, 128), new Point(0, 0), new Point(4, 1), 200, Color.White, 1.0f);
            mPlayerSprite.SetVisible(false);
            mFairySprite.SetVisible(false);

            mPlayerFader = new Fader(ScreenManager.contentMgr.Load<Texture2D>("Sprites/player_fader"), ScreenManager.pusher.mPosition, Color.White);
            mFairyFader = new Fader(ScreenManager.contentMgr.Load<Texture2D>("Sprites/fairy_fader"), mFairyPosition, Color.White);
            mBlackSquareFader = new Fader(ScreenManager.contentMgr.Load<Texture2D>("Sprites/black_square"), Vector2.Zero, Color.White, 1.0f);

            mBackgroundTextures = new Texture2D[]
           {
                ScreenManager.contentMgr.Load<Texture2D>("Sprites/background_trees"),
                ScreenManager.contentMgr.Load<Texture2D>("Sprites/background_bushes"),
           };
        }

        public override void UnloadAssests()
        {
        }

        public override void Update(GameTime gameTime)
        {
            mRoomTimer.Update(gameTime);
            mBlackSquareFader.Update(gameTime);
            mFairySprite.Update(gameTime, true);
            mPlayerSprite.Update(gameTime, true);
            mTextFader.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds);
            mPlayerFader.Update(gameTime);


            if (mRoomTimer.mCurrentTime > 10)
            {
                //mBlackSquareFader.SmoothFade(false, 0.008f);
                mPlayerSprite.SetVisible(true);
                mPlayerFader.SmoothFade(false, 0.06f);
            }

            if (mRoomTimer.mCurrentTime > 3)
            {
                mPlayerFader.SmoothFade(true, 0.03f);
                //playerFader.GlowFade(true, 0.04f, 0.02f, 0.5f, 0.2f);
                // playerFader.StagedFade(true, 0.2f, 0.3f);
            }


        }

        public override void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            foreach (Texture2D background in mBackgroundTextures)
            {
                spriteBatch.Draw(background, new Rectangle(0, 0, 800, 800), Color.White);
            }

            mBlackSquareFader.DrawSprite(spriteBatch);

            mFairySprite.Draw(spriteBatch, mFairyPosition);
            mPlayerSprite.Draw(spriteBatch, ScreenManager.pusher.mPosition);

            mPlayerFader.DrawSprite(spriteBatch);

            mTextFader.Draw(spriteBatch, new Vector2(0, 600), 10, true, ScreenManager.lightBlue);



            spriteBatch.DrawString(ScreenManager.font, $"mCurrentTime = {mRoomTimer.mCurrentTime}", Vector2.Zero, Color.White);
        }
    }
}
