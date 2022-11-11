using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stonephonia.Effects;
using Stonephonia.Managers;

namespace Stonephonia.Screens
{
    public class IntroCutscene : Screen
    {
        Timer mRoomTimer;
        AnimatedTextFader mTextFader;
        Sprite mPlayerSprite, mFairySprite;
        Fader mPlayerFader, mFairyFader, mBlackSquareFader;
        FaderManager mFaderManager;
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

            mBlackSquareFader = new Fader(ScreenManager.contentMgr.Load<Texture2D>("Sprites/black_square"), Vector2.Zero, Color.White, 1.0f);
            mFairyFader = new Fader(ScreenManager.contentMgr.Load<Texture2D>("Sprites/fairy_fader"), mFairyPosition, Color.White);
            mPlayerFader = new Fader(ScreenManager.contentMgr.Load<Texture2D>("Sprites/player_fader"), ScreenManager.pusher.mPosition, Color.White);
             
            mFaderManager = new FaderManager(new Fader[3] { mBlackSquareFader, mFairyFader, mPlayerFader });

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
            //mFairySprite.Update(gameTime, true);
            //mPlayerSprite.Update(gameTime, true);
            //mTextFader.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds);

            mFaderManager.FadeInAndOut(mBlackSquareFader, 1.0f, 0.008f, 0, 30);
            mFaderManager.FadeInAndOut(mFairyFader, 0.03f, 0.01f, 3, 2);
            mFaderManager.FadeInAndOut(mPlayerFader, 0.03f, 0.01f, 10, 2);


            mFaderManager.Update(gameTime);



        }

        public override void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            foreach (Texture2D background in mBackgroundTextures)
            {
                spriteBatch.Draw(background, new Rectangle(0, 0, 800, 800), Color.White);
            }

            mFaderManager.Draw(spriteBatch);
           

            //mFairySprite.Draw(spriteBatch, mFairyPosition);
            //mPlayerSprite.Draw(spriteBatch, ScreenManager.pusher.mPosition);


            mTextFader.Draw(spriteBatch, new Vector2(0, 600), 10, true, ScreenManager.lightBlue);



            spriteBatch.DrawString(ScreenManager.font, $"mCurrentTime = {mRoomTimer.mCurrentTime}", Vector2.Zero, Color.White);
        }
    }
}
