/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Kernel.Memory.X86
{
	/// <summary>
	/// Static class of helpful memory functions
	/// </summary>
	public static class Memory
	{
		/// <summary>
		/// Clears the specified memory area.
		/// </summary>
		/// <param name="start">The start.</param>
		/// <param name="bytes">The bytes.</param>
		public unsafe static void Clear(uint start, uint bytes)
		{
			byte* at = (byte*)start;
			byte* end = at + bytes;
			while (at <= end) {
				*at = 0;
				at++;
			}
		}

		/// <summary>
		/// Sets the specified value at location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <param name="value">The value.</param>
		public unsafe static void Set32(uint location, uint value)
		{
			uint* at = (uint*)location;
			*at = value;
		}

		/// <summary>
		/// Sets the specified value at location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <param name="value">The value.</param>
		public unsafe static void Set8(uint location, byte value)
		{
			byte* at = (byte*)location;
			*at = value;
		}

		/// <summary>
		/// Gets the value at specified location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <returns></returns>
		public unsafe static uint Get32(uint location)
		{
			uint* at = (uint*)location;
			return *at;
		}

		/// <summary>
		/// Gets the value at specified location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <returns></returns>
		public unsafe static byte Get8(uint location)
		{
			byte* at = (byte*)location;
			return *at;
		}

		/// <summary>
		/// Gets the value at specified location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <returns></returns>
		public unsafe static ulong Get64(uint location)
		{
			ulong* at = (ulong*)location;
			return *at;
		}

		/// <summary>
		/// Flushes the Translation Lookaside Buffer (TLB).
		/// </summary>
		public static void FlushTLB()
		{
			// TODO
			// Native.Invlpg(x)?
		}

		/// <summary>
		/// Flushes the Translation Lookaside Buffer (TLB).
		/// </summary>
		/// <param name="address">The address.</param>
		public static void FlushTLB(uint address)
		{
			// TODO
			// Native.Invlpg(Address)?
		}
	}
}
