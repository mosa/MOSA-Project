// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceDriver.ISA.ACPI;

public struct RSDT
{
	public readonly Pointer Pointer;

	public readonly uint Size => Offset.Size;

	internal static class Offset
	{
		public const int ACPISDTHeader = 0;
		public const int PointerToOtherSDT = ACPI.ACPISDTHeader.Offset.Size + ACPISDTHeader;
		public const int Size = PointerToOtherSDT + 8 * 4;
	}

	public RSDT(Pointer entry) => Pointer = entry;

	public readonly ACPISDTHeader ACPISDTHeader => new(Pointer + Offset.ACPISDTHeader);

	public Pointer GetPointerToOtherSDT(uint index) => new(Pointer.Load32(Offset.PointerToOtherSDT + 4 * index));
}
