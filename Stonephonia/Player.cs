using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Stonephonia
{
    public class Player
    {
        public Rectangle Bounds = new Rectangle(100, 350, 32, 32);
        public int Speed = 3;

        public Player() { }

        public void Move()
        {
            int x = this.Bounds.X;

            if (Keyboard.GetState().IsKeyDown(Keys.Right)) { x += Speed; }
            if (Keyboard.GetState().IsKeyDown(Keys.Left)) { x -= Speed; }

            x = Math.Clamp(x, GamePort.renderSurface.Bounds.Left, GamePort.renderSurface.Bounds.Right - this.Bounds.Width);

            Bounds = new Rectangle(x, this.Bounds.Y, 32, 32);
        }
    }
}
