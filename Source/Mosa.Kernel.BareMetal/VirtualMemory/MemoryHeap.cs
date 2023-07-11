// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal.VirtualMemory;

public struct MemoryHeap
{
	private readonly Pointer Entry;

	public MemoryHeap(Pointer entry)
	{
		Entry = entry;
	}

	public Pointer Address
	{
		get => Entry.LoadPointer();
		set => Entry.StorePointer(value);
	}

	public uint Size
	{
		get => Entry.Load32(Pointer.Size);
		set => Entry.Store32(Pointer.Size, value);
	}

	public uint Used
	{
		get => Entry.Load32(Pointer.Size + sizeof(uint));
		set => Entry.Store32(Pointer.Size + sizeof(uint), value);
	}

	public Pointer EndAddress => Address + Size;
}
