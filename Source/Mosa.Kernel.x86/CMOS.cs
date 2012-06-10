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
	public class CMOS
	{
		/// <summary>
		/// Gets the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public byte Get(byte index)
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
		public void Set(byte index, byte value)
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
		private void Delay()
		{
			Native.In(0x80);
			Native.Out(0x80, 0);
		}

		/// <summary>
		/// Gets the second.
		/// </summary>
		/// <value>The second.</value>
		public byte Second
		{
			get
			{
				return Get(0);
			}
		}

		/// <summary>
		/// Gets the minute.
		/// </summary>
		/// <value>The minute.</value>
		public byte Minute
		{
			get
			{
				return Get(2);
			}
		}

		/// <summary>
		/// Gets the hour.
		/// </summary>
		/// <value>The hour.</value>
		public byte Hour
		{
			get
			{
				return Get(4);
			}
		}

		/// <summary>
		/// Gets the year.
		/// </summary>
		/// <value>The year.</value>
		public byte Year
		{
			get
			{
				return Get(9);
			}
		}

		/// <summary>
		/// Gets the month.
		/// </summary>
		/// <value>The month.</value>
		public byte Month
		{
			get
			{
				return Get(8);
			}
		}

		/// <summary>
		/// Gets the day.
		/// </summary>
		/// <value>The day.</value>
		public byte Day
		{
			get
			{
				return Get(7);
			}
		}

		/// <summary>
		/// Gets the BCD.
		/// </summary>
		/// <value>The BCD.</value>
		public bool BCD
		{
			get
			{
				return (Get(0x0B) & 0x04) == 0x00;
			}
		}

		/// <summary>
		/// Dump multiboot info.
		/// </summary>
		public void Dump(ConsoleSession console, uint row, uint col)
		{
			console.Row = row;
			console.Column = col;
			console.Color = 0x0A;
			console.Write(@"CMOS:");
			console.WriteLine();

			for (byte i = 0; i < 19; i++)
			{
				console.Column = col;
				console.Color = 0x0F;
				console.Write(i, 16, 2);
				console.Write(':');
				console.Write(' ');
				console.Write(Get(i), 16, 2);
				console.Write(' ');
				console.Color = 0x07;
				console.Write(Get(i), 10, 3);
				console.WriteLine();
			}
		}
	}
}
