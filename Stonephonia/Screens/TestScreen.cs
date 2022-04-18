using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stonephonia.TextEffects;

namespace Stonephonia.Screens
{
    public class TestScreen : GameScreen
    {
        Player player;
        Character character;
        Rock[] rock;
        TextFader textFader;

        public override void LoadAssets()
        {
            //player = new Player(ScreenManager.contentMgr.Load<Texture2D>("Sprites/sprite_test"),
            //    new Vector2(0, 100), new Point(32, 32), new Point(0, 0), new Point(32, 32), 10, 16, 3);

            //textFader = new TextFader(ScreenManager.font, "Here is some text on screen", 100f, 0.5f, 0f);

            character = new Character(ScreenManager.contentMgr.Load<Texture2D>("Sprites/sprite_test"))
            {
                mPosition = new Vector2(10, 50),
                mMaxSpeed = 4,
                mColour = Color.Red,
            };

            rock = new Rock[4];

            rock[0] = new Rock(ScreenManager.contentMgr.Load<Texture2D>("Sprites/sprite_test"))
            {
                mPosition = new Vector2(100, 50),
                mMaxSpeed = 4,
                mSpeedModifier = 0.08f,
                mColour = Color.Yellow,
            };

            rock[1] = new Rock(ScreenManager.contentMgr.Load<Texture2D>("Sprites/sprite_test"))
            {
                mPosition = new Vector2(200, 50),
                mMaxSpeed = 3,
                mSpeedModifier = 0.02f,
                mColour = Color.Pink,
            };

            rock[2] = new Rock(ScreenManager.contentMgr.Load<Texture2D>("Sprites/sprite_test"))
            {
                mPosition = new Vector2(300, 50),
                mMaxSpeed = 2,
                mSpeedModifier = 0.008f,
                mColour = Color.Orange,
            };

            rock[3] = new Rock(ScreenManager.contentMgr.Load<Texture2D>("Sprites/sprite_test"))
            {
                mPosition = new Vector2(400, 50),
                mMaxSpeed = 1,
                mSpeedModifier = 0.005f,
                mColour = Color.Blue,
            };

            base.LoadAssets();
        }
        public override void UnloadAssests()
        {
        }

        public override void Update(GameTime gameTime)
        {
            //player.Update(gameTime);
            //textFader.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds);
            character.Update(gameTime, rock);

            foreach (Rock rock in rock)
            {
                rock.Update(gameTime);
            }

            

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //player.Draw(gameTime, spriteBatch);
            //textFader.Draw(spriteBatch, new Vector2(10, 65), 6, true, Color.White);

            foreach (Rock rock in rock)
            {
                rock.Draw(spriteBatch);
            }

            character.Draw(spriteBatch);

            spriteBatch.DrawString(ScreenManager.font, $"currentRock: {Array.IndexOf(rock, character.mCurrentRock)}", Vector2.Zero, Color.White);
            spriteBatch.DrawString(ScreenManager.font, $"mVelocity: {character.mVelocity}", new Vector2(0, 15), Color.White);

        }

    }
}
