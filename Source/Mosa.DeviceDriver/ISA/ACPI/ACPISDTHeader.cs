// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceDriver.ISA.ACPI;

public struct ACPISDTHeader
{
	public readonly Pointer Pointer;

	public readonly uint Size = Offset.Size;

	internal static class Offset
	{
		public const int Signature = 0;
		public const int Length = Signature + 4;
		public const int Revision = Length + 1;
		public const int Checksum = Revision + 1;
		public const int OEMID = Checksum + 1;
		public const int OEMTableID = OEMID + 1;
		public const int OEMRevision = OEMTableID + 1;
		public const int CreatorID = OEMRevision + 1;
		public const int CreatorRevision = CreatorID + 1;
		public const int Size = CreatorRevision + 4;
	}

	public ACPISDTHeader(Pointer entry) => Pointer = entry;

	public readonly uint Signature => Pointer.Load32(Offset.Signature);

	public readonly uint Length => Pointer.Load32(Offset.Length);

	public readonly byte Revision => Pointer.Load8(Offset.Revision);

	public readonly byte Checksum => Pointer.Load8(Offset.Checksum);

	public byte GetOEMID(int index) => Pointer.Load8(Offset.OEMID + index);

	public byte GetOEMTableID(int index) => Pointer.Load8(Offset.OEMTableID + index);

	public readonly uint OEMRevision => Pointer.Load32(Offset.OEMRevision);

	public readonly uint CreatorID => Pointer.Load32(Offset.CreatorID);

	public readonly uint CreatorRevision => Pointer.Load32(Offset.CreatorRevision);
}
