using Microsoft.Xna.Framework;

namespace Stonephonia
{
    public class Rock : Entity
    {
        public Rock(Vector2 position, int collisionOffset, int maxSpeed, float acceleration)
            : base(position, collisionOffset, maxSpeed, acceleration)
        {
        }
    }
}
