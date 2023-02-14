// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal.GC;

public /*readonly*/ struct GCHeapList
{
	private readonly Pointer Entry;

	public GCHeapList(Pointer entry)
	{
		Entry = entry;
	}

	public uint Count
	{
		get => Entry.Load32();
		set => Entry.Store32(value);
	}

	public GCHeap GetGCHeapEntry(uint index)
	{
		return new GCHeap(Entry + sizeof(uint) + (Pointer.Size * 2 * index));
	}
}
