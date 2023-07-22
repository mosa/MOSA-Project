// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal.BootMemory;

public struct IDTStackEntry
{
	public readonly Pointer Pointer;

	public IDTStackEntry(Pointer entry)
	{
		Pointer = entry;
	}

	public readonly uint EDI => Pointer.Load32(0x00);

	public readonly uint ESI => Pointer.Load32(0x04);

	public readonly uint EBP => Pointer.Load32(0x08);

	public readonly uint ESP => Pointer.Load32(0x0C);

	public readonly uint EBX => Pointer.Load32(0x10);

	public readonly uint EDX => Pointer.Load32(0x14);

	public readonly uint ECX => Pointer.Load32(0x18);

	public readonly uint EAX => Pointer.Load32(0x1C);

	public readonly uint Interrupt => Pointer.Load32(0x20);

	public readonly uint ErrorCode => Pointer.Load32(0x24);

	public readonly uint EIP => Pointer.Load32(0x28);

	public readonly uint CS => Pointer.Load32(0x2C);

	public readonly uint EFLAGS => Pointer.Load32(0x30);
}
