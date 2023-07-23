// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal.BootMemory;

public struct BootMemoryList
{
	private readonly Pointer Pointer;

	public BootMemoryList(Pointer entry) => Pointer = entry;

	public uint Count
	{
		get => Pointer.Load32();
		set => Pointer.Store32(value);
	}

	public BootMemoryMapEntry GetBootMemoryMapEntry(uint index)
	{
		return new BootMemoryMapEntry(Pointer + sizeof(uint) + BootMemoryMapEntry.EntrySize * index);
	}
}
