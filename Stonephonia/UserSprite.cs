using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Stonephonia
{
    class UserSprite : Sprite
    {
        public UserSprite(Texture2D texture, Vector2 position,
          Vector2 speed, Point frameSize, Point currentFrame,
          Point sheetSize, int collisionOffset, int timePerFrame)
            : base(texture, position, speed, frameSize, currentFrame, sheetSize, collisionOffset, timePerFrame) { }

        public override Vector2 direction
        {
            get
            {
                Vector2 inputDir = Vector2.Zero;

                if (Keyboard.GetState().IsKeyDown(Keys.Right)) { inputDir.X += 1; }
                if (Keyboard.GetState().IsKeyDown(Keys.Left)) { inputDir.X -= 1; }

                return inputDir * speed;
            }
        }

        public override void Update(GameTime gameTime)
        {
            // Move sprite within screen bounds
            position += direction;
            Math.Clamp(direction.X, GamePort.renderSurface.Bounds.X, GamePort.renderSurface.Bounds.Right - frameSize.X);

            base.Update(gameTime);
        }
    }
}