// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceDriver.ISA.ACPI;

public struct ProcessorLocalAPICEntry
{
	private readonly Pointer Entry;

	public readonly Pointer Pointer => Entry;

	public readonly uint Size => Offset.Size;

	internal static class Offset
	{
		public const int Entry = 0;
		public const int AcpiProcessorID = MADTEntry.Offset.Size + Entry;
		public const int ApicID = AcpiProcessorID + 1;
		public const int Flags = ApicID + 1;
		public const int Size = Flags + 4;
	}

	public ProcessorLocalAPICEntry(Pointer entry) => Entry = entry;

	public readonly MADTEntry MADTEntry { get { return new MADTEntry(Pointer + Offset.Entry); } }

	public readonly byte AcpiProcessorID => Entry.Load8(Offset.AcpiProcessorID);

	public readonly byte ApicID => Entry.Load8(Offset.ApicID);

	public readonly uint Flags => Entry.Load32(Offset.Flags);
}
