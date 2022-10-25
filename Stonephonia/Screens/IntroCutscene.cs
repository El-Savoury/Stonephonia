using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Stonephonia.Effects;

namespace Stonephonia.Screens
{
    public class IntroCutscene : Screen
    {
        Timer roomTimer;
        Sprite playerSprite, fairySprite;
        Fader playerFader, fairyFader;
        AnimatedTextFader textFader;

        Vector2 playerPosition = new Vector2(30, 100);
        Vector2 fairyPosition = new Vector2(300, 30);

        public override void LoadAssets()
        {
            roomTimer = new Timer();

            playerSprite = new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/player_sprite"),
                new Point(24, 24), new Point(0, 0), new Point(1, 1), 200, Color.White, 0.0f);

            fairySprite = new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/sprite_test"),
                new Point(32, 32), new Point(0, 0), new Point(1, 1), 0, Color.White, 0.0f);

            playerFader = new Fader(ScreenManager.contentMgr.Load<Texture2D>("Sprites/temp_sprite_fade"), playerPosition);
            fairyFader = new Fader(ScreenManager.contentMgr.Load<Texture2D>("Sprites/sprite_test"), fairyPosition);
            textFader = new AnimatedTextFader(ScreenManager.font, "You must document the passing of time", 100f, 0.5f, 0f);
        }

        public override void UnloadAssests()
        {
        }

        public override void Update(GameTime gameTime)
        {
            roomTimer.Update(gameTime);


            if (roomTimer.mCurrentTime > 10)
            {
                playerSprite.SetVisible(true);
                playerFader.StagedFade(false, 0.3f, 0.5f);
            }

            if (roomTimer.mCurrentTime > 3)
            {
                //playerFader.GlowFade(true, 0.04f, 0.02f, 0.5f, 0.2f);
                playerFader.StagedFade(true, 0.2f, 0.3f);
            }


            textFader.Update((float)gameTime.ElapsedGameTime.TotalMilliseconds);
            //playerFader.Update(gameTime);
            playerSprite.Update(gameTime);
        }

        public override void Draw(GameTime gametime, SpriteBatch spriteBatch)
        {
            //textFader.Draw(spriteBatch, new Vector2(0, 150), 5, true, Color.White);
            //playerSprite.Draw(spriteBatch, playerPosition);
            //playerFader.Draw(spriteBatch);
            spriteBatch.DrawString(ScreenManager.font, $"mCurrentTime = {roomTimer.mCurrentTime}", Vector2.Zero, Color.White);
        }
    }
}
