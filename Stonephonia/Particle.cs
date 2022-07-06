using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stonephonia
{
    public class Particle
    {
        public Rectangle mBounds;

        public Particle(Point position, Point size)
        {
            mBounds = new Rectangle(position, size);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, mBounds, Color.Red);
        }
    }
}
