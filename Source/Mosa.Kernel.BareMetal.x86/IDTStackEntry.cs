// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal.BootMemory;

public struct IDTStackEntry
{
	private readonly Pointer Entry;

	public IDTStackEntry(Pointer entry)
	{
		Entry = entry;
	}

	public readonly uint EDI => Entry.Load32(0x00);

	public readonly uint ESI => Entry.Load32(0x04);

	public readonly uint EBP => Entry.Load32(0x08);

	public readonly uint ESP => Entry.Load32(0x0C);

	public readonly uint EBX => Entry.Load32(0x10);

	public readonly uint EDX => Entry.Load32(0x14);

	public readonly uint ECX => Entry.Load32(0x18);

	public readonly uint EAX => Entry.Load32(0x1C);

	public readonly uint Interrupt => Entry.Load32(0x20);

	public readonly uint ErrorCode => Entry.Load32(0x24);

	public readonly uint EIP => Entry.Load32(0x28);

	public readonly uint CS => Entry.Load32(0x2C);

	public readonly uint EFLAGS => Entry.Load32(0x30);
}
