// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Drawing;
using Mosa.DeviceSystem;

namespace Mosa.Demo.SVGAWorld.x86.Components
{
	public class PaintArea
	{
		public int X, Y, LastX, LastY, Width, Height;

		public Color Color;

		public readonly VirtualBitmap Bitmap;

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
					Bitmap.DrawLine((uint)(Mouse.X - X), (uint)(Mouse.Y - Y), (uint)LastX, (uint)LastY, Mouse.Color);
				else
					Bitmap.DrawLine((uint)LastX, (uint)LastY, (uint)(Mouse.X - X), (uint)(Mouse.Y - Y), Mouse.Color);
			}
			else Mouse.IsOnPaintingArea = false;

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
