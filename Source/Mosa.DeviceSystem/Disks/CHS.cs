// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.Disks;

/// <summary>
/// Describes a CHS (Cylinders-Heads-Sectors) structure.
/// </summary>
public class CHS
{
	public ushort Cylinder;
	public byte Head;
	public ushort Sector;

	public CHS(ushort cylinder, byte head, ushort sector)
	{
		Cylinder = cylinder;
		Head = head;
		Sector = sector;
	}

	public CHS(DiskGeometry diskGeometry, ulong lba) => SetCHS(diskGeometry, lba);

	public void SetCHS(DiskGeometry diskGeometry, ulong lba)
	{
		if (lba / (uint)(diskGeometry.SectorsPerTrack * diskGeometry.Heads) > 1023)
			lba = (uint)diskGeometry.Heads * diskGeometry.SectorsPerTrack * 1024 - 1;

		Sector = (ushort)(lba % diskGeometry.SectorsPerTrack + 1);
		lba /= diskGeometry.SectorsPerTrack;

		Head = (byte)(lba % diskGeometry.Heads);
		lba /= diskGeometry.Heads;

		Cylinder = (ushort)(lba & 0xFF);
		Sector |= (ushort)((lba >> 2) & 0xC0);
	}
}
