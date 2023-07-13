// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceDriver.ISA.ACPI;

public struct ACPISDTHeader
{
	private readonly Pointer Entry;

	public readonly Pointer Pointer => Entry;

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

	public ACPISDTHeader(Pointer entry) => Entry = entry;

	public readonly uint Signature => Entry.Load32(Offset.Signature);

	public readonly uint Length => Entry.Load32(Offset.Length);

	public readonly byte Revision => Entry.Load8(Offset.Revision);

	public readonly byte Checksum => Entry.Load8(Offset.Checksum);

	public byte GetOEMID(int index) => Entry.Load8(Offset.OEMID + index);

	public byte GetOEMTableID(int index) => Entry.Load8(Offset.OEMTableID + index);

	public readonly uint OEMRevision => Entry.Load32(Offset.OEMRevision);

	public readonly uint CreatorID => Entry.Load32(Offset.CreatorID);

	public readonly uint CreatorRevision => Entry.Load32(Offset.CreatorRevision);
}
