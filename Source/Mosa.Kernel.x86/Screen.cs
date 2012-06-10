/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using Mosa.Platform.x86.Intrinsic;

namespace Mosa.Kernel.x86
{

	/// <summary>
	/// 
	/// </summary>
	public static class Screen
	{
		public static uint column = 0;
		public static uint row = 0;
		private static byte color = 0;

		/// <summary>
		/// 
		/// </summary>
		public const uint Columns = 80;

		/// <summary>
		/// 
		/// </summary>
		public const uint Rows = 40;

		/// <summary>
		/// 
		/// </summary>
		public static uint Column
		{
			get { return column; }
			set { column = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public static uint Row
		{
			get { return row; }
			set { row = value; }
		}

		public static byte Color
		{
			get { return (byte)(color & 0x0F); }
			set { color &= 0xF0; color |= (byte)(value & 0x0F); }
		}

		public static byte BackgroundColor
		{
			get { return (byte)(color >> 4); }
			set { color &= 0x0F; color |= (byte)((value & 0x0F) << 4); }
		}

		/// <summary>
		/// Next Column
		/// </summary>
		private static void Next()
		{
			Column++;

			if (Column >= Columns)
			{
				Column = 0;
				Row++;
			}
		}

		/// <summary>
		/// Skips the specified skip.
		/// </summary>
		/// <param name="skip">The skip.</param>
		private static void Skip(uint skip)
		{
			for (uint i = 0; i < skip; i++)
				Next();
		}

		/// <summary>
		/// Writes the character.
		/// </summary>
		/// <param name="chr">The character.</param>
		public static void RawWrite(uint row, uint column, char chr, byte color)
		{
			uint address = (0x0B8000 + ((row * Columns + column) * 2));

			Native.Set8(address, (byte)chr);
			Native.Set8(address + 1, color);
		}

		/// <summary>
		/// Writes the character.
		/// </summary>
		/// <param name="chr">The character.</param>
		public static void Write(char chr)
		{
			uint address = (0x0B8000 + ((Row * Columns + Column) * 2));

			Native.Set8(address, (byte)chr);
			Native.Set8(address + 1, color);

			Next();
		}

		/// <summary>
		/// Writes the string to the screen.
		/// </summary>
		/// <param name="value">The string value to write to the screen.</param>
		public static void Write(string value)
		{
			for (int index = 0; index < value.Length; index++)
			{
				char chr = value[index];
				Write(chr);
			}
		}

		/// <summary>
		/// Goto the top.
		/// </summary>
		public static void GotoTop()
		{
			Column = 0;
			Row = 0;
		}

		/// <summary>
		/// Goto the next line.
		/// </summary>
		public static void NextLine()
		{
			Column = 0;
			Row++;
		}

		/// <summary>
		/// Clears this instance.
		/// </summary>
		public static void Clear()
		{
			GotoTop();

			byte c = Color;
			Color = 0x0A;

			for (int i = 0; i < Columns * Rows; i++)
				Write(' ');

			Color = c;
			GotoTop();
		}

		/// <summary>
		/// Sets the cursor.
		/// </summary>
		/// <param name="row">The row.</param>
		/// <param name="col">The col.</param>
		public static void Goto(uint row, uint col)
		{
			Row = row;
			Column = col;
		}

		/// <summary>
		/// Writes the specified value.
		/// </summary>
		/// <param name="val">The val.</param>
		public static void Write(uint val)
		{
			Write(val, 10, -1);
		}

		/// <summary>
		/// Writes the specified value.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <param name="digits">The digits.</param>
		/// <param name="size">The size.</param>
		public static void Write(ulong val, byte digits, int size)
		{
			uint count = 0;
			ulong temp = val;

			do
			{
				temp /= digits;
				count++;
			} while (temp != 0);

			if (size != -1)
				count = (uint)size;

			uint x = Column;
			uint y = Row;

			for (uint i = 0; i < count; i++)
			{
				uint digit = (uint)(val % digits);
				Column = x;
				Row = y;
				Skip(count - 1 - i);
				if (digit < 10)
					Write((char)('0' + digit));
				else
					Write((char)('A' + digit - 10));
				val /= digits;
			}

			Column = x;
			Row = y;
			Skip(count);
		}

	}
}
