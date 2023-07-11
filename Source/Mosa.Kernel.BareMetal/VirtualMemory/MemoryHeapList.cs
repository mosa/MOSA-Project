// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal.VirtualMemory;

public struct MemoryHeapList
{
	private readonly Pointer Entry;

	public MemoryHeapList(Pointer entry)
	{
		Entry = entry;
	}

	public uint Count
	{
		get => Entry.Load32();
		set => Entry.Store32(value);
	}

	public MemoryHeap GetHeapEntry(uint index)
	{
		return new MemoryHeap(Entry + sizeof(uint) + Pointer.Size * 2 * index);
	}
}
