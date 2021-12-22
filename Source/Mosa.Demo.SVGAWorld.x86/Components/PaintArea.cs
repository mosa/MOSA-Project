// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;
using System.Drawing;

namespace Mosa.Demo.VBEWorld.x86.Components
{
	public class PaintArea
	{
		public int X, Y, LastX, LastY, Width, Height;

		public Color Color;

		public VirtualBitmap Bitmap;

		public PaintArea(int x, int y, int width, int height, Color color)
		{
			X = x;
			Y = y;

			// Initialize, but update later too
			LastX = Mouse.X - X;
			LastY = Mouse.Y - Y;

			Width = width;
			Height = height;

			Color = color;

			Bitmap = new VirtualBitmap((uint)width, (uint)height);
			Bitmap.Clear(color);
		}

		public void Draw()
		{
			Display.DrawImage(X, Y, Bitmap.Image, false);
		}

		//https://github.com/nifanfa/MOSA-GUI-Sample/blob/master/MOSA1/Apps/Paint.cs
		public void Update()
		{
			if (Mouse.State == (int)MouseState.Left && IsInBounds())
			{
				Mouse.IsOnPaintingArea = true;

				if (Mouse.X - X < LastX || Mouse.Y - Y < LastY)
					Bitmap.DrawLine(Mouse.X - X, Mouse.Y - Y, LastX, LastY, Mouse.Color);
				else
					Bitmap.DrawLine(LastX, LastY, Mouse.X - X, Mouse.Y - Y, Mouse.Color);
			} else Mouse.IsOnPaintingArea = false;

			LastX = Mouse.X - X;
			LastY = Mouse.Y - Y;
		}

		public bool IsInBounds()
		{
			return !WindowManager.IsWindowMoving &&
				Mouse.IsInBounds(X, Y, Width, Height);
		}
	}
}
