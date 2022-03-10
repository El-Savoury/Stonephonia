using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stonephonia
{
    abstract class Sprite
    {
        private Texture2D texture;
        protected Vector2 position;
        protected Point frameSize;
        private Point currentFrame;
        private Point sheetSize;
        private int collisionOffset;
        private int timeSinceLastFrame = 0;
        private int timePerFrame;
        protected int speed;

        // Allow subclasses to define behaviour based on movement direction
        public abstract int direction { get; }

        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                  (int)position.X + collisionOffset,
                  (int)position.Y + collisionOffset,
                  frameSize.X - (collisionOffset * 2),
                  frameSize.Y - (collisionOffset * 2));
            }
        }

        public Sprite(Texture2D texture, Vector2 position,
            Point frameSize, Point currentFrame, Point sheetSize,
            int collisionOffset, int timePerFrame, int speed)
        {
            this.texture = texture;
            this.position = position;
            this.frameSize = frameSize;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.collisionOffset = collisionOffset;
            this.timePerFrame = timePerFrame;
            this.speed = speed;
        }

        public virtual void Update(GameTime gameTime)
        {
            // Animate sprite
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if (timeSinceLastFrame > timePerFrame)
            {
                timeSinceLastFrame = 0;
                ++currentFrame.X;

                if (currentFrame.X >= sheetSize.X)
                {
                    currentFrame.X = 0;
                    ++currentFrame.Y;

                    if (currentFrame.Y >= sheetSize.Y) { currentFrame.Y = 0; }
                }
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, new Rectangle(currentFrame.X * frameSize.X, currentFrame.Y * frameSize.Y, frameSize.X, frameSize.Y),
                Color.White, rotation: 0, origin: Vector2.Zero, scale: 1f, SpriteEffects.None, layerDepth: 0);
        }
    }
}
