// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

// Portions of this code are from Cosmos

//https://wiki.osdev.org/ACPI
//https://wiki.osdev.org/MADT

namespace Mosa.DeviceDriver.ISA.ACPI;

public struct RSDPDescriptor20
{
	public readonly Pointer Pointer;

	public readonly uint Size = Offset.Size;

	private static class Offset
	{
		public const int Signature = 0;
		public const int Checksum = Signature + 8;
		public const int OEMID = Checksum + 1;
		public const int Revision = OEMID + 6;
		public const int RsdtAddress = Revision + 1;

		public const int Length = RsdtAddress + 4;
		public const int XsdtAddress = Length + 4;
		public const int ExtendedChecksum = XsdtAddress + 8;
		public const int Reserved = ExtendedChecksum + 1;
		public const int Size = Reserved + 3;
	}

	public RSDPDescriptor20(Pointer entry) => Pointer = entry;

	public readonly byte Checksum => Pointer.Load8(Offset.Checksum);

	public readonly byte Revision => Pointer.Load8(Offset.Revision);

	public readonly Pointer RsdtAddress => new Pointer(Pointer.Load32(Offset.RsdtAddress));

	public readonly uint Length => Pointer.Load32(Offset.Length);

	public readonly ulong XsdtAddress => Pointer.Load32(Offset.XsdtAddress);

	public readonly byte ExtendedChecksum => Pointer.Load8(Offset.ExtendedChecksum);
}
