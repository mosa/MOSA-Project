// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceDriver.ACPI;

/// <summary>
/// Describes a processor local APIC entry in the ACPI MADT.
/// </summary>
public readonly struct ProcessorLocalAPICEntry
{
	public readonly Pointer Pointer;

	public const uint Size = Offset.Size;

	private static class Offset
	{
		public const int Entry = 0;
		public const int AcpiProcessorID = MADTEntry.Offset.Size + Entry;
		public const int ApicID = AcpiProcessorID + 1;
		public const int Flags = ApicID + 1;
		public const int Size = Flags + 4;
	}

	public ProcessorLocalAPICEntry(Pointer entry) => Pointer = entry;

	public MADTEntry MADTEntry => new MADTEntry(Pointer + Offset.Entry);

	public byte AcpiProcessorID => Pointer.Load8(Offset.AcpiProcessorID);

	public byte ApicID => Pointer.Load8(Offset.ApicID);

	public uint Flags => Pointer.Load32(Offset.Flags);
}
