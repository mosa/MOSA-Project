/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Platform.x86.Intrinsic;

namespace Mosa.Kernel.x86
{
	/// <summary>
	///
	/// </summary>
	public static class GDT
	{
		private static uint _gdtTable = 0x1401000;
		private static uint _gdtEntries = 0x1401000 + 6;

		#region Data members

		internal struct Offset
		{
			internal const byte LimitLow = 0x00;
			internal const byte BaseLow = 0x02;
			internal const byte BaseMiddle = 0x04;
			internal const byte Access = 0x05;
			internal const byte Granularity = 0x06;
			internal const byte BaseHigh = 0x07;
			internal const byte TotalSize = 0x08;
		}

		#endregion Data members

		public static void Setup()
		{
			Memory.Clear(_gdtTable, 6);
			Native.Set16(_gdtTable, (Offset.TotalSize * 3) - 1);
			Native.Set32(_gdtTable + 2, _gdtEntries);

			Set(0, 0, 0, 0, 0);                // Null segment
			Set(1, 0, 0xFFFFFFFF, 0x9A, 0xCF); // Code segment
			Set(2, 0, 0xFFFFFFFF, 0x92, 0xCF); // Data segment

			Native.Lgdt(_gdtTable);
		}

		private static void Set(uint index, uint address, uint limit, byte access, byte granularity)
		{
			uint entry = GetEntryLocation(index);
			Native.Set16(entry + Offset.BaseLow, (ushort)(address & 0xFFFF));
			Native.Set8(entry + Offset.BaseMiddle, (byte)((address >> 16) & 0xFF));
			Native.Set8(entry + Offset.BaseHigh, (byte)((address >> 24) & 0xFF));
			Native.Set16(entry + Offset.LimitLow, (ushort)(limit & 0xFFFF));
			Native.Set8(entry + Offset.Granularity, (byte)(((byte)(limit >> 16) & 0x0F) | (granularity & 0xF0)));
			Native.Set8(entry + Offset.Access, access);
		}

		/// <summary>
		/// Gets the gdt entry location.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		private static uint GetEntryLocation(uint index)
		{
			return (uint)(_gdtEntries + (index * Offset.TotalSize));
		}
	}
}