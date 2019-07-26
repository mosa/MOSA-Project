// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal.Extension;
using Mosa.Runtime;
using Mosa.Runtime.x86;
using System;

namespace Mosa.Kernel.BareMetal.x86
{
	/// <summary>
	/// Screen
	/// </summary>
	public static class VGAText
	{
		private const int Address = 0x0B8000;
		private const uint Columns = 80;
		private const uint Rows = 40;

		private static short Offset = 0;

		private static byte Color { get; set; }

		private static byte BackgroundColor { get; set; }

		public static int Column
		{
			get { return (int)(Offset % Rows); }
			private set { Offset = (short)(Row * Rows + value); }
		}

		public static int Row
		{
			get { return (int)(Offset / Rows); }
			private set { Offset = (short)(Column * Columns + value); }
		}

		public static void SetColor(byte color)
		{
			Color = color;
		}

		public static void SetBackground(byte color)
		{
			BackgroundColor = color;
		}

		public static void Goto(int column, int row)
		{
			if (column > Columns || row > Rows)
				return;

			Row = row;
			Column = column;
			UpdateCursor();
		}

		public static void Newline()
		{
			Row++;
			Column = 0;
			CheckForScroll();
			UpdateCursor();
		}

		public static void Formfeed()
		{
			Row++;
			CheckForScroll();
			UpdateCursor();
		}

		public static void CarriageReturn()
		{
			Column = 0;
			UpdateCursor();
		}

		public static void Up()
		{
			if (Row == 0)
				return;

			Row--;
		}

		public static void Down()
		{
			Row++;
			CheckForScroll();
			UpdateCursor();
		}

		public static void Backspace()
		{
			if (Offset <= 0)
				return;

			Offset--;
			UpdateCursor();
		}

		public static void Clear()
		{
			ushort value = (ushort)(Color << 8 | (BackgroundColor & 0x0F) << 12);
			Runtime.Internal.MemorySet(new IntPtr(Address), value, (int)(Rows * Columns * 2));
			Offset = 0;
		}

		private static void Next()
		{
			Offset++;
			CheckForScroll();
			UpdateCursor();
		}

		public static void Write(byte b)
		{
			var address = new IntPtr(Address + (Offset * 2));

			address.Store8(0, b);
			address.Store8(1, (byte)(Color | ((BackgroundColor & 0x0F) << 4)));
			Next();
			UpdateCursor();
		}

		private static void CheckForScroll()
		{
			while (Offset >= Columns * Rows)
			{
				Runtime.Internal.MemoryCopy(new IntPtr(Address), new IntPtr(Address + (Columns * 2)), Columns * 2);
				Runtime.Internal.MemorySet(new IntPtr(Address + (Columns * (Rows - 1))), (byte)((BackgroundColor & 0x0F) << 4), (int)(Columns * 2));

				Offset = (short)(Offset - Columns);
			}
			UpdateCursor();
		}

		private static void UpdateCursor()
		{
			Native.Out8(0x3D4, 0x0F);
			Native.Out8(0x3D5, (byte)(Offset & 0xFF));

			Native.Out8(0x3D4, 0x0E);
			Native.Out8(0x3D5, (byte)((Offset >> 8) & 0xFF));
		}
	}
}
