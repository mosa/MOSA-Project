// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceDriver.ISA.ACPI;

public struct LongLocalAPICEntry
{
	public readonly Pointer Pointer;

	public readonly uint Size = Offset.Size;

	internal static class Offset
	{
		public const int Entry = 0;
		public const int Reserved = Entry + ACPI.MADTEntry.Offset.Size;
		public const int ApicAddress = Reserved + 1;
		public const int Size = ApicAddress + 8;
	}

	public LongLocalAPICEntry(Pointer entry) => Pointer = entry;

	public readonly MADTEntry MADTEntry { get { return new MADTEntry(Pointer + Offset.Entry); } }

	public readonly byte Reserved => Pointer.Load8(Offset.Reserved);

	public readonly uint ApicAddress => Pointer.Load8(Offset.ApicAddress);
}
