using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stonephonia
{
    public class Reflection
    {
        public Sprite mSprite;
        public Vector2 mPosition;

        public Reflection(Sprite sprite, Vector2 position)
        {
            mSprite = sprite;
            mPosition = position;
        }

        public void Update(GameTime gameTime)
        {
            mSprite.AnimateLoop(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            mSprite.Draw(spriteBatch, mPosition);
        }

    }
}
