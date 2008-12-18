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
	/// Disk Geometry (Heads, Cylinders, SectorsPerTrack)
	/// </summary>
	public struct DiskGeometry
	{
		/// <summary>
		/// Cylinder
		/// </summary>
		public ushort Cylinders;
		/// <summary>
		/// Head
		/// </summary>
		public byte Heads;
		/// <summary>
		/// Sector
		/// </summary>
		public ushort SectorsPerTrack;

		/// <summary>
		/// Initializes a new instance of the <see cref="DiskGeometry"/> struct.
		/// </summary>
		/// <param name="cylinders">The cylinders.</param>
		/// <param name="heads">The heads.</param>
		/// <param name="sectorsPerTrack">The sectors per track.</param>
		public DiskGeometry(ushort cylinders, byte heads, ushort sectorsPerTrack)
		{
			this.Cylinders = cylinders;
			this.Heads = heads;
			this.SectorsPerTrack = sectorsPerTrack;
		}

		/// <summary>
		/// Guesses the geometry.
		/// </summary>
		/// <param name="lba">The lba.</param>
		public void GuessGeometry(ulong lba)
		{
			uint cylinderTimesHeads;

			if (lba > 65535 * 16 * 255)
				lba = 65535 * 16 * 255;

			if (lba >= 65535 * 16 * 63) {
				SectorsPerTrack = 255;
				Heads = 16;
				cylinderTimesHeads = (uint)(lba / SectorsPerTrack);
			}
			else {
				SectorsPerTrack = 17;
				cylinderTimesHeads = (uint)(lba / SectorsPerTrack);

				Heads = (byte)((cylinderTimesHeads + 1023) / 1024);

				if (Heads < 4)
					Heads = 4;

				if (cylinderTimesHeads >= (Heads * 1024) || Heads > 16) {
					SectorsPerTrack = 31;
					Heads = 16;
					cylinderTimesHeads = (uint)(lba / SectorsPerTrack);
				}

				if (cylinderTimesHeads >= (Heads * 1024)) {
					SectorsPerTrack = 63;
					Heads = 16;
					cylinderTimesHeads = (uint)(lba / SectorsPerTrack);
				}
			}

			Cylinders = (ushort)(cylinderTimesHeads / Heads);
		}


	}
}
