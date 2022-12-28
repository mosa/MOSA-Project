using System;
using System.Drawing;

namespace Mosa.DeviceSystem
{
	public class VirtualBitmap
	{
		public Image Image;

		public uint Width, Height, Bpp;

		public VirtualBitmap(uint width, uint height)
		{
			Width = width;
			Height = height;
			Bpp = 4;

			Image = new Image(Width, Height, Bpp);
		}

		public void Clear(Color color)
		{
			Image.Clear((uint)color.ToArgb());
		}

		public void DrawPoint(uint x, uint y, Color color)
		{
			if (x >= Width || y >= Height)
				return;

			Image.SetColor(x, y, (uint)color.ToArgb());
		}

		public void DrawLine(uint x1, uint y1, uint x2, uint y2, Color color)
		{
			var dx = x2 - x1; /* The horizontal distance of the line */
			var dy = y2 - y1; /* The vertical distance of the line */

			if (dy == 0) /* The line is horizontal */
			{
				DrawHorizontalLine(dx, x1, y1, color);
				return;
			}

			if (dx == 0) /* The line is vertical */
			{
				DrawVerticalLine(dy, x1, y1, color);
				return;
			}

			/* The line is neither horizontal nor vertical, so it's diagonal! */
			DrawDiagonalLine(dx, dy, x1, y1, color);
		}

		private void DrawHorizontalLine(uint dx, uint x, uint y, Color color)
		{
			for (uint i = 0; i < dx; i++)
				DrawPoint(x + i, y, color);
		}

		private void DrawVerticalLine(uint dy, uint x, uint y, Color color)
		{
			for (uint i = 0; i < dy; i++)
				DrawPoint(x, y + i, color);
		}

		private void DrawDiagonalLine(uint dx, uint dy, uint x1, uint y1, Color color)
		{
			var sdx = (uint)Math.Sign(dx);
			var sdy = (uint)Math.Sign(dy);
			var x = dy >> 1;
			var y = dx >> 1;
			var px = x1;
			var py = y1;

			if (dx >= dy) /* The line is more horizontal than vertical */
			{
				for (var i = 0; i < dx; i++)
				{
					y += dy;
					if (y >= dx)
					{
						y -= dx;
						py += sdy;
					}
					px += sdx;
					DrawPoint(px, py, color);
				}
			}
			else /* The line is more vertical than horizontal */
			{
				for (var i = 0; i < dy; i++)
				{
					x += dx;
					if (x >= dy)
					{
						x -= dy;
						px += sdx;
					}
					py += sdy;
					DrawPoint(px, py, color);
				}
			}
		}
	}
}
