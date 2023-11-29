// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal.MultibootSpecification;

public struct MultibootV2MemoryMapEntry
{
	private readonly Pointer Pointer;

	public Pointer BaseAddress => Pointer;

	public ulong Length => Pointer.Load64(8);

	public uint Type => Pointer.Load32(16);

	public MultibootV2MemoryMapEntry(Pointer entry)
	{
		Pointer = entry;
	}

	public MultibootV2MemoryMapEntry GetNext() => new(Pointer + Multiboot.V2.EntrySize);
}
