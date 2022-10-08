using Microsoft.Xna.Framework;

namespace Stonephonia
{
    public class Rock : Entity
    {
        public Rock(Vector2 position, int collisionOffset, int maxSpeed, float acceleration)
            : base(position, collisionOffset, maxSpeed, acceleration)
        {
        }

        private void Glow(Pusher pusher)
        {
            float rockLeft = mPosition.X;
            float rockRight = mPosition.X + mSprite.mFrameSize.X;
            float pusherLeft = pusher.mPosition.X;
            float pusherRight = pusher.mPosition.X + pusher.mSprite.mFrameSize.X;

            if (pusherLeft < rockRight && pusherRight > rockRight ||
                pusherRight > rockLeft && pusherLeft < rockLeft ||
                pusherLeft >= rockLeft && pusherRight <= rockRight)
            {
                mSprite.mColour = Color.Green;
            }
            else { mSprite.mColour = Color.White; }
        }

        public void Update(Pusher pusher)
        {
            Glow(pusher);
        }
    }
}
