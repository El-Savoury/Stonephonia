using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Stonephonia.Screens
{
    class ScreenStage03 : GameScreen
    {
        public override void LoadAssets()
        {
            ScreenManager.pusher.mMaxSpeed = 2;
            ScreenManager.pusher.mSprite = new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/player_stage_Three_sheet"),
                                           new Point(60, 84), new Point(0, 0), new Point(2, 1), 200, Color.White);
        }

        public override void UnloadAssests()
        {
        }

        public override void Update(GameTime gameTime)
        {
            ScreenManager.pusher.Update(gameTime, ScreenManager.rock);

            if (InputManager.KeyPressed(Keys.I))
            {
                ScreenManager.ChangeScreen(new ScreenStage03(), new ScreenStage04());
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Rock rock in ScreenManager.rock)
            {
                rock.Draw(spriteBatch);
            }

            ScreenManager.pusher.Draw(spriteBatch);
        }
    }
}
