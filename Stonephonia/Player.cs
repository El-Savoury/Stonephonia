using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Stonephonia
{
    class Player : Sprite
    {
        public Player(Texture2D texture, Vector2 position,
            Point frameSize, Point currentFrame, Point sheetSize,
            int collisionOffset, int timePerFrame, int velocity)
            : base(texture, position, frameSize, currentFrame, sheetSize, collisionOffset, timePerFrame, velocity)
        {
        }

        public override int direction
        {
            get
            {
                int inputDir = 0;

                if (InputManager.KeyHeld(Keys.Right) || InputManager.KeyHeld(Keys.D))
                {
                    inputDir += 1;
                }
                if (InputManager.KeyHeld(Keys.Left) || InputManager.KeyHeld(Keys.A))
                {
                    inputDir -= 1;
                }

                return inputDir * mVelocity;
            }
        }

        public override void Update(GameTime gameTime)
        {
            // Move sprite within screen bounds
            mPosition.X += direction;

            //if (collisionRect.X < GamePort.renderSurface.Bounds.X) { position.X = GamePort.renderSurface.Bounds.X - (collisionRect.X - position.X); }
            //if (collisionRect.Right > GamePort.renderSurface.Bounds.Right) { position.X = GamePort.renderSurface.Bounds.Right - (collisionRect.Width; }

        }
    }
}