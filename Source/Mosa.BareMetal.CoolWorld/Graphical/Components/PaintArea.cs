// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Drawing;
using Mosa.DeviceSystem.Graphics;
using Mosa.DeviceSystem.HardwareAbstraction;
using Mosa.DeviceSystem.Mouse;

namespace Mosa.BareMetal.CoolWorld.Graphical.Components;

public class PaintArea
{
	public uint X, Y;

	public readonly FrameBuffer32 Buffer;

	private uint LastX, LastY;
	private readonly uint Width;
	private readonly uint Height;

	public PaintArea(uint x, uint y, uint width, uint height, Color color)
	{
		X = x;
		Y = y;

		// Initialize, but update later too
		LastX = Mouse.X - X;
		LastY = Mouse.Y - Y;

		Width = width;
		Height = height;

		Buffer = new(HAL.AllocateMemory(width * height * 4, 0), width, height, (xx, yy) => (yy * width + xx) * 4);
		Buffer.ClearScreen((uint)color.ToArgb());
	}

	public void Draw()
	{
		Display.DrawBuffer(X, Y, Buffer);
	}

	public void Update()
	{
		if (Mouse.State == MouseState.Left && IsInBounds())
		{
			if (Mouse.X - X < LastX || Mouse.Y - Y < LastY)
				Buffer.DrawLine((uint)Mouse.Color.ToArgb(), Mouse.X - X, Mouse.Y - Y, LastX, LastY);
			else
				Buffer.DrawLine((uint)Mouse.Color.ToArgb(), LastX, LastY, Mouse.X - X, Mouse.Y - Y);
		}

		LastX = Mouse.X - X;
		LastY = Mouse.Y - Y;
	}

	public bool IsInBounds()
	{
		return !WindowManager.IsWindowMoving && Mouse.IsInBounds(X, Y, Width, Height);
	}
}
