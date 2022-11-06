using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stonephonia.Managers;

namespace Stonephonia.Screens
{
    public class GameplayScreen : Screen
    {
        Timer mRoomTimer;
        LeafManager mLeafManager;
        Texture2D[] mBackgroundTextures;
        Reflection[] mReflections;
        TimedText[] mTextPrompts;
        TextManager mTextPromptManager;

        public override void LoadAssets()
        {
            mRoomTimer = new Timer();
            mLeafManager = new LeafManager();

            mBackgroundTextures = new Texture2D[]
            {
                ScreenManager.contentMgr.Load<Texture2D>("Sprites/background_trees"),
                ScreenManager.contentMgr.Load<Texture2D>("Sprites/background_bushes"),
            };

            mReflections = new Reflection[]
            {
                new Reflection(new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/river_rock_shadow"),
                new Point(124,124), new Point(0,0), new Point(4,1), 100, Color.White), new Vector2(100,100))
            };

            mTextPrompts = new TimedText[]
            {
                new TimedText(new Vector2(0, 600), 3, "Arrow keys to move", ScreenManager.darkBlue),
                new TimedText(new Vector2(0, 600), 2, "Hold space to push", ScreenManager.darkBlue)
            };

            mTextPromptManager = new TextManager(mTextPrompts);
        }

        public override void UnloadAssests()
        {
        }

        public override void Update(GameTime gameTime)
        {
            //InputManager.NoInputTimeOut(gameTime, 10, new GameplayScreen(), new IntroCutscene());

            mRoomTimer.Update(gameTime);
            
            foreach (Reflection reflection in mReflections)
            {
                reflection.Update(gameTime);
            }

            foreach (Rock rock in ScreenManager.rock)
            {
                rock.Update(gameTime, ScreenManager.pusher);
            }

            ScreenManager.pusher.Update(gameTime, mRoomTimer, ScreenManager.rock);
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

            foreach (Reflection reflection in mReflections)
            {
                //reflection.Draw(spriteBatch);
            }

            //foreach (Rock rock in ScreenManager.rock)
            //{
            //    rock.Draw(spriteBatch);
            //}

            ScreenManager.rock[0].Draw(spriteBatch);

            ScreenManager.pusher.Draw(spriteBatch);
            mLeafManager.Draw(spriteBatch);
            mTextPromptManager.Draw(spriteBatch);

            // Debug Stats
            //ScreenManager.pusher.DrawDebug(gameTime, spriteBatch);
            // spriteBatch.Draw(ScreenManager.pixel, rock.mCollisionRect, Color.Blue * 0.5f);
            // ScreenManager.pusher.Draw(gameTime, spriteBatch); 
            spriteBatch.DrawString(ScreenManager.font, $"Timer: {mRoomTimer.mCurrentTime}", new Vector2(600, 0), Color.Red);
            //spriteBatch.DrawString(ScreenManager.font, $"mCurrentRock: {Array.IndexOf(ScreenManager.rock, ScreenManager.pusher.mCurrentRock)}", new Vector2(600, 0), Color.Red);
            // spriteBatch.DrawString(ScreenManager.font, $"mVelocity: {player.mVelocity}", new Vector2(0, 15), Color.White);
            // spriteBatch.DrawString(ScreenManager.font, $"mPushVelocity: {player.mPushVelocity}", new Vector2(0, 30), Color.Red);
        }

    }
}
