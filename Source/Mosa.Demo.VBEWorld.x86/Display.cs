// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;
using Mosa.Kernel.x86;
using System.Drawing;
using System.Collections.Generic;
using System;

namespace Mosa.Demo.VBEWorld.x86
{
	public static class Display
	{
		public static ConstrainedPointer lfb;

		private static IFrameBuffer Framebuffer { get; set; }

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

			DefaultFont = new BitFont();
			DefaultFont.Name = "ArialCustomCharset16";
			DefaultFont.Charset = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
			DefaultFont.Size = 16;
			DefaultFont.Buffer = Convert.FromBase64String("AAAAAAAAAAAAAAAAHAAiACIAIgAiACIAIgAcAAAAAAAAAAAAAAAAAAAAAAAIABgAKAAIAAgACAAIAAgAAAAAAAAAAAAAAAAAAAAAABwAIgACAAIABAAIABAAPgAAAAAAAAAAAAAAAAAAAAAAHAAiAAIADAACAAIAIgAcAAAAAAAAAAAAAAAAAAAAAAAEAAwAFAAUACQAPgAEAAQAAAAAAAAAAAAAAAAAAAAAAB4AEAAgADwAAgACACIAHAAAAAAAAAAAAAAAAAAAAAAAHAAiACAAPAAiACIAIgAcAAAAAAAAAAAAAAAAAAAAAAA+AAQABAAIAAgAEAAQABAAAAAAAAAAAAAAAAAAAAAAABwAIgAiABwAIgAiACIAHAAAAAAAAAAAAAAAAAAAAAAAHAAiACIAIgAeAAIAIgAcAAAAAAAAAAAAAAAAAAAAAAAEAAoACgAKABEAHwAggCCAAAAAAAAAAAAAAAAAAAAAAD4AIQAhAD8AIQAhACEAPgAAAAAAAAAAAAAAAAAAAAAADgARACAAIAAgACAAEQAOAAAAAAAAAAAAAAAAAAAAAAA8ACIAIQAhACEAIQAiADwAAAAAAAAAAAAAAAAAAAAAAD4AIAAgAD4AIAAgACAAPgAAAAAAAAAAAAAAAAAAAAAAPgAgACAAPAAgACAAIAAgAAAAAAAAAAAAAAAAAAAAAAAOABEAIIAgACOAIIARAA4AAAAAAAAAAAAAAAAAAAAAACEAIQAhAD8AIQAhACEAIQAAAAAAAAAAAAAAAAAAAAAAIAAgACAAIAAgACAAIAAgAAAAAAAAAAAAAAAAAAAAAAAEAAQABAAEAAQAJAAkABgAAAAAAAAAAAAAAAAAAAAAACEAIgAkACwANAAiACIAIQAAAAAAAAAAAAAAAAAAAAAAIAAgACAAIAAgACAAIAA+AAAAAAAAAAAAAAAAAAAAAAAggDGAMYAqgCqAKoAkgCSAAAAAAAAAAAAAAAAAAAAAACEAMQApACkAJQAlACMAIQAAAAAAAAAAAAAAAAAAAAAADgARACCAIIAggCCAEQAOAAAAAAAAAAAAAAAAAAAAAAA8ACIAIgAiADwAIAAgACAAAAAAAAAAAAAAAAAAAAAAAA4AEQAggCCAIIAmgBEADoAAAAAAAAAAAAAAAAAAAAAAPgAhACEAPgAkACIAIgAhAAAAAAAAAAAAAAAAAAAAAAAeACEAIAAYAAYAAQAhAB4AAAAAAAAAAAAAAAAAAAAAAD4ACAAIAAgACAAIAAgACAAAAAAAAAAAAAAAAAAAAAAAIQAhACEAIQAhACEAIQAeAAAAAAAAAAAAAAAAAAAAAAAggCCAEQARAAoACgAEAAQAAAAAAAAAAAAAAAAAAAAAAEIQRRAlICUgKKAooBBAEEAAAAAAAAAAAAAAAAAAAAAAIQASABIADAAMABIAEgAhAAAAAAAAAAAAAAAAAAAAAAAggBEAEQAKAAQABAAEAAQAAAAAAAAAAAAAAAAAAAAAAB8AAgAEAAQACAAIABAAPwAAAAAAAAAAAAAAAAAAAAAAAAAAABwAIgAeACIAJgAaAAAAAAAAAAAAAAAAAAAAAAAgACAALAAyACIAIgAyACwAAAAAAAAAAAAAAAAAAAAAAAAAAAAcACIAIAAgACIAHAAAAAAAAAAAAAAAAAAAAAAAAgACABoAJgAiACIAJgAaAAAAAAAAAAAAAAAAAAAAAAAAAAAAHAAiAD4AIAAiABwAAAAAAAAAAAAAAAAAAAAAAAgAEAA4ABAAEAAQABAAEAAAAAAAAAAAAAAAAAAAAAAAAAAAABoAJgAiACIAJgAaAAIAPAAAAAAAAAAAAAAAAAAgACAALAAyACIAIgAiACIAAAAAAAAAAAAAAAAAAAAAACAAAAAgACAAIAAgACAAIAAAAAAAAAAAAAAAAAAAAAAAIAAAACAAIAAgACAAIAAgACAAQAAAAAAAAAAAAAAAAAAgACAAJAAoADAAKAAoACQAAAAAAAAAAAAAAAAAAAAAACAAIAAgACAAIAAgACAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAC8ANIAkgCSAJIAkgAAAAAAAAAAAAAAAAAAAAAAAAAAAPAAiACIAIgAiACIAAAAAAAAAAAAAAAAAAAAAAAAAAAAcACIAIgAiACIAHAAAAAAAAAAAAAAAAAAAAAAAAAAAACwAMgAiACIAMgAsACAAIAAAAAAAAAAAAAAAAAAAAAAAGgAmACIAIgAmABoAAgACAAAAAAAAAAAAAAAAAAAAAAAoADAAIAAgACAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAABwAIgAYAAQAIgAcAAAAAAAAAAAAAAAAAAAAAAAgACAAcAAgACAAIAAgADAAAAAAAAAAAAAAAAAAAAAAAAAAAAAiACIAIgAiACYAGgAAAAAAAAAAAAAAAAAAAAAAAAAAACIAIgAUABQACAAIAAAAAAAAAAAAAAAAAAAAAAAAAAAAIiAlIBVAFUAIgAiAAAAAAAAAAAAAAAAAAAAAAAAAAAAiABQACAAIABQAIgAAAAAAAAAAAAAAAAAAAAAAAAAAACIAIgAUABQACAAIAAgAEAAAAAAAAAAAAAAAAAAAAAAAPgAEAAgACAAQAD4AAAAAAA==");

			Utils.Fonts = new List<BitFont>();
			Utils.Fonts.Add(DefaultFont);

			uint memorySize = (uint)(Width * Height * (VBE.BitsPerPixel / 8));
			lfb = Mosa.DeviceSystem.HAL.GetPhysicalMemory(VBE.MemoryPhysicalLocation, memorySize);

			switch (VBE.BitsPerPixel)
			{
				case 8: Framebuffer = new FrameBuffer8bpp(lfb, (uint)Width, (uint)Height, 0, VBE.Pitch); break;
				case 16: Framebuffer = new FrameBuffer16bpp(lfb, (uint)Width, (uint)Height, 0, VBE.Pitch); break;
				case 24: Framebuffer = new FrameBuffer24bpp(lfb, (uint)Width, (uint)Height, 0, VBE.Pitch); break;
				case 32: Framebuffer = new FrameBuffer32bpp(lfb, (uint)Width, (uint)Height, 0, VBE.Pitch); break;

				default:
					return false;
			}

			return true;
		}

		public static void DrawMosaLogo(int v)
		{
			MosaLogo.Draw(Framebuffer, (uint)v);
		}

		public static void DrawPoint(int x, int y, Color color)
		{
			Framebuffer.SetPixel((uint)color.ToArgb(), (uint)x, (uint)y);
		}

		public static void DrawImage(int x, int y, Image image, bool drawWithAlpha)
		{
			Framebuffer.DrawImage(image, (uint)x, (uint)y, drawWithAlpha);
		}

		public static void DrawString(int x, int y, string text, string bitFont, Color color)
		{
			BitFont desc = new BitFont();

			for (int i1 = 0; i1 < Utils.Fonts.Count; i1++)
			{
				BitFont font = Utils.Fonts[i1];

				if (font.Name == bitFont)
				{
					desc = font;
					break;
				}
			}

			desc.DrawString(Framebuffer, (uint)color.ToArgb(), (uint)x, (uint)y, text);
		}

		public static void DrawRectangle(int x, int y, int width, int height, Color color, bool fill)
		{
			if (fill)
				Framebuffer.FillRectangle((uint)color.ToArgb(), (uint)x, (uint)y, (uint)width, (uint)height);
			else
				Framebuffer.DrawRectangle((uint)color.ToArgb(), (uint)x, (uint)y, (uint)width, (uint)height, 1);
		}

		public static bool IsInBounds(int x1, int x2, int y1, int y2, int width, int height)
		{
			return x1 >= x2 && x1 <= x2 + width && y1 >= y2 && y1 <= y2 + height;
		}

		public static void Clear(Color color)
		{
			Framebuffer.ClearScreen((uint)color.ToArgb());
		}

		public static void Update()
		{
			Framebuffer.SwapBuffers();
		}
	}
}
