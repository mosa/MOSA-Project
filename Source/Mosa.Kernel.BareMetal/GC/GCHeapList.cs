// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal.GC;

public struct GCHeapList
{
	private readonly Pointer Pointer;

	public GCHeapList(Pointer entry) => Pointer = entry;

	public uint Count
	{
		get => Pointer.Load32();
		set => Pointer.Store32(value);
	}

	public GCHeap GetHeapEntry(uint index)
	{
		return new GCHeap(Pointer + sizeof(uint) + Pointer.Size * 2 * index);
	}
}
