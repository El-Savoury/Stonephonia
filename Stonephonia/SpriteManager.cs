using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stonephonia
{
    public class SpriteManager : DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        Player player;
        List<Sprite> spriteList = new List<Sprite>();

        public SpriteManager(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);

            player = new Player(Game.Content.Load<Texture2D>("Sprites/sprite_test"),
                new Vector2(0, 100), new Point(32, 32), new Point(0, 32), new Point(0, 0), 10, 16, 3);

            spriteList.Add(new Rock(Game.Content.Load<Texture2D>("Sprites/big_paddle"),
                new Vector2(50, 100), new Point(32, 32), new Point(0, 32), new Point(0, 0), 0, 16, 1));



            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            player.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(GamePort.renderSurface);
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            player.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(GamePort.renderSurface, GamePort.renderArea, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
