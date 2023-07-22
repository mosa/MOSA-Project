// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceDriver.ISA.ACPI;

public struct ProcessorLocalAPICEntry
{
	public readonly Pointer Pointer;

	public readonly uint Size => Offset.Size;

	internal static class Offset
	{
		public const int Entry = 0;
		public const int AcpiProcessorID = MADTEntry.Offset.Size + Entry;
		public const int ApicID = AcpiProcessorID + 1;
		public const int Flags = ApicID + 1;
		public const int Size = Flags + 4;
	}

	public ProcessorLocalAPICEntry(Pointer entry) => Pointer = entry;

	public readonly MADTEntry MADTEntry { get { return new MADTEntry(Pointer + Offset.Entry); } }

	public readonly byte AcpiProcessorID => Pointer.Load8(Offset.AcpiProcessorID);

	public readonly byte ApicID => Pointer.Load8(Offset.ApicID);

	public readonly uint Flags => Pointer.Load32(Offset.Flags);
}
