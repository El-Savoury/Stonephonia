using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stonephonia.Effects;

namespace Stonephonia.Screens
{
    public class TestScreen : GameScreen
    {
        Player player;
        Rock[] rock;
        TextFader textFader;

        public override void LoadAssets()
        {
            textFader = new TextFader(ScreenManager.font, "Here is some text on screen", 100f, 0.5f, 0f);

            player = new Player(10, 50)
            {
                mSprite = new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/idle_test_sheet"),
                new Point(32, 32), new Point(0, 0), new Point(4, 1), 200, Color.White),

                mMaxSpeed = 4,
            };

            rock = new Rock[4];

            rock[0] = new Rock(10, 50)
            {
                mSprite = new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/sprite_test"),
                new Point(32, 32), new Point(0, 0), new Point(32, 32), 15, Color.Yellow),

                mMaxSpeed = 4,
                mAcceleration = 0.08f,
            };

            rock[1] = new Rock(200, 50)
            {
                mSprite = new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/sprite_test"),
                new Point(32, 32), new Point(0, 0), new Point(32, 32), 15, Color.Pink),

                mMaxSpeed = 3,
                mAcceleration = 0.02f,
            };

            rock[2] = new Rock(350, 50)
            {
                mSprite = new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/sprite_test"),
                new Point(32, 32), new Point(0, 0), new Point(32, 32), 15, Color.Orange),

                mMaxSpeed = 2,
                mAcceleration = 0.008f,
            };

            rock[3] = new Rock(400, 50)
            {
                mSprite = new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/sprite_test"),
                new Point(32, 32), new Point(0, 0), new Point(32, 32), 15, Color.LightBlue),

                mMaxSpeed = 1,
                mAcceleration = 0.005f,
            };
        }

        public override void UnloadAssests()
        {
        }

        public override void Update(GameTime gameTime)
        {
            //textFader.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds);
            player.Update(gameTime, rock);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //textFader.Draw(spriteBatch, new Vector2(10, 65), 6, true, Color.White);

            foreach (Rock rock in rock)
            {
                rock.Draw(spriteBatch);
            }

            player.Draw(spriteBatch);

            spriteBatch.DrawString(ScreenManager.font, $"mcurrentRock: {Array.IndexOf(rock, player.mCurrentRock)}", new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(ScreenManager.font, $"mVelocity: {player.mVelocity}", new Vector2(0, 15), Color.White);
            spriteBatch.DrawString(ScreenManager.font, $"mPushVelocity: {player.mPushVelocity}", new Vector2(0, 30), Color.Red);
        }

    }
}
