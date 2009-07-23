/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */
using Mosa.Kernel.Memory.X86;
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
			Native.Out8(0x70, index);
			return Native.In8(0x71);
		}

		/// <summary>
		/// Sets the specified index.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static void Set(byte index, byte value)
		{
			Native.Out8(0x70, index);
			Native.Out8(0x70, index);
			Native.Out8(0x71, value);
			Native.Out8(0x71, value);
		}

		/// <summary>
		/// Initializes the clock.
		/// </summary>
		public static void InitializeClock()
		{
			Set(0x0B, 0x06);
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

	}
}
