// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal.MultibootSpecification;

/// <summary>
/// Multiboot V1 Memory Map
/// </summary>
public struct MultibootV1MemoryMapEntry
{
	public readonly Pointer Entry;

	#region Multiboot Memory Map Offsets

	private struct MultiBootMemoryMapOffset
	{
		public const byte Size = 0;
		public const byte BaseAddr = 4;
		public const byte Length = 12;
		public const byte Type = 20;
	}

	#endregion Multiboot Memory Map Offsets

	/// <summary>
	/// Setup Multiboot V1 Memory Map Entry.
	/// </summary>
	public MultibootV1MemoryMapEntry(Pointer entry) => Entry = entry;

	public uint Size => Entry.Load32(MultiBootMemoryMapOffset.Size);

	public Pointer BaseAddr => Entry.LoadPointer(MultiBootMemoryMapOffset.BaseAddr);

	public ulong Length => Entry.Load64(MultiBootMemoryMapOffset.Length);

	public byte Type => Entry.Load8(MultiBootMemoryMapOffset.Type);

	public MultibootV1MemoryMapEntry GetNext()
	{
		var next = Entry + Size + sizeof(uint);

		return new MultibootV1MemoryMapEntry(next);
	}
}
