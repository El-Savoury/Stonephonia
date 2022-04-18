using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stonephonia
{
    public class Rock : Entity
    {
        public Rock(Texture2D texture)
            : base(texture)
        {
        }

        public override void Update()
        {
            mPosition.X += mVelocity;
        }
    }
}
