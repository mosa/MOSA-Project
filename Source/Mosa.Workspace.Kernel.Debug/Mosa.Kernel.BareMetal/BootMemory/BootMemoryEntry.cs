// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal.BootMemory;

public /*readonly*/ struct BootMemoryEntry
{
	private readonly Pointer Entry;

	public BootMemoryEntry(Pointer entry)
	{
		Entry = entry;
	}

	public Pointer StartAddress
	{
		get { return Entry.LoadPointer(); }
		set { Entry.StorePointer(value); }
	}

	public ulong Size
	{
		get { return Entry.Load64(Pointer.Size); }
		set { Entry.Store64(Pointer.Size, value); }
	}

	public Pointer EndAddress
	{
		get { return StartAddress + Size; }
	}

	public BootMemoryType Type
	{
		get { return (BootMemoryType)Entry.Load8(Pointer.Size + sizeof(ulong)); }
		set { Entry.Store8(Pointer.Size + sizeof(ulong), (byte)value); }
	}

	public bool IsAvailable { get { return Type == BootMemoryType.Available; } }

	public static uint EntrySize => (uint)Pointer.Size + sizeof(ulong) + sizeof(byte);
}
