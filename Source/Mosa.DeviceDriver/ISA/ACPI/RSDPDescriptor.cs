// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceDriver.ISA.ACPI;

public struct RSDPDescriptor
{
	private readonly Pointer Entry;

	public readonly Pointer Pointer => Entry;
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

	public RSDPDescriptor(Pointer entry) => Entry = entry;

	public readonly ulong Signature => Entry.Load64(Offset.Signature);

	public readonly byte Checksum => Entry.Load8(Offset.Checksum);

	public readonly byte Revision => Entry.Load8(Offset.Revision);

	public readonly Pointer RsdtAddress => new Pointer(Entry.Load32(Offset.RsdtAddress));

	public byte GetSignature(int index) => Entry.Load8(Offset.Signature + index);
}
