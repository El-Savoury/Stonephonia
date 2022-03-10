using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Stonephonia
{
    class UserSprite : Sprite
    {
        public UserSprite(Texture2D texture, Vector2 position,
          Point frameSize, Point currentFrame, Point sheetSize,
          int collisionOffset, int timePerFrame, int speed)
            : base(texture, position, frameSize, currentFrame, sheetSize, collisionOffset, timePerFrame, speed) { }

        public override int direction
        {
            get
            {
                int inputDir = 0;

                if (Keyboard.GetState().IsKeyDown(Keys.Right)) { inputDir += 1; }
                if (Keyboard.GetState().IsKeyDown(Keys.Left)) { inputDir -= 1; }

                return inputDir * speed;
            }
        }

        public override void Update(GameTime gameTime)
        {
            // Move sprite within screen bounds
            position.X += direction;

            //if (collisionRect.X < GamePort.renderSurface.Bounds.X) { position.X = GamePort.renderSurface.Bounds.X - (collisionRect.X - position.X); }
            //if (collisionRect.Right > GamePort.renderSurface.Bounds.Right) { position.X = GamePort.renderSurface.Bounds.Right - (collisionRect.Width; }

            base.Update(gameTime);
        }
    }
}