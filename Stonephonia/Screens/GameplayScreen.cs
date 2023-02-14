using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using Stonephonia.Managers;

namespace Stonephonia.Screens
{
    public class GameplayScreen : Screen
    {
        Timer mRoomTimer;
        LeafManager mLeafManager;
        Texture2D[] mBackgroundTextures, mForegroundTextures;
        TextPrompt[] mTextPrompts;
        TextPromptManager mTextPromptManager;
        Rock[] mRocks;

        public GameplayScreen()
        {
            OnActivate();
        }

        public override void LoadAssets()
        {
            ScreenManager.pusher.Load(ScreenManager.contentMgr);
            mRocks = Rock.Load();
            mTextPrompts = TextPrompt.Load();
            mRoomTimer = new Timer();
            mLeafManager = new LeafManager();
            mTextPromptManager = new TextPromptManager(mTextPrompts);

            mBackgroundTextures = new Texture2D[]
            {
                ScreenManager.contentMgr.Load<Texture2D>("Sprites/background_trees"),
                ScreenManager.contentMgr.Load<Texture2D>("Sprites/background_bushes"),
            };

            mForegroundTextures = new Texture2D[] { ScreenManager.contentMgr.Load<Texture2D>("Sprites/canopy") };
        }

        public override void UnloadAssests()
        {
        }

        private void OnActivate()
        {
            SoundManager.PlayMusic(SoundManager.MusicType.AmbientTrack, 0.5f);
            //SoundManager.PlaySFX(SoundManager.SFXType.MainTheme, 1.0f);
        }

        public override void Update(GameTime gameTime)
        {
            //InputManager.NoInputTimeOut(gameTime, 10, new GameplayScreen(), new SplashScreen());

            mRoomTimer.Update(gameTime);

            foreach (Rock rock in mRocks)
            {
                rock.Update(gameTime, ScreenManager.pusher);
            }

            ScreenManager.pusher.Update(gameTime, mRoomTimer, mRocks);
            mLeafManager.Update(gameTime, ScreenManager.pusher);
            mTextPromptManager.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Texture2D background in mBackgroundTextures)
            {
                spriteBatch.Draw(background, new Rectangle(0, 0, 800, 800), Color.White);
            }

            foreach (Rock rock in mRocks)
            {
                rock.Draw(spriteBatch);
            }

            ScreenManager.pusher.Draw(spriteBatch);
            mLeafManager.Draw(spriteBatch);

            foreach (Texture2D foreground in mForegroundTextures)
            {
                spriteBatch.Draw(foreground, new Rectangle(0, 0, 800, 800), Color.White);
            }

            mTextPromptManager.Draw(spriteBatch);
            spriteBatch.DrawString(ScreenManager.font, $"mVelocity: {ScreenManager.pusher.mVelocity}", new Vector2(0, 15), Color.White);
        }

    }
}
