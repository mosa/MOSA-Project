// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.DeviceSystem;
using System.Collections.Generic;
using System.Drawing;

namespace Mosa.Demo.VBEWorld.x86.Components
{
	// TODO: Fix
	public class VirtualBitmap
	{
		public Image Bitmap;

		public int Width, Height, Bpp;

		public VirtualBitmap(int width, int height)
		{
			Width = width;
			Height = height;
			Bpp = 4;

			Bitmap = new Image(Width, Height);
		}

		public void Clear(Color color)
		{
			Internal.MemoryClear(Bitmap.Data.Address, (uint)(Width * Height * Bpp), (uint)color.ToArgb());
		}

		public void DrawPoint(int x, int y, Color color)
		{
			if (x < Width)
				Bitmap.Data.Write32((uint)((Width * y + x) * Bpp), (uint)color.ToArgb());
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

			Bitmap = new VirtualBitmap(width, height);
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
					Bitmap.DrawPoint(Mouse.X, Mouse.Y, Mouse.Color);
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
