using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stonephonia
{
    public class Leaf
    {
        private Sprite mSprite;
        public Vector2 mPosition;

        public Leaf(Vector2 position)
        {
            mSprite = new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/leaf_sheet"),
                new Point(64, 64), new Point(0, 0), new Point(4, 1), 150, Color.White);
            mPosition = position;
        }

        public void Update(GameTime gameTime)
        {
            mSprite.AnimateLoop(gameTime);
            mPosition.Y += 4;
            //mPosition.X += 1;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            mSprite.Draw(spriteBatch, mPosition);
        }
    }
}
