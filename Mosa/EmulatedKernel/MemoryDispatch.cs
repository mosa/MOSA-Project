/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceSystem;
using System.Collections.Generic;
using Mosa.EmulatedKernel;

namespace Mosa.EmulatedKernel
{

	/// <summary>
	/// 
	/// </summary>
	public static class MemoryDispatch
	{
		/// <summary>
		/// 
		/// </summary>
		private static List<MemoryHandler> dispatches = new List<MemoryHandler>();

		/// <summary>
		/// Registers the memory.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="size">The size.</param>
		/// <param name="read8">The read8 delegate.</param>
		/// <param name="write8">The write8 delegate.</param>
		public static void RegisterMemory(uint address, uint size, MemoryRead8 read8, MemoryWrite8 write8)
		{
			dispatches.Add(new MemoryHandler(address, size, read8, write8));
		}

		/// <summary>
		/// Finds the specified address memory range.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns></returns>
		public static MemoryHandler Find(uint address)
		{
			foreach (MemoryHandler memoryRange in dispatches)
				if (memoryRange.Contains(address))
					return memoryRange;

			return null;
		}

		/// <summary>
		/// Writes a byte to the specified address.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="value">The value.</param>
		public static void Write8(uint address, byte value)
		{
			MemoryHandler memoryRange = Find(address);

			if (memoryRange != null)
				if (memoryRange.write8 != null)
					memoryRange.write8(address, value);

			return;
		}

		/// <summary>
		/// Reads a byte from the specified address.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns></returns>
		public static byte Read8(uint address)
		{
			MemoryHandler memoryRange = Find(address);

			if (memoryRange != null)
				if (memoryRange.read8 != null)
					return memoryRange.read8(address);

			return 0;
		}

		/// <summary>
		/// Writes a short to the specified address.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="value">The value.</param>
		public static void Write16(uint address, ushort value)
		{
			MemoryHandler memoryRange = Find(address);

			if (memoryRange != null)
				if (memoryRange.write8 != null) {
					memoryRange.write8(address, (byte)value);
					memoryRange.write8(address + 1, (byte)(value >> 8));
				}

			return;
		}

		/// <summary>
		/// Reads a short from the specified address.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns></returns>
		public static ushort Read16(uint address)
		{
			MemoryHandler memoryRange = Find(address);

			if (memoryRange != null)
				if (memoryRange.read8 != null)
					return (ushort)(memoryRange.read8(address) | (memoryRange.read8(address + 1) << 8));

			return 0;
		}

        /// <summary>
        /// Writes a short to the specified address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="value">The value.</param>
        public static void Write24(uint address, uint value)
        {
            MemoryHandler memoryRange = Find(address);

            if (memoryRange != null)
                if (memoryRange.write8 != null)
                {
                    memoryRange.write8(address, (byte)value);
                    memoryRange.write8(address + 1, (byte)(value >>  8));
                    memoryRange.write8(address + 2, (byte)(value >> 16));
                }

            return;
        }

        /// <summary>
        /// Reads a short from the specified address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns></returns>
        public static ushort Read24(uint address)
        {
            MemoryHandler memoryRange = Find(address);

            if (memoryRange != null)
                if (memoryRange.read8 != null)
                    return (ushort)(memoryRange.read8(address) | (memoryRange.read8(address + 1) << 8) | (memoryRange.read8(address + 2) << 16));

            return 0;
        }

		/// <summary>
		/// Reads an integer from the specified address.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns></returns>
		public static ushort Read32(uint address)
		{
			MemoryHandler memoryRange = Find(address);

			if (memoryRange != null)
				if (memoryRange.write8 != null)
					return (ushort)(memoryRange.read8(address) | (memoryRange.read8(address + 1) << 8) | (memoryRange.read8(address + 2) << 16) | (memoryRange.read8(address + 3) << 24));

			return 0;
		}

		/// <summary>
		/// Writes an integer to the specified address.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="value">The value.</param>
		public static void Write32(uint address, uint value)
		{
			MemoryHandler memoryRange = Find(address);

			if (memoryRange != null)
				if (memoryRange.write8 != null) {
					memoryRange.write8(address, (byte)value);
					memoryRange.write8(address + 1, (byte)(value >> 8));
					memoryRange.write8(address + 2, (byte)(value >> 16));
					memoryRange.write8(address + 3, (byte)(value >> 24));
				}
			return;
		}

		/// <summary>
		/// Registers the memory.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="size">The size.</param>
		/// <returns></returns>
		public static IMemory RegisterMemory(uint address, uint size)
		{
			return new Memory(address, size);
		}

		/// <summary>
		/// 
		/// </summary>
		public delegate byte MemoryRead8(uint address);

		/// <summary>
		/// 
		/// </summary>
		public delegate void MemoryWrite8(uint address, byte value);
	}
}
