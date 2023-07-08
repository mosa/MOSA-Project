// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;
using Mosa.Runtime;
using Mosa.Runtime.x86;

namespace Mosa.Kernel.BareMetal.x86;

/// <summary>
/// GDT
/// </summary>
public struct GDT
{
	private Pointer GDTTable;

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

	public void Setup()
	{
		Debug.WriteLine("GDTTable.Setup()");

		GDTTable = BootPageAllocator.AllocatePage();   // PhysicalPageAllocator is okay too

		GDTTable.Store16(0, GDTEntryOffset.TotalSize * 3 - 1);
		GDTTable.StorePointer(2, GDTTable + 6);

		Set(0, 0, 0, 0, 0);                // Null segment
		Set(1, 0, 0xFFFFFFFF, 0x9A, 0xCF); // Code segment
		Set(2, 0, 0xFFFFFFFF, 0x92, 0xCF); // Data segment

		SetLgdt(GDTTable);

		Debug.WriteLine("GDTTable.Setup() [Exit]");
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetLgdt(Pointer address)
	{
		Native.Lgdt(address.ToUInt32());
		Native.SetSegments(0x10, 0x10, 0x10, 0x10, 0x10);
		Native.FarJump();
	}

	private void Set(uint index, uint address, uint limit, byte access, byte granularity)
	{
		var entry = GDTTable + (6 + index * GDTEntryOffset.TotalSize);

		entry.Store16(GDTEntryOffset.BaseLow, (ushort)(address & 0xFFFF));
		entry.Store8(GDTEntryOffset.BaseMiddle, (byte)((address >> 16) & 0xFF));
		entry.Store8(GDTEntryOffset.BaseHigh, (byte)((address >> 24) & 0xFF));
		entry.Store16(GDTEntryOffset.LimitLow, (ushort)(limit & 0xFFFF));
		entry.Store8(GDTEntryOffset.Granularity, (byte)(((byte)(limit >> 16) & 0x0F) | (granularity & 0xF0)));
		entry.Store8(GDTEntryOffset.Access, access);
	}
}
