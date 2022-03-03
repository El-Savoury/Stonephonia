using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Stonephonia
{
    public static class GamePort
    {
        public static RenderTarget2D renderSurface;
        public static Rectangle renderArea;

        public static void KeepAspectRatio(GameWindow window)
        {
            int width = window.ClientBounds.Width;
            int height = window.ClientBounds.Height;

            if (height < width / (float)renderSurface.Width * renderSurface.Height)
            {
                width = (int)(height / (float)renderSurface.Height * renderSurface.Width);
            }
            else { height = (int)(width / (float)renderSurface.Width * renderSurface.Height); }

            int x = (window.ClientBounds.Width - width) / 2;
            int y = (window.ClientBounds.Height - height) / 2;

            renderArea = new Rectangle(x, y, width, height);
        }
    }
}
