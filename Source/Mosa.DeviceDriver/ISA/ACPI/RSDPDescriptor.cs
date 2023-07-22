// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceDriver.ISA.ACPI;

public struct RSDPDescriptor
{
	public readonly Pointer Pointer;

	public readonly uint Size = Offset.Size;

	internal static class Offset
	{
		public const int Signature = 0;
		public const int Checksum = Signature + 8;
		public const int OEMID = Checksum + 1;
		public const int Revision = OEMID + 6;
		public const int RsdtAddress = Revision + 1;
		public const int Size = RsdtAddress + 8;
	}

	public RSDPDescriptor(Pointer entry) => Pointer = entry;

	public readonly ulong Signature => Pointer.Load64(Offset.Signature);

	public readonly byte Checksum => Pointer.Load8(Offset.Checksum);

	public readonly byte Revision => Pointer.Load8(Offset.Revision);

	public readonly Pointer RsdtAddress => new Pointer(Pointer.Load32(Offset.RsdtAddress));

	public byte GetSignature(int index) => Pointer.Load8(Offset.Signature + index);
}
