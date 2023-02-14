﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.x86;

/// <summary>
/// Console Session
/// </summary>
public class ConsoleSession
{
	protected readonly byte[] text;
	protected readonly byte[] textcolor;
	protected uint column;
	protected uint row;
	protected byte color;
	protected ConsoleManager consoleManager;

	protected uint scrollRow;

	/// <summary>
	/// The columns
	/// </summary>
	public readonly uint Columns;

	/// <summary>
	/// The rows
	/// </summary>
	public readonly uint Rows;

	/// <summary>
	/// Gets or sets the column.
	/// </summary>
	/// <value>
	/// The column.
	/// </value>
	public uint Column
	{
		get => column;
		set { column = value; consoleManager.UpdateCursor(this); }
	}

	/// <summary>
	/// Gets or sets the row.
	/// </summary>
	/// <value>
	/// The row.
	/// </value>
	public uint Row
	{
		get => row;
		set { row = value; consoleManager.UpdateCursor(this); }
	}

	public uint ScrollRow
	{
		get => scrollRow;
		set => scrollRow = value;
	}

	public byte Color
	{
		get => (byte)(color & 0x0F);
		set { color &= 0xF0; color |= (byte)(value & 0x0F); }
	}

	public byte BackgroundColor
	{
		get => (byte)(color >> 4);
		set { color &= 0x0F; color |= (byte)((value & 0x0F) << 4); }
	}

	public ConsoleManager ConsoleManager
	{
		get => consoleManager;
		set => consoleManager = value;
	}

	public ConsoleSession(uint columns, uint rows, ConsoleManager consoleManager, uint scrollRow)
	{
		Columns = columns;
		Rows = rows;
		ConsoleManager = consoleManager;
		text = new byte[Columns * Rows];
		textcolor = new byte[Columns * Rows];
		ScrollRow = scrollRow;

		Color = ScreenColor.White;
		BackgroundColor = ScreenColor.Black;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="ConsoleSession"/> class.
	/// </summary>
	public ConsoleSession(ConsoleManager consoleManager)
		: this(80, 25, consoleManager, 25)
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
		if (Row >= ScrollRow)
		{
			Row--;
			ScrollUp();
		}

		uint address = Row * Columns + Column;

		text[address] = (byte)chr;
		textcolor[address] = color;

		if (consoleManager != null)
		{
			consoleManager.RawWrite(this, Row, Column, chr, color);
		}

		Next();
	}

	/// <summary>
	/// Writes the string to the screen.
	/// </summary>
	/// <param name="value">The string value to write to the screen.</param>
	public void Write(string value)
	{
		if (value == null)
			return;

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

		if (Row >= ScrollRow)
		{
			Row--;
			ScrollUp();
		}
	}

	/// <summary>
	/// Clears this instance.
	/// </summary>
	public void Clear()
	{
		GotoTop();

		for (uint r = 0; r < Rows; r++)
		{
			for (uint c = 0; c < Columns; c++)
			{
				uint address = r * Columns + c;

				text[address] = (byte)' ';
				textcolor[address] = BackgroundColor;

				if (consoleManager != null)
				{
					consoleManager.RawWrite(this, r, c, ' ', BackgroundColor);
				}
			}
		}
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
		for (uint r = 0; r < ScrollRow; r++)
		{
			for (uint c = 0; c < Columns; c++)
			{
				uint address = r * Columns + c;

				if (r == ScrollRow)
				{
					text[address] = 0;
					textcolor[address] = BackgroundColor;
				}
				else
				{
					text[address] = text[address + Columns];
					textcolor[address] = textcolor[address + Columns];
				}

				if (consoleManager != null)
				{
					consoleManager.RawWrite(this, r, c, (char)text[address], textcolor[address]);
				}
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
	public void Write(uint val)
	{
		Write(val, 10, -1);
	}

	public void WriteLine(uint val, byte digits, int size)
	{
		Write(val, digits, size);
		WriteLine();
	}

	/// <summary>
	/// Writes the specified value.
	/// </summary>
	/// <param name="val">The val.</param>
	/// <param name="digits">The digits.</param>
	/// <param name="size">The size.</param>
	public void Write(uint val, byte digits, int size)
	{
		uint count = 0;
		uint temp = val;

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
			uint digit = val % digits;
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
