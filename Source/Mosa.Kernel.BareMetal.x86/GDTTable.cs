// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.x86;
using System.Runtime.CompilerServices;

namespace Mosa.Kernel.BareMetal.x86
{
	/// <summary>
	/// GDT
	/// </summary>
	public /*readonly*/ struct GDTTable
	{
		private /*readonly*/ Pointer Entry;

		#region GDT Entry Offsets

		internal struct GDTEntryOffset
		{
			internal const byte LimitLow = 0x00;
			internal const byte BaseLow = 0x02;
			internal const byte BaseMiddle = 0x04;
			internal const byte Access = 0x05;
			internal const byte Granularity = 0x06;
			internal const byte BaseHigh = 0x07;
			internal const byte TotalSize = 0x08;
		}

		#endregion GDT Entry Offsets

		public GDTTable(Pointer entry)
		{
			Entry = entry;
		}

		public void Setup()
		{
			Entry.Store16(0, (GDTEntryOffset.TotalSize * 3) - 1);
			Entry.StorePointer(2, Entry + 6);

			Set(0, 0, 0, 0, 0);                // Null segment
			Set(1, 0, 0xFFFFFFFF, 0x9A, 0xCF); // Code segment
			Set(2, 0, 0xFFFFFFFF, 0x92, 0xCF); // Data segment
		}

		public void Enable()
		{
			SetLgdt(Entry);
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void SetLgdt(Pointer address)
		{
			Native.Lgdt((uint)address.ToInt32());
			Native.SetSegments(0x10, 0x10, 0x10, 0x10, 0x10);
			Native.FarJump();
		}

		private void Set(uint index, uint address, uint limit, byte access, byte granularity)
		{
			var entry = Entry + (6 + (index * GDTEntryOffset.TotalSize));

			entry.Store16(GDTEntryOffset.BaseLow, (ushort)(address & 0xFFFF));
			entry.Store8(GDTEntryOffset.BaseMiddle, (byte)((address >> 16) & 0xFF));
			entry.Store8(GDTEntryOffset.BaseHigh, (byte)((address >> 24) & 0xFF));
			entry.Store16(GDTEntryOffset.LimitLow, (ushort)(limit & 0xFFFF));
			entry.Store8(GDTEntryOffset.Granularity, (byte)(((byte)(limit >> 16) & 0x0F) | (granularity & 0xF0)));
			entry.Store8(GDTEntryOffset.Access, access);
		}
	}
}
