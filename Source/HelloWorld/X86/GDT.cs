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
	/// 
	/// </summary>
	public static class GDT
	{
		private static uint _gdtTable = 0x1401000;
		private static uint _gdtEntries = 0x1401000 + 6;

		internal const byte GDT_LimitLow = 0x00;
		internal const byte GDT_BaseLow = 0x02;
		internal const byte GDT_BaseMiddle = 0x04;
		internal const byte GDT_Access = 0x05;
		internal const byte GDT_Granularity = 0x06;
		internal const byte GDT_BaseHigh = 0x07;
		internal const byte GDT_Size = 0x08;

		public static void Setup()
		{
			Memory.Clear(_gdtTable, 6);
			Native.Set16(_gdtTable, (GDT_Size * 3) - 1);
			Native.Set32(_gdtTable + 2, _gdtEntries);

			Set(0, 0, 0, 0, 0);                // Null segment
			Set(1, 0, 0xFFFFFFFF, 0x9A, 0xCF); // Code segment
			Set(2, 0, 0xFFFFFFFF, 0x92, 0xCF); // Data segment

			Native.Lgdt(_gdtTable);
		}

		private static void Set(uint index, uint address, uint limit, byte access, byte granularity)
		{
			Native.Set16(_gdtEntries + (index * GDT_Size) + GDT_BaseLow, (ushort)(address & 0xFFFF));
			Native.Set8(_gdtEntries + (index * GDT_Size) + GDT_BaseMiddle, (byte)((address >> 16) & 0xFF));
			Native.Set8(_gdtEntries + (index * GDT_Size) + GDT_BaseHigh, (byte)((address >> 24) & 0xFF));
			Native.Set16(_gdtEntries + (index * GDT_Size) + GDT_LimitLow, (ushort)(limit & 0xFFFF));
			Native.Set8(_gdtEntries + (index * GDT_Size) + GDT_Granularity, (byte)(((byte)(limit >> 16) & 0x0F) | (granularity & 0xF0)));
			Native.Set8(_gdtEntries + (index * GDT_Size) + GDT_Access, access);
		}
	}
}
