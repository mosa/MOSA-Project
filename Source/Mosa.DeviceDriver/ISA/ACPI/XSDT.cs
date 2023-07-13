// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceDriver.ISA.ACPI;

public struct XSDT
{
	private readonly Pointer Entry;

	public readonly Pointer Pointer => Entry;

	public readonly uint Size => Offset.Size;

	internal static class Offset
	{
		public const int ACPISDTHeader = 0;
		public const int PointerToOtherSDT = ACPI.ACPISDTHeader.Offset.Size + ACPISDTHeader;
		public const int Size = PointerToOtherSDT + (16 * 4);
	}

	public XSDT(Pointer entry) => Entry = entry;

	public readonly ACPISDTHeader ACPISDTHeader { get { return new ACPISDTHeader(Pointer + Offset.ACPISDTHeader); } }

	public uint GetPointerToOtherSDT(uint index) => Entry.Load32(Offset.PointerToOtherSDT + 16 * index);
}
