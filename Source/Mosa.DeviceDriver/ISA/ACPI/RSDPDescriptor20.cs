// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceDriver.ISA.ACPI;

public readonly struct RSDPDescriptor20
{
	public readonly Pointer Pointer;

	public const uint Size = Offset.Size;

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

	public byte Checksum => Pointer.Load8(Offset.Checksum);

	public byte Revision => Pointer.Load8(Offset.Revision);

	public Pointer RsdtAddress => new Pointer(Pointer.Load32(Offset.RsdtAddress));

	public uint Length => Pointer.Load32(Offset.Length);

	public Pointer XsdtAddress => new Pointer(Pointer.Load64(Offset.XsdtAddress));

	public byte ExtendedChecksum => Pointer.Load8(Offset.ExtendedChecksum);
}
