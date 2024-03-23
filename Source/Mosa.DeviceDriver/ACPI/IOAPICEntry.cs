// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceDriver.ACPI;

/// <summary>
/// Describes an I/O APIC entry in the ACPI MADT.
/// </summary>
public readonly struct IOAPICEntry
{
	public readonly Pointer Pointer;

	public const uint Size = Offset.Size;

	private static class Offset
	{
		public const int MADTEntry = 0;
		public const int ApicID = MADTEntry + 2;
		public const int Reserved = ApicID + 1;
		public const int ApicAddress = Reserved + 1;
		public const int GlobalSystemInterruptBase = ApicAddress + 4;
		public const int Size = GlobalSystemInterruptBase + 4;
	}

	public IOAPICEntry(Pointer entry) => Pointer = entry;

	public MADTEntry MADTEntry => new MADTEntry(Pointer + Offset.MADTEntry);

	public byte ApicID => Pointer.Load8(Offset.ApicID);

	public byte Reserved => Pointer.Load8(Offset.Reserved);

	public uint ApicAddress => Pointer.Load8(Offset.ApicAddress);

	public uint GlobalSystemInterruptBase => Pointer.Load8(Offset.GlobalSystemInterruptBase);
}
