// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal.BootMemory;

public struct BootMemoryMapEntry
{
	private readonly Pointer Entry;

	public BootMemoryMapEntry(Pointer entry)
	{
		Entry = entry;
	}

	public Pointer StartAddress
	{
		get => Entry.LoadPointer();
		set => Entry.StorePointer(value);
	}

	public ulong Size
	{
		get => Entry.Load64(Pointer.Size);
		set => Entry.Store64(Pointer.Size, value);
	}

	public Pointer EndAddress => StartAddress + Size;

	public BootMemoryType Type
	{
		get => (BootMemoryType)Entry.Load8(Pointer.Size + sizeof(ulong));
		set => Entry.Store8(Pointer.Size + sizeof(ulong), (byte)value);
	}

	public bool IsAvailable => Type == BootMemoryType.Available;

	public static uint EntrySize => (uint)Pointer.Size + sizeof(ulong) + sizeof(byte);
}
