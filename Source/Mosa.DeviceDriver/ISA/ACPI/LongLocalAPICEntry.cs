// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceDriver.ISA.ACPI;

public struct LongLocalAPICEntry
{
	public Pointer Pointer;

	public const uint Size = Offset.Size;

	private static class Offset
	{
		public const int Entry = 0;
		public const int Reserved = Entry + MADTEntry.Offset.Size;
		public const int ApicAddress = Reserved + 1;
		public const int Size = ApicAddress + 8;
	}

	public LongLocalAPICEntry(Pointer entry) => Pointer = entry;

	public MADTEntry MADTEntry => new MADTEntry(Pointer + Offset.Entry);

	public byte Reserved => Pointer.Load8(Offset.Reserved);

	public uint ApicAddress => Pointer.Load8(Offset.ApicAddress);
}
