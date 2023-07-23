// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal.BootMemory;

public struct BootMemoryMapEntry
{
	private readonly Pointer Pointer;

	public BootMemoryMapEntry(Pointer entry) => Pointer = entry;

	public Pointer StartAddress
	{
		get => Pointer.LoadPointer();
		set => Pointer.StorePointer(value);
	}

	public ulong Size
	{
		get => Pointer.Load64(Pointer.Size);
		set => Pointer.Store64(Pointer.Size, value);
	}

	public Pointer EndAddress => StartAddress + Size;

	public BootMemoryType Type
	{
		get => (BootMemoryType)Pointer.Load8(Pointer.Size + sizeof(ulong));
		set => Pointer.Store8(Pointer.Size + sizeof(ulong), (byte)value);
	}

	public bool IsAvailable => Type == BootMemoryType.Available;

	public static uint EntrySize => (uint)Pointer.Size + sizeof(ulong) + sizeof(byte);
}
