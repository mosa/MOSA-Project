// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceDriver.ACPI;

/// <summary>
/// The ACPI Root System Description Table.
/// </summary>
public readonly struct RSDT
{
	public readonly Pointer Pointer;

	public const uint Size = Offset.Size;

	private static class Offset
	{
		public const int ACPISDTHeader = 0;
		public const int PointerToOtherSDT = ACPI.ACPISDTHeader.Offset.Size + ACPISDTHeader;
		public const int Size = PointerToOtherSDT + 8 * 4;
	}

	public RSDT(Pointer entry) => Pointer = entry;

	public ACPISDTHeader ACPISDTHeader => new ACPISDTHeader(Pointer + Offset.ACPISDTHeader);

	public Pointer GetPointerToOtherSDT(uint index) => new Pointer(Pointer.Load32(Offset.PointerToOtherSDT + 4 * index));
}
