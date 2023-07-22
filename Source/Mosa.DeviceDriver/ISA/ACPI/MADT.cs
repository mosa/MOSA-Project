// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceDriver.ISA.ACPI;

public struct MADT
{
	public readonly Pointer Pointer;

	public readonly uint Size => Offset.Size;

	internal static class Offset
	{
		public const int ACPISDTHeader = 0;
		public const int LocalApicAddress = ACPISDTHeader + ACPI.ACPISDTHeader.Offset.Size;
		public const int Flags = LocalApicAddress + 4;
		public const int Size = Flags + 4;
	}

	public MADT(Pointer entry) => Pointer = entry;

	public readonly ACPISDTHeader ACPISDTHeader { get { return new ACPISDTHeader(Pointer + Offset.ACPISDTHeader); } }

	public readonly uint LocalApicAddress => Pointer.Load32(Offset.LocalApicAddress);

	public readonly uint Flags => Pointer.Load32(Offset.Flags);
}
