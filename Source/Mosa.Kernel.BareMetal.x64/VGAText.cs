﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.x64;

// copied from Mosa.Kernel.BareMetal.x86

namespace Mosa.Kernel.BareMetal.x64
{
	/// <summary>
	/// Screen
	/// </summary>
	public static class VGAText
	{
		private const int Address = 0x0B8000;
		private const uint Columns = 80;
		private const uint Rows = 25;

		private static short Offset = 0;

		private static byte Color { get; set; }

		private static byte BackgroundColor { get; set; }

		public static byte Column
		{
			get { return (byte)(Offset % Columns); }
			private set { Offset = (short)((Columns * Row) + value); }
		}

		public static byte Row
		{
			get { return (byte)(Offset / Columns); }
			private set { Offset = (short)((Columns * value) + Column); }
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

			Row = (byte)row;
			Column = (byte)column;
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
			Runtime.Internal.MemorySet(new Pointer(Address), value, (int)(Rows * Columns * 2));
			Goto(0, 0);
		}

		private static void Next()
		{
			Offset++;
			CheckForScroll();
			UpdateCursor();
		}

		public static void Write(byte b)
		{
			var address = new Pointer(Address + (Offset * 2));

			address.Store8(0, b);
			address.Store8(1, (byte)(Color | ((BackgroundColor & 0x0F) << 4)));
			Next();
		}

		private static void CheckForScroll()
		{
			while (Offset >= Columns * Rows)
			{
				Runtime.Internal.MemoryCopy(new Pointer(Address), new Pointer(Address + (Columns * 2)), Columns * 2);
				Runtime.Internal.MemorySet(new Pointer(Address + (Columns * (Rows - 1))), (byte)((BackgroundColor & 0x0F) << 4), (int)(Columns * 2));

				Offset = (short)(Offset - Columns);
			}
			UpdateCursor();
		}

		private static void UpdateCursor()
		{
			var location = (Row * Columns) + Column;

			Native.Out8(0x3D4, 0x0F);
			Native.Out8(0x3D5, (byte)(location & 0xFF));

			Native.Out8(0x3D4, 0x0E);
			Native.Out8(0x3D5, (byte)((location >> 8) & 0xFF));
		}
	}
}
