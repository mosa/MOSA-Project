// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal.MultibootSpecification;

public class MultibootV2MemoryMapEntry
{
	private Pointer Entry;

	public readonly Pointer BaseAddress;
	public readonly ulong Length;
	public readonly uint Type;

	public MultibootV2MemoryMapEntry(Pointer entry)
	{
		Entry = entry;

		BaseAddress = (Pointer)entry.Load64(0);
		Length = entry.Load64(8);
		Type = entry.Load32(16);
	}

	public MultibootV2MemoryMapEntry GetNext() => new(Entry + Multiboot.V2.EntrySize);
}
