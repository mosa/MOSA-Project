/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// CHS (Cylinders-Heads-Sectors)
	/// </summary>
	public class CHS
	{
		/// <summary>
		/// Cylinder
		/// </summary>
		public ushort Cylinder = 0;

		/// <summary>
		/// Head
		/// </summary>
		public byte Head = 0;

		/// <summary>
		/// Sector
		/// </summary>
		public ushort Sector = 0;

		/// <summary>
		/// Initializes a new instance of the <see cref="CHS"/> struct.
		/// </summary>
		public CHS()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CHS"/> struct.
		/// </summary>
		/// <param name="diskGeometry">The disk geometry.</param>
		/// <param name="lba">The lba.</param>
		public CHS(DiskGeometry diskGeometry, ulong lba)
		{
			SetCHS(diskGeometry, lba);
		}

		/// <summary>
		/// Computes the CHS given a disk geometry
		/// </summary>
		/// <param name="diskGeometry">The disk geometry.</param>
		/// <param name="lba">The lba.</param>
		public void SetCHS(DiskGeometry diskGeometry, ulong lba)
		{
			if ((lba / (uint)(diskGeometry.SectorsPerTrack * diskGeometry.Heads) > 1023))
				lba = (uint)diskGeometry.Heads * diskGeometry.SectorsPerTrack * 1024 - 1;

			Sector = (ushort)(lba % diskGeometry.SectorsPerTrack + 1);
			lba /= diskGeometry.SectorsPerTrack;
			Head = (byte)(lba % diskGeometry.Heads);
			lba /= diskGeometry.Heads;
			Cylinder = (ushort)(lba & 0xFF);
			Sector |= (ushort)((lba >> 2) & 0xC0);
		}
	}
}