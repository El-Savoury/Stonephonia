using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Stonephonia.Effects;

namespace Stonephonia.Screens
{
    class SplashScreen : Screen
    {
        Timer mRoomTimer = new Timer();
        Rectangle mBlackRectangle;
        Sprite mTitleSprite;
        Vector2 mTitlePosition;
        Fader mPressSpaceFader;

        public override void LoadAssets()
        {
            mBlackRectangle = new Rectangle(0, 0, GamePort.renderSurface.Width, GamePort.renderSurface.Height);

            mTitleSprite = new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/title_sheet"),
               new Point(256, 64), new Point(0, 0), new Point(4, 1), 150, Color.White);
            mTitlePosition = new Vector2((GamePort.renderSurface.Width / 2) - (mTitleSprite.mFrameSize.X / 2),
                                        (GamePort.renderSurface.Height / 2) - (mTitleSprite.mFrameSize.Y / 2));

            mPressSpaceFader = new Fader(ScreenManager.font, "Press space", new Vector2(0, 600), ScreenManager.lightBlue, 1.0f);

            //textFader = new AnimatedTextFader(ScreenManager.font, "Please turn sound on", 1.0f, 0.3f, 0.0f);

            base.LoadAssets();
        }
        public override void UnloadAssests()
        {
        }

        private void StartGame()
        {
            if (InputManager.KeyPressed(Keys.Space))
            {
                ScreenManager.ChangeScreen(new SplashScreen(), new IntroCutscene());
            }
        }

        private void MoveTitle()
        {
            if (mTitlePosition.Y < mTitlePosition.Y * 3 &&
                mRoomTimer.mCurrentTime > 0.2f)
            {
                mTitlePosition.Y += 4;
                mRoomTimer.Reset();
            }
        }

        public override void Update(GameTime gameTime)
        {
            mRoomTimer.Update(gameTime);
            mPressSpaceFader.Flash(1.0f, 0.3f, 0.04f);

            StartGame();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(ScreenManager.pixel, mBlackRectangle, Color.Black);
            mPressSpaceFader.DrawString(spriteBatch, true);
            mTitleSprite.DrawScaled(spriteBatch, mTitlePosition /2, 2.0f);

            //textFader.Draw(spriteBatch, new Vector2(0, 70), 6, true, Color.White);
        }
    }
}
