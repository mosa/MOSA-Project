// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceDriver.ACPI;

/// <summary>
/// The ACPI eXtended System Description Table.
/// </summary>
public readonly struct XSDT
{
	public readonly Pointer Pointer;

	public const uint Size = Offset.Size;

	private static class Offset
	{
		public const int ACPISDTHeader = 0;
		public const int PointerToOtherSDT = ACPI.ACPISDTHeader.Offset.Size + ACPISDTHeader;
		public const int Size = PointerToOtherSDT + 16 * 4;
	}

	public XSDT(Pointer entry) => Pointer = entry;

	public ACPISDTHeader ACPISDTHeader => new ACPISDTHeader(Pointer + Offset.ACPISDTHeader);

	public Pointer GetPointerToOtherSDT(uint index) => new Pointer(Pointer.Load64(Offset.PointerToOtherSDT + 16 * index));
}
