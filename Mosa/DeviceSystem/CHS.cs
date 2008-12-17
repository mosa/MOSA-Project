/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.ClassLib;

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
		/// Computes the CHS (Cylinders-Heads-Sectors)
		/// </summary>
		/// <param name="lba">The lba.</param>
		public void ComputeCHSv1(ulong lba)
		{
			ushort cylinderTimesHeads;

			if (lba > 65535 * 16 * 255)
				lba = 65535 * 16 * 255;

			if (lba >= 65535 * 16 * 63) {
				Sector = 255;
				Head = 16;
				cylinderTimesHeads = (ushort)(lba / Sector);
			}
			else {
				Sector = 17;
				cylinderTimesHeads = (ushort)(lba / Sector);

				Head = (byte)((cylinderTimesHeads + 1023) / 1024);

				if (Head < 4)
					Head = 4;

				if (cylinderTimesHeads >= (Head * 1024) || Head > 16) {
					Sector = 31;
					Head = 16;
					cylinderTimesHeads = (ushort)(lba / Sector);
				}

				if (cylinderTimesHeads >= (Head * 1024)) {
					Sector = 63;
					Head = 16;
					cylinderTimesHeads = (ushort)(lba / Sector);
				}
			}

			Cylinder = (ushort)(cylinderTimesHeads / Head);
		}

		/// <summary>
		/// Computes the CHS v2.
		/// </summary>
		/// <param name="lba">The lba.</param>
		public void ComputeCHSv2(ulong lba)
		{
			uint SectorsPerTrack = 17; // 63
			uint NumHeads = 4;

			if ((lba / (SectorsPerTrack * NumHeads) > 1023))
				lba = NumHeads * SectorsPerTrack * 1024 - 1;

			Sector = (ushort)(lba % SectorsPerTrack + 1);
			lba /= SectorsPerTrack;
			Head = (byte)(lba % NumHeads);
			lba /= NumHeads;
			Cylinder = (ushort)(lba & 0xFF);
			Sector |= (ushort)((lba >> 2) & 0xC0);
		}

		/// <summary>
		/// Computes the CHS given a disk geometry
		/// </summary>
		/// <param name="lba">The lba.</param>
		/// <param name="diskGeometry">The disk geometry.</param>
		public void ComputeCHS(ulong lba, DiskGeometry diskGeometry)
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
