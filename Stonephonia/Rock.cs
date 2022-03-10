using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stonephonia
{
    class Rock : AutoSprite
    {
        public Rock(Texture2D texture, Vector2 position,
         Point frameSize, Point currentFrame, Point sheetSize,
         int collisionOffset, int timePerFrame, int speed)
           : base(texture, position, frameSize, currentFrame, sheetSize, collisionOffset, timePerFrame, speed) { }
    }
}
