// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.Disks;

/// <summary>
/// Describes a disk's geometry (Cylinders-Heads-SectorsPerTrack). Note that this fulfills a different role than the
/// <see cref="CHS"/> class: whereas the CHS class sets the Cylinder, Head and Sector of a drive based on a DiskGeometry class and an
/// LBA, the DiskGeometry class itself tries to guess the Cylinders, Heads and SectorsPerTrack of a disk, based on an LBA alone.
/// </summary>
public struct DiskGeometry
{
	public ushort Cylinders;
	public byte Heads;
	public ushort SectorsPerTrack;

	public DiskGeometry(ushort cylinders, byte heads, ushort sectorsPerTrack)
	{
		Cylinders = cylinders;
		Heads = heads;
		SectorsPerTrack = sectorsPerTrack;
	}

	public void GuessGeometry(ulong lba)
	{
		uint cylinderTimesHeads;

		if (lba > 65535 * 16 * 255)
			lba = 65535 * 16 * 255;

		if (lba >= 65535 * 16 * 63)
		{
			SectorsPerTrack = 255;
			Heads = 16;
			cylinderTimesHeads = (uint)(lba / SectorsPerTrack);
		}
		else
		{
			SectorsPerTrack = 17;
			cylinderTimesHeads = (uint)(lba / SectorsPerTrack);

			Heads = (byte)((cylinderTimesHeads + 1023) / 1024);

			if (Heads < 4)
				Heads = 4;

			if (cylinderTimesHeads >= Heads * 1024 || Heads > 16)
			{
				SectorsPerTrack = 31;
				Heads = 16;
				cylinderTimesHeads = (uint)(lba / SectorsPerTrack);
			}

			if (cylinderTimesHeads >= Heads * 1024)
			{
				SectorsPerTrack = 63;
				Heads = 16;
				cylinderTimesHeads = (uint)(lba / SectorsPerTrack);
			}
		}

		Cylinders = (ushort)(cylinderTimesHeads / Heads);
	}
}
