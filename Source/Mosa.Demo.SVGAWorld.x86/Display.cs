// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;
using System.Drawing;

namespace Mosa.Demo.VBEWorld.x86
{
	public static class Display
	{
		private static FrameBuffer32 DisplayFrame { get; set; }

		public static FrameBuffer32 BackFrame { get; set; }

		public static IGraphicsDevice Driver { get; set; }

		public static int Width { get; private set; }

		public static int Height { get; private set; }

		public static ISimpleFont DefaultFont { get; set; }

		public static string CurrentDriver = "VMware SVGA II";

		public static bool Initialize()
		{
			Width = 1280;
			Height = 720;

			Driver = Boot.DeviceService.GetFirstDevice<IGraphicsDevice>().DeviceDriver as IGraphicsDevice;
			if (Driver == null)
				return false;

			Driver.SetMode((ushort)Width, (ushort)Height);

			DisplayFrame = Driver.FrameBuffer;
			BackFrame = DisplayFrame.Clone();

			return true;
		}

		public static void DrawMosaLogo(int v)
		{
			MosaLogo.Draw(v);
		}

		public static void DrawPoint(int x, int y, Color color)
		{
			BackFrame.SetPixel((uint)color.ToArgb(), (uint)x, (uint)y);
		}

		public static void DrawImage(int x, int y, Image image, bool drawWithAlpha)
		{
			BackFrame.DrawImage(image, (uint)x, (uint)y, drawWithAlpha);
		}

		public static void DrawString(int x, int y, string text, ISimpleFont font, Color color)
		{
			font.DrawString(BackFrame, (uint)color.ToArgb(), (uint)x, (uint)y, text);
		}

		public static void DrawRectangle(int x, int y, int width, int height, Color color, bool fill)
		{
			//Driver.FillRectangle((uint)x, (uint)y, (uint)width, (uint)height, (uint)color.ToArgb());
			if (fill)
				BackFrame.FillRectangle((uint)color.ToArgb(), (uint)x, (uint)y, (uint)width, (uint)height);
			else
				BackFrame.DrawRectangle((uint)color.ToArgb(), (uint)x, (uint)y, (uint)width, (uint)height, 1);
		}

		public static bool IsInBounds(int x1, int x2, int y1, int y2, int width, int height)
		{
			return x1 >= x2 && x1 <= x2 + width && y1 >= y2 && y1 <= y2 + height;
		}

		public static void Clear(Color color)
		{
			BackFrame.ClearScreen((uint)color.ToArgb());
		}

		public static void Update()
		{
			DisplayFrame.CopyFrame(BackFrame);
			Driver.Update();
		}
	}
}
