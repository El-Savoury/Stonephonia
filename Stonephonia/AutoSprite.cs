using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stonephonia
{
    class AutoSprite : Sprite
    {
        public AutoSprite(Texture2D texture, Vector2 position,
          Point frameSize, Point currentFrame, Point sheetSize,
          int collisionOffset, int timePerFrame, int speed)
            : base(texture, position, frameSize, currentFrame, sheetSize, collisionOffset, timePerFrame, speed) { }

        public override int direction { get { return speed; } } // Inherit speed member of base class to allow movement

        public override void Update(GameTime gametime)
        {
            position.X += direction;

            base.Update(gametime);
        }
    }
}
