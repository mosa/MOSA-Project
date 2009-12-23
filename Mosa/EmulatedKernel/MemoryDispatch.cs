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
		public static uint CR0;
		/// <summary>
		/// 
		/// </summary>
		public static uint CR3;

		/// <summary>
		/// 
		/// </summary>
		public static List<MemoryHandler> MemorySegments = new List<MemoryHandler>();

		/// <summary>
		/// Registers the memory.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="size">The size.</param>
		/// <param name="type">The type.</param>
		/// <param name="read8">The read8 delegate.</param>
		/// <param name="write8">The write8 delegate.</param>
		public static void RegisterMemory(uint address, uint size, byte type, MemoryRead8 read8, MemoryWrite8 write8)
		{
			MemorySegments.Add(new MemoryHandler(address, size, type, read8, write8));
		}

		/// <summary>
		/// Finds the specified address memory range.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns></returns>
		public static MemoryHandler Find(ulong address)
		{
			foreach (MemoryHandler memoryRange in MemorySegments)
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
			address = TranslateToPhysical(address);
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
			address = TranslateToPhysical(address);
			MemoryHandler memoryRange = Find(address);

			if (memoryRange != null)
				if (memoryRange.read8 != null)
					return memoryRange.read8(address);

			return 0;
		}

		/// <summary>
		/// Reads a byte from the specified address.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns></returns>
		private static byte PhysicalRead8(uint address)
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
			Write8(address, (byte)value);
			Write8(address + 1, (byte)(value >> 8));
		}

		/// <summary>
		/// Reads a short from the specified address.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns></returns>
		public static ushort Read16(uint address)
		{
			return (ushort)(Read8(address) | (Read8(address + 1) << 8));
		}

		/// <summary>
		/// Writes a short to the specified address.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="value">The value.</param>
		public static void Write24(uint address, uint value)
		{
			Write8(address, (byte)value);
			Write8(address + 1, (byte)(value >> 8));
			Write8(address + 2, (byte)(value >> 16));
		}

		/// <summary>
		/// Reads a short from the specified address.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns></returns>
		public static uint Read24(uint address)
		{
			return (uint)(Read8(address) | (Read8(address + 1) << 8) | (Read8(address + 2) << 16));
		}

		/// <summary>
		/// Reads an integer from the specified address.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns></returns>
		public static uint Read32(uint address)
		{
			return (uint)(Read8(address) | (Read8(address + 1) << 8) | (Read8(address + 2) << 16) | (Read8(address + 3) << 24));
		}

		/// <summary>
		/// Reads an integer from the specified address.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns></returns>
		public static uint PhysicalRead32(uint address)
		{
			return (uint)(PhysicalRead8(address) | (PhysicalRead8(address + 1) << 8) | (PhysicalRead8(address + 2) << 16) | (PhysicalRead8(address + 3) << 24));
		}

		/// <summary>
		/// Writes an integer to the specified address.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="value">The value.</param>
		public static void Write32(uint address, uint value)
		{
			Write8(address, (byte)value);
			Write8(address + 1, (byte)(value >> 8));
			Write8(address + 2, (byte)(value >> 16));
			Write8(address + 3, (byte)(value >> 24));
		}

		/// <summary>
		/// Reads an integer from the specified address.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns></returns>
		public static ulong Read64(uint address)
		{
			return (ulong)(Read8(address) | (Read8(address + 1) << 8) | (Read8(address + 2) << 16) | (Read8(address + 3) << 24) |
					Read8(address + 4) << 32 | (Read8(address + 5) << 40) | (Read8(address + 6) << 48) | (Read8(address + 7) << 56));
		}

		/// <summary>
		/// Translates to physical address.
		/// </summary>
		/// <param name="memory">The memory.</param>
		/// <returns></returns>
		public static uint TranslateToPhysical(uint memory)
		{
			if ((CR0 & 0x80000000) == 0)
				return memory;

			uint pdentry = PhysicalRead32(CR3 + ((memory >> 22) * sizeof(uint)));
			uint ptentry = PhysicalRead32((pdentry & 0xFFFFF000) + ((memory >> 12 & 0x03FF) * sizeof(uint)));

			return (memory & 0xFFF) | (ptentry & 0xFFFFF000);
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
