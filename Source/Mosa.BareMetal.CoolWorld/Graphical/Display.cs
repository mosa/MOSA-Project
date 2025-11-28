// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Drawing;
using Mosa.DeviceSystem.Fonts;
using Mosa.DeviceSystem.Graphics;

namespace Mosa.BareMetal.CoolWorld.Graphical;

public static class Display
{
	private static FrameBuffer32 DisplayFrame { get; set; }

	public static FrameBuffer32 BackFrame { get; set; }

	public static IGraphicsDevice Driver { get; private set; }

	public static uint Width { get; private set; }

	public static uint Height { get; private set; }

	public static ISimpleFont DefaultFont { get; set; }

	public static bool Initialize()
	{
		Width = 1280;
		Height = 720;

		Driver = Desktop.DeviceService.GetFirstDevice<IGraphicsDevice>().DeviceDriver as IGraphicsDevice;

		if (Driver == null) return false;

		Driver.SetMode((ushort)Width, (ushort)Height);

		DisplayFrame = Driver.FrameBuffer;
		BackFrame = DisplayFrame.Clone();

		return true;
	}

	public static void DrawMosaLogo(uint v)
	{
		MosaLogo.Draw(v);
	}

	public static void DrawPoint(uint x, uint y, Color color)
	{
		BackFrame.SetPixel((uint)color.ToArgb(), x, y);
	}

	public static void DrawBuffer(uint x, uint y, FrameBuffer32 buffer, bool drawWithAlpha = false)
	{
		BackFrame.DrawBuffer(buffer, x, y, drawWithAlpha);
	}

	public static void DrawString(uint x, uint y, string text, ISimpleFont font, Color color)
	{
		font.DrawString(BackFrame, (uint)color.ToArgb(), x, y, text);
	}

	public static void DrawRectangle(uint x, uint y, uint width, uint height, Color color, bool fill)
	{
		if (fill)
		{
			/*var col = (uint)color.ToArgb();

			for (var xx = x; xx < x + width; xx++)
				BackFrame.SetPixel(col, xx, y);

			for (var yy = y; yy < y + height; yy++)
				Driver.CopyRectangle(x, y, x, yy, width, 1);*/

			BackFrame.FillRectangle((uint)color.ToArgb(), x, y, width, height);
		}
		else BackFrame.DrawRectangle((uint)color.ToArgb(), x, y, width, height, 1);
	}

	public static bool IsInBounds(uint x1, uint x2, uint y1, uint y2, uint width, uint height)
	{
		return x1 >= x2 && x1 <= x2 + width && y1 >= y2 && y1 <= y2 + height;
	}

	public static void Clear(Color color)
	{
		BackFrame.ClearScreen((uint)color.ToArgb());
	}

	public static void Update()
	{
		DisplayFrame.CopyFrameBuffer(BackFrame);
		Driver.Update(0, 0, Width, Height);
	}
}
