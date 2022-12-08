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

        public static Reflection[] Load()
        {
            Reflection[] relections = new Reflection[]
            {
                //new Reflection(new Sprite(ScreenManager.contentMgr.Load<Texture2D>("Sprites/river_rock_shadow"),
                //new Point(124, 124), new Point(0, 0), new Point(4, 1), 100, Color.White), new Vector2(100, 100))
            };
            return relections;
        }

        public void Update(GameTime gameTime)
        {
            mSprite.Update(gameTime, true);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            mSprite.Draw(spriteBatch, mPosition);
        }

    }
}
