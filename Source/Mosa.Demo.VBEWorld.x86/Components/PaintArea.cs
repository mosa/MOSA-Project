// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;
using System.Drawing;

namespace Mosa.Demo.VBEWorld.x86.Components
{
	// TODO: Fix
	public class VirtualBitmap
	{
		public Image Bitmap;

		public uint Width, Height, Bpp;

		public VirtualBitmap(uint width, uint height)
		{
			Width = width;
			Height = height;
			Bpp = 4;

			Bitmap = new Image(Width, Height, Bpp);
		}

		public void Clear(Color color)
		{
			Bitmap.Clear((uint)color.ToArgb());
		}

		public void DrawPoint(uint x, uint y, Color color)
		{
			if (x < Width && y < Height)
				Bitmap.SetColor(x, y, (uint)color.ToArgb());
		}
	}

	public class PaintArea
	{
		public int X, Y, Width, Height;

		public Color Color;

		public VirtualBitmap Bitmap;

		public PaintArea(int x, int y, int width, int height, Color color)
		{
			X = x;
			Y = y;

			Width = width;
			Height = height;

			Color = color;

			Bitmap = new VirtualBitmap((uint)width, (uint)height);
			Bitmap.Clear(color);
		}

		public void Draw()
		{
			Display.DrawImage(X, Y, Bitmap.Bitmap, false);
		}

		public void Update()
		{
			if (IsInBounds())
			{
				Mouse.IsOnPaintingArea = true;
				Mouse.Color = Boot.Invert(Color);

				if (Mouse.State == 0)
					Bitmap.DrawPoint((uint)Mouse.X, (uint)Mouse.Y, Mouse.Color);
			}
			else Mouse.IsOnPaintingArea = false;
		}

		public bool IsInBounds()
		{
			return !WindowManager.IsWindowMoving &&
				Mouse.IsInBounds(X, Y, Width, Height);
		}
	}
}
