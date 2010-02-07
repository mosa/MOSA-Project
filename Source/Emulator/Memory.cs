/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Platforms.x86;

namespace Mosa.Kernel.X86
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
			for (uint at = start; (at < (start + bytes)); at++)
				Mosa.EmulatedKernel.MemoryDispatch.Write8(at, 0);
		}

		/// <summary>
		/// Sets the specified value at location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <param name="value">The value.</param>
		public static void Set32(uint location, uint value)
		{
			Mosa.EmulatedKernel.MemoryDispatch.Write32(location, value);
		}

		/// <summary>
		/// Sets the specified value at location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <param name="value">The value.</param>
		public static void Set8(uint location, byte value)
		{
			Mosa.EmulatedKernel.MemoryDispatch.Write8(location, value);
		}

		/// <summary>
		/// Sets the specified value at location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <param name="value">The value.</param>
		public static void Set16(uint location, ushort value)
		{
			Mosa.EmulatedKernel.MemoryDispatch.Write16(location, value);
		}

		/// <summary>
		/// Gets the value at specified location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <returns></returns>
		public static uint Get32(uint location)
		{
			return Mosa.EmulatedKernel.MemoryDispatch.Read32(location);
		}

		/// <summary>
		/// Gets the value at specified location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <returns></returns>
		public static byte Get8(uint location)
		{
			return Mosa.EmulatedKernel.MemoryDispatch.Read8(location);
		}

		/// <summary>
		/// Gets the value at specified location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <returns></returns>
		public static ulong Get64(ulong location)
		{
			return Mosa.EmulatedKernel.MemoryDispatch.Read64((uint)location);
		}

		/// <summary>
		/// Flushes the Translation Lookaside Buffer (TLB).
		/// </summary>
		/// <param name="address">The address.</param>
		public static void FlushTLB(uint address)
		{
			Native.Invlpg(address);
		}

		/// <summary>
		/// Gets or sets the Control Register 0.
		/// </summary>
		/// <value>The CR0.</value>
		public static uint CR0
		{
			get { return Native.GetControlRegister(0); }
			set { Native.SetControlRegister(0, value); }
		}

		/// <summary>
		/// Sets the Control Register 3.
		/// </summary>
		/// <value>The CR3.</value>
		public static uint CR3
		{
			set { Native.SetControlRegister(3, value); }
		}
	}
}
