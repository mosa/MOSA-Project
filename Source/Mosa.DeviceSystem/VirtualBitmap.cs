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

		public void DrawPoint(int x, int y, Color color)
		{
			if (x < Width && y < Height)
				Image.SetColor((uint)x, (uint)y, (uint)color.ToArgb());
		}

		public void DrawHorizontalLine(int DX, int X1, int Y1, Color color)
		{
			for (var i = 0; i < DX; i++)
				DrawPoint(X1 + i, Y1, color);
		}

		public void DrawVerticalLine(int DY, int X1, int Y1, Color color)
		{
			for (var i = 0; i < DY; i++)
				DrawPoint(X1, Y1 + i, color);
		}

		public void DrawDiagonalLine(int DX, int DY, int X1, int Y1, Color color)
		{
			var dxabs = Math.Abs(DX);
			var dyabs = Math.Abs(DY);
			var sdx = Math.Sign(DX);
			var sdy = Math.Sign(DY);
			var x = dyabs >> 1;
			var y = dxabs >> 1;
			var px = X1;
			var py = Y1;

			if (dxabs >= dyabs) /* the line is more horizontal than vertical */
			{
				for (var i = 0; i < dxabs; i++)
				{
					y += dyabs;
					if (y >= dxabs)
					{
						y -= dxabs;
						py += sdy;
					}
					px += sdx;
					DrawPoint(px, py, color);
				}
			}
			else /* the line is more vertical than horizontal */
			{
				for (var i = 0; i < dyabs; i++)
				{
					x += dxabs;
					if (x >= dyabs)
					{
						x -= dyabs;
						px += sdx;
					}
					py += sdy;
					DrawPoint(px, py, color);
				}
			}
		}

		public void DrawLine(int x1, int y1, int x2, int y2, Color color)
		{
			var dx = x2 - x1;      /* the horizontal distance of the line */
			var dy = y2 - y1;      /* the vertical distance of the line */

			if (dy == 0) /* The line is horizontal */
			{
				DrawHorizontalLine(dx, x1, y1, color);
				return;
			}

			if (dx == 0) /* the line is vertical */
			{
				DrawVerticalLine(dy, x1, y1, color);
				return;
			}

			/* the line is neither horizontal neither vertical, it's diagonal then! */
			DrawDiagonalLine(dx, dy, x1, y1, color);
		}
	}
}
