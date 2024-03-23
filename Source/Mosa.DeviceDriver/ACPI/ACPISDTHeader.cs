// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceDriver.ACPI;

/// <summary>
/// A standard ACPI header, which is the base of all other headers (like the <see cref="FADT"/> for example).
/// </summary>
public readonly struct ACPISDTHeader
{
	public readonly Pointer Pointer;

	public const uint Size = Offset.Size;

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

	public uint Signature => Pointer.Load32(Offset.Signature);

	public uint Length => Pointer.Load32(Offset.Length);

	public byte Revision => Pointer.Load8(Offset.Revision);

	public byte Checksum => Pointer.Load8(Offset.Checksum);

	public byte GetOEMID(int index) => Pointer.Load8(Offset.OEMID + index);

	public byte GetOEMTableID(int index) => Pointer.Load8(Offset.OEMTableID + index);

	public uint OEMRevision => Pointer.Load32(Offset.OEMRevision);

	public uint CreatorID => Pointer.Load32(Offset.CreatorID);

	public uint CreatorRevision => Pointer.Load32(Offset.CreatorRevision);
}
