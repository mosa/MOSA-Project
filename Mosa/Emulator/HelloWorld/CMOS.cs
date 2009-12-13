/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using Mosa.Platforms.x86;

namespace Mosa.HelloWorld
{
	/// <summary>
	/// 
	/// </summary>
	public static class CMOS
	{

		/// <summary>
		/// Gets the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public static byte Get(byte index)
		{
			//Native.Cli();
			Native.Out(0x70, index);
			Native.Nop();
			Native.Nop();
			Native.Nop();
			byte result = (byte)Native.In(0x71);
			//Native.Sti();
			return result;
		}

		/// <summary>
		/// Sets the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static void Set(byte index, byte value)
		{
			//Native.Cli();
			Native.Out(0x70, index);
			Native.Nop();
			Native.Nop();
			Native.Nop();
			Native.Out(0x71, value);
			//Native.Sti();
		}

		/// <summary>
		/// Delays the io bus.
		/// </summary>
		private static void Delay()
		{
			Native.In(0x80);
			Native.Out(0x80, 0);
		}

		/// <summary>
		/// Gets the second.
		/// </summary>
		/// <value>The second.</value>
		public static byte Second { get { return Get(0); } }

		/// <summary>
		/// Gets the minute.
		/// </summary>
		/// <value>The minute.</value>
		public static byte Minute { get { return Get(2); } }

		/// <summary>
		/// Gets the hour.
		/// </summary>
		/// <value>The hour.</value>
		public static byte Hour { get { return Get(4); } }

		/// <summary>
		/// Gets the year.
		/// </summary>
		/// <value>The year.</value>
		public static byte Year { get { return Get(9); } }

		/// <summary>
		/// Gets the month.
		/// </summary>
		/// <value>The month.</value>
		public static byte Month { get { return Get(8); } }

		/// <summary>
		/// Gets the day.
		/// </summary>
		/// <value>The day.</value>
		public static byte Day { get { return Get(7); } }

		/// <summary>
		/// Gets the BCD.
		/// </summary>
		/// <value>The BCD.</value>
		public static bool BCD { get { return (Get(0x0B) & 0x04) == 0x00; } }

		/// <summary>
		/// Dump multiboot info.
		/// </summary>
		public static void Dump(uint row, uint col)
		{
			Screen.Row = row;
			Screen.Column = col;
			Screen.Color = 0x0A;
			Screen.Write('C');
			Screen.Write('M');
			Screen.Write('O');
			Screen.Write('S');
			Screen.Write(':');
			Screen.NextLine();

			for (byte i = 0; i < 19; i++) {
				Screen.Column = col;
				Screen.Color = 0x0F;
				Screen.Write(i, 16, 2);
				Screen.Write(':');
				Screen.Write(' ');
				Screen.Write(Get(i), 16, 2);
				Screen.Write(' ');
				Screen.Color = 0x07;
				Screen.Write(Get(i), 10, 3);
				Screen.NextLine();
			}
		}

	}
}
