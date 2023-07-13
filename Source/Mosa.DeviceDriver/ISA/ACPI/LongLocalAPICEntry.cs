// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceDriver.ISA.ACPI;

public struct LongLocalAPICEntry
{
	private readonly Pointer Entry;

	public readonly Pointer Pointer => Entry;

	public readonly uint Size = Offset.Size;

	internal static class Offset
	{
		public const int Entry = 0;
		public const int Reserved = Entry + ACPI.MADTEntry.Offset.Size;
		public const int ApicAddress = Reserved + 1;
		public const int Size = ApicAddress + 8;
	}

	public LongLocalAPICEntry(Pointer entry) => Entry = entry;

	public readonly MADTEntry MADTEntry { get { return new MADTEntry(Pointer + Offset.Entry); } }

	public readonly byte Reserved => Entry.Load8(Offset.Reserved);

	public readonly uint ApicAddress => Entry.Load8(Offset.ApicAddress);
}
