// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;
using Mosa.Kernel.x86;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Mosa.Demo.VBEWorld.x86
{
	public static class Display
	{
		private static IFrameBuffer DisplayFrame { get; set; }

		private static IFrameBuffer BackFrame { get; set; }

		public static int Width { get; private set; }

		public static int Height { get; private set; }

		public static BitFont DefaultFont;

		public static string CurrentDriver = "VBE";

		public static bool Initialize()
		{
			if (!VBE.IsVBEAvailable)
				return false;

			Width = VBE.ScreenWidth;
			Height = VBE.ScreenHeight;

			DefaultFont = new BitFont(
				name: "ArialCustomCharset16",
				charset: "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz",
				size: 16,
				data: Convert.FromBase64String("AAAAAAAAAAAAAAAAHAAiACIAIgAiACIAIgAcAAAAAAAAAAAAAAAAAAAAAAAIABgAKAAIAAgACAAIAAgAAAAAAAAAAAAAAAAAAAAAABwAIgACAAIABAAIABAAPgAAAAAAAAAAAAAAAAAAAAAAHAAiAAIADAACAAIAIgAcAAAAAAAAAAAAAAAAAAAAAAAEAAwAFAAUACQAPgAEAAQAAAAAAAAAAAAAAAAAAAAAAB4AEAAgADwAAgACACIAHAAAAAAAAAAAAAAAAAAAAAAAHAAiACAAPAAiACIAIgAcAAAAAAAAAAAAAAAAAAAAAAA+AAQABAAIAAgAEAAQABAAAAAAAAAAAAAAAAAAAAAAABwAIgAiABwAIgAiACIAHAAAAAAAAAAAAAAAAAAAAAAAHAAiACIAIgAeAAIAIgAcAAAAAAAAAAAAAAAAAAAAAAAEAAoACgAKABEAHwAggCCAAAAAAAAAAAAAAAAAAAAAAD4AIQAhAD8AIQAhACEAPgAAAAAAAAAAAAAAAAAAAAAADgARACAAIAAgACAAEQAOAAAAAAAAAAAAAAAAAAAAAAA8ACIAIQAhACEAIQAiADwAAAAAAAAAAAAAAAAAAAAAAD4AIAAgAD4AIAAgACAAPgAAAAAAAAAAAAAAAAAAAAAAPgAgACAAPAAgACAAIAAgAAAAAAAAAAAAAAAAAAAAAAAOABEAIIAgACOAIIARAA4AAAAAAAAAAAAAAAAAAAAAACEAIQAhAD8AIQAhACEAIQAAAAAAAAAAAAAAAAAAAAAAIAAgACAAIAAgACAAIAAgAAAAAAAAAAAAAAAAAAAAAAAEAAQABAAEAAQAJAAkABgAAAAAAAAAAAAAAAAAAAAAACEAIgAkACwANAAiACIAIQAAAAAAAAAAAAAAAAAAAAAAIAAgACAAIAAgACAAIAA+AAAAAAAAAAAAAAAAAAAAAAAggDGAMYAqgCqAKoAkgCSAAAAAAAAAAAAAAAAAAAAAACEAMQApACkAJQAlACMAIQAAAAAAAAAAAAAAAAAAAAAADgARACCAIIAggCCAEQAOAAAAAAAAAAAAAAAAAAAAAAA8ACIAIgAiADwAIAAgACAAAAAAAAAAAAAAAAAAAAAAAA4AEQAggCCAIIAmgBEADoAAAAAAAAAAAAAAAAAAAAAAPgAhACEAPgAkACIAIgAhAAAAAAAAAAAAAAAAAAAAAAAeACEAIAAYAAYAAQAhAB4AAAAAAAAAAAAAAAAAAAAAAD4ACAAIAAgACAAIAAgACAAAAAAAAAAAAAAAAAAAAAAAIQAhACEAIQAhACEAIQAeAAAAAAAAAAAAAAAAAAAAAAAggCCAEQARAAoACgAEAAQAAAAAAAAAAAAAAAAAAAAAAEIQRRAlICUgKKAooBBAEEAAAAAAAAAAAAAAAAAAAAAAIQASABIADAAMABIAEgAhAAAAAAAAAAAAAAAAAAAAAAAggBEAEQAKAAQABAAEAAQAAAAAAAAAAAAAAAAAAAAAAB8AAgAEAAQACAAIABAAPwAAAAAAAAAAAAAAAAAAAAAAAAAAABwAIgAeACIAJgAaAAAAAAAAAAAAAAAAAAAAAAAgACAALAAyACIAIgAyACwAAAAAAAAAAAAAAAAAAAAAAAAAAAAcACIAIAAgACIAHAAAAAAAAAAAAAAAAAAAAAAAAgACABoAJgAiACIAJgAaAAAAAAAAAAAAAAAAAAAAAAAAAAAAHAAiAD4AIAAiABwAAAAAAAAAAAAAAAAAAAAAAAgAEAA4ABAAEAAQABAAEAAAAAAAAAAAAAAAAAAAAAAAAAAAABoAJgAiACIAJgAaAAIAPAAAAAAAAAAAAAAAAAAgACAALAAyACIAIgAiACIAAAAAAAAAAAAAAAAAAAAAACAAAAAgACAAIAAgACAAIAAAAAAAAAAAAAAAAAAAAAAAIAAAACAAIAAgACAAIAAgACAAQAAAAAAAAAAAAAAAAAAgACAAJAAoADAAKAAoACQAAAAAAAAAAAAAAAAAAAAAACAAIAAgACAAIAAgACAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAC8ANIAkgCSAJIAkgAAAAAAAAAAAAAAAAAAAAAAAAAAAPAAiACIAIgAiACIAAAAAAAAAAAAAAAAAAAAAAAAAAAAcACIAIgAiACIAHAAAAAAAAAAAAAAAAAAAAAAAAAAAACwAMgAiACIAMgAsACAAIAAAAAAAAAAAAAAAAAAAAAAAGgAmACIAIgAmABoAAgACAAAAAAAAAAAAAAAAAAAAAAAoADAAIAAgACAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAABwAIgAYAAQAIgAcAAAAAAAAAAAAAAAAAAAAAAAgACAAcAAgACAAIAAgADAAAAAAAAAAAAAAAAAAAAAAAAAAAAAiACIAIgAiACYAGgAAAAAAAAAAAAAAAAAAAAAAAAAAACIAIgAUABQACAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAIiAlIBVAFUAIgAiAAAAAAAAAAAAAAAAAAAAAAAAAAAAiABQACAAIABQAIgAAAAAAAAAAAAAAAAAAAAAAAAAAACIAIgAUABQACAAIAAgAEAAAAAAAAAAAAAAAAAAAAAAAPgAEAAgACAAQAD4AAAAAAA==")
			);

			Utils.Fonts = new List<BitFont>();
			Utils.Fonts.Add(DefaultFont);

			uint memorySize = (uint)(Width * Height * (VBE.BitsPerPixel / 8));

			var lfb = DeviceSystem.HAL.GetPhysicalMemory(VBE.MemoryPhysicalLocation, memorySize);

			DisplayFrame = CreateBuffer(lfb, (uint)Width, (uint)Height, VBE.Pitch, VBE.BitsPerPixel);

			var sfb = DeviceSystem.HAL.AllocateMemory(memorySize, 0);

			BackFrame = CreateBuffer(sfb, (uint)Width, (uint)Height, VBE.Pitch, VBE.BitsPerPixel);

			return true;
		}

		private static IFrameBuffer CreateBuffer(ConstrainedPointer buffer, uint width, uint height, uint pitch, uint bitsPerPixel)
		{
			switch (bitsPerPixel)
			{
				case 8: return new FrameBuffer8bpp(buffer, width, height, 0, pitch);
				case 16: return new FrameBuffer16bpp(buffer, width, height, 0, pitch);
				case 24: return new FrameBuffer24bpp(buffer, width, height, 0, pitch);
				case 32: return new FrameBuffer32bpp(buffer, width, height, 0, pitch);
				default: return null;
			}
		}

		public static void DrawMosaLogo(int v)
		{
			MosaLogo.Draw(BackFrame, (uint)v);
		}

		public static void DrawPoint(int x, int y, Color color)
		{
			BackFrame.SetPixel((uint)color.ToArgb(), (uint)x, (uint)y);
		}

		public static void DrawImage(int x, int y, Image image, bool drawWithAlpha)
		{
			BackFrame.DrawImage(image, (uint)x, (uint)y);
		}

		public static void DrawString(int x, int y, string text, string fontName, Color color)
		{
			var desc = GetFont(fontName);

			desc.DrawString(BackFrame, (uint)color.ToArgb(), (uint)x, (uint)y, text);
		}

		private static BitFont GetFont(string name)
		{
			foreach (var font in Utils.Fonts)
			{
				if (font.Name == name)
					return font;
			}

			return null;
		}

		public static void DrawRectangle(int x, int y, int width, int height, Color color, bool fill)
		{
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
		}
	}
}
