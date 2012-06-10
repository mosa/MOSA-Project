/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

namespace Mosa.Kernel.x86
{

	/// <summary>
	/// 
	/// </summary>
	public class ConsoleSession
	{
		protected readonly byte[] text;
		protected readonly byte[] textcolor;
		protected uint column = 0;
		protected uint row = 0;
		protected byte color = 0;
		protected ConsoleManager consoleManager;

		/// <summary>
		/// 
		/// </summary>
		public readonly uint Columns;

		/// <summary>
		/// 
		/// </summary>
		public readonly uint Rows;

		/// <summary>
		/// 
		/// </summary>
		public uint Column
		{
			get { return column; }
			set { column = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public uint Row
		{
			get { return row; }
			set { row = value; }
		}

		public byte Color
		{
			get { return (byte)(color & 0x0F); }
			set { color &= 0xF0; color |= (byte)(value & 0x0F); }
		}

		public byte BackgroundColor
		{
			get { return (byte)(color >> 4); }
			set { color &= 0x0F; color |= (byte)((value & 0x0F) << 4); }
		}

		public ConsoleManager ConsoleManager
		{
			get { return consoleManager; }
			set { consoleManager = value; }
		}

		public ConsoleSession(byte columns, byte rows, ConsoleManager consoleManager)
		{
			Columns = columns;
			Rows = rows;
			ConsoleManager = consoleManager;
			text = new byte[Columns * Rows];
			textcolor = new byte[Columns * Rows];
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ConsoleSession"/> class.
		/// </summary>
		public ConsoleSession(ConsoleManager consoleManager)
			: this(80, 40, consoleManager)
		{
		}

		/// <summary>
		/// Next Column
		/// </summary>
		private void Next()
		{
			Column++;

			if (Column >= Columns)
			{
				Column = 0;
				Row++;
			}

		}

		/// <summary>
		/// Writes the character.
		/// </summary>
		/// <param name="chr">The character.</param>
		public void Write(char chr)
		{
			if (Row >= Rows)
			{
				Row--;
				ScrollUp();
			}

			uint address = Row * Columns + Column;

			text[address] = (byte)chr;
			textcolor[address] = (byte)color;

			if (consoleManager != null)
				consoleManager.RawWrite(this, Row, Column, chr, (byte)color);

			Next();
		}

		/// <summary>
		/// Writes the string to the screen.
		/// </summary>
		/// <param name="value">The string value to write to the screen.</param>
		public void Write(string value)
		{
			for (int index = 0; index < value.Length; index++)
			{
				char chr = value[index];
				Write(chr);
			}
		}

		/// <summary>
		/// Writes the string to the screen.
		/// </summary>
		/// <param name="value">The string value to write to the screen.</param>
		public void WriteLine(string value)
		{
			Write(value);
			WriteLine();
		}
		/// <summary>
		/// Goto the top.
		/// </summary>
		public void GotoTop()
		{
			Column = 0;
			Row = 0;
		}

		/// <summary>
		/// Goto the next line.
		/// </summary>
		public void WriteLine()
		{
			Column = 0;
			Row++;
		}

		/// <summary>
		/// Clears this instance.
		/// </summary>
		public void Clear()
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
		public void Goto(uint row, uint col)
		{
			Row = row;
			Column = col;
		}

		/// <summary>
		/// Scrolls up.
		/// </summary>
		protected void ScrollUp()
		{
			for (uint row = 0; row < Rows - 1; row++)
			{
				for (uint column = 0; column < Columns; column++)
				{
					uint address = row * Columns + column;

					if (row != Rows - 1)
					{
						text[address] = text[address + Columns];
						textcolor[address] = textcolor[address + Columns];
					}
					else
					{
						text[address] = 0;
						textcolor[address] = BackgroundColor;
					}

					if (consoleManager != null)
						consoleManager.RawWrite(this, row, column, (char)text[address], textcolor[address]);
				}
			}
		}

		public char GetText(byte column, byte row)
		{
			uint address = (row * Columns + column);
			return (char)text[address];
		}

		public byte GetColor(byte column, byte row)
		{
			uint address = (row * Columns + column);
			return textcolor[address];
		}

		/// <summary>
		/// Skips the specified skip.
		/// </summary>
		/// <param name="skip">The skip.</param>
		private void Skip(uint skip)
		{
			for (uint i = 0; i < skip; i++)
				Next();
		}
		
		/// <summary>
		/// Writes the specified value.
		/// </summary>
		/// <param name="val">The val.</param>
		/// <param name="digits">The digits.</param>
		/// <param name="size">The size.</param>
		public void Write(ulong val, byte digits, int size)
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
