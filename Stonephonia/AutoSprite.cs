using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stonephonia
{
    class AutoSprite : Sprite
    {
        public AutoSprite(Texture2D texture, Vector2 position,
          Vector2 speed, Point frameSize, Point currentFrame,
          Point sheetSize, int collisionOffset, int timePerFrame)
            : base(texture, position, speed, frameSize, currentFrame, sheetSize, collisionOffset, timePerFrame) { }

        public override Vector2 direction { get { return speed; } } // Inherit speed member of base class to allow movement

        public override void Update(GameTime gametime)
        {
            position += direction;

            base.Update(gametime);
        }
    }
}
