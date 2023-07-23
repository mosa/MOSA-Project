// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal.VirtualMemory;

public struct MemoryHeapList
{
	private readonly Pointer Pointer;

	public MemoryHeapList(Pointer entry)
	{
		Pointer = entry;
	}

	public uint Count
	{
		get => Pointer.Load32();
		set => Pointer.Store32(value);
	}

	public MemoryHeap GetHeapEntry(uint index)
	{
		return new MemoryHeap(Pointer + sizeof(uint) + Pointer.Size * 2 * index);
	}
}
