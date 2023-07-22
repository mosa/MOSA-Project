// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceDriver.ISA.ACPI;

public struct XSDT
{
	public readonly Pointer Pointer;

	public readonly uint Size => Offset.Size;

	internal static class Offset
	{
		public const int ACPISDTHeader = 0;
		public const int PointerToOtherSDT = ACPI.ACPISDTHeader.Offset.Size + ACPISDTHeader;
		public const int Size = PointerToOtherSDT + (16 * 4);
	}

	public XSDT(Pointer entry) => Pointer = entry;

	public readonly ACPISDTHeader ACPISDTHeader { get { return new ACPISDTHeader(Pointer + Offset.ACPISDTHeader); } }

	public uint GetPointerToOtherSDT(uint index) => Pointer.Load32(Offset.PointerToOtherSDT + 16 * index);
}
