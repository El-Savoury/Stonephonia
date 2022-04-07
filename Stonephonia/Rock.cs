using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stonephonia
{
    class Rock : Entity
    {
        public Rock(Texture2D texture)
            : base(texture)
        {
        }

        //public override int mVelocity { get { return mMoveSpeed; } } // Inherit speed member of base class to allow movement

        //public override void Update(GameTime gametime)
        //{
        //    mPosition.X += mDirection;
        //}
    }
}
