// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.x86;
using System;

namespace Mosa.Kernel.x86
{
	/// <summary>
	/// GDT
	/// </summary>
	public static class GDT
	{
		#region Data Members

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

		#endregion Data Members

		public static void Setup()
		{
			Runtime.Internal.MemoryClear(new IntPtr(Address.GDTTable), 6);
			Intrinsic.Store16(new IntPtr(Address.GDTTable), (Offset.TotalSize * 3) - 1);
			Intrinsic.Store32(new IntPtr(Address.GDTTable), 2, Address.GDTTable + 6);

			Set(0, 0, 0, 0, 0);                // Null segment
			Set(1, 0, 0xFFFFFFFF, 0x9A, 0xCF); // Code segment
			Set(2, 0, 0xFFFFFFFF, 0x92, 0xCF); // Data segment

			Native.Lgdt(Address.GDTTable);
		}

		private static void Set(uint index, uint address, uint limit, byte access, byte granularity)
		{
			var entry = GetEntryLocation(index);
			Intrinsic.Store16(entry, Offset.BaseLow, (ushort)(address & 0xFFFF));
			Intrinsic.Store8(entry, Offset.BaseMiddle, (byte)((address >> 16) & 0xFF));
			Intrinsic.Store8(entry, Offset.BaseHigh, (byte)((address >> 24) & 0xFF));
			Intrinsic.Store16(entry, Offset.LimitLow, (ushort)(limit & 0xFFFF));
			Intrinsic.Store8(entry, Offset.Granularity, (byte)(((byte)(limit >> 16) & 0x0F) | (granularity & 0xF0)));
			Intrinsic.Store8(entry, Offset.Access, access);
		}

		/// <summary>
		/// Gets the GTP entry location.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		private static IntPtr GetEntryLocation(uint index)
		{
			return new IntPtr(Address.GDTTable + 6 + (index * Offset.TotalSize));
		}
	}
}
