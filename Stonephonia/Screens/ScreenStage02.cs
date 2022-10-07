using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Stonephonia.Screens
{
    class ScreenStage02 : GameScreen
    {
        public override void LoadAssets()
        {
            ScreenManager.pusher.mMaxSpeed = 3;
            ScreenManager.pusher.mSprite = new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/player_stage_Two_sheet"),
                                           new Point(60, 84), new Point(0, 0), new Point(2, 1), 200, Color.White);
        }

        public override void UnloadAssests()
        {
        }

        public override void Update(GameTime gameTime)
        {
            ScreenManager.pusher.Update(gameTime, ScreenManager.rock);

            if (InputManager.KeyPressed(Keys.O))
            {
                ScreenManager.ChangeScreen(new ScreenStage02(), new ScreenStage03());
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
