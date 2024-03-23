// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceDriver.ACPI;

public readonly struct MADT
{
	public readonly Pointer Pointer;

	public const uint Size = Offset.Size;

	private static class Offset
	{
		public const int ACPISDTHeader = 0;
		public const int LocalApicAddress = ACPISDTHeader + ACPI.ACPISDTHeader.Offset.Size;
		public const int Flags = LocalApicAddress + 4;
		public const int Size = Flags + 4;
	}

	public MADT(Pointer entry) => Pointer = entry;

	public ACPISDTHeader ACPISDTHeader => new ACPISDTHeader(Pointer + Offset.ACPISDTHeader);

	public uint LocalApicAddress => Pointer.Load32(Offset.LocalApicAddress);

	public uint Flags => Pointer.Load32(Offset.Flags);
}
