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
		/// Cylinders
		/// </summary>
		public ushort Cylinders = 0;
		/// <summary>
		/// Heads
		/// </summary>
		public byte Heads = 0;
		/// <summary>
		/// Sectors
		/// </summary>
		public ushort Sectors = 0;

		/// <summary>
		/// Initializes a new instance of the <see cref="CHS"/> struct.
		/// </summary>
		/// <param name="totalSectors">The total sectors.</param>
		public CHS(ulong totalSectors)
		{
			ComputeCHS(totalSectors);
		}

		/// <summary>
		/// Computes the CHS (Cylinders-Heads-Sectors)
		/// </summary>
		/// <param name="totalSectors">The total sectors.</param>
		/// <returns></returns>
		public void ComputeCHS(ulong totalSectors)
		{
			ushort cylinderTimesHeads;

			if (totalSectors > 65535 * 16 * 255)
				totalSectors = 65535 * 16 * 255;

			if (totalSectors >= 65535 * 16 * 63) {
				Sectors = 255;
				Heads = 16;
				cylinderTimesHeads = (ushort)(totalSectors / Sectors);
			}
			else {
				Sectors = 17;
				cylinderTimesHeads = (ushort)(totalSectors / Sectors);

				Heads = (byte)((cylinderTimesHeads + 1023) / 1024);

				if (Heads < 4)
					Heads = 4;

				if (cylinderTimesHeads >= (Heads * 1024) || Heads > 16) {
					Sectors = 31;
					Heads = 16;
					cylinderTimesHeads = (ushort)(totalSectors / Sectors);
				}

				if (cylinderTimesHeads >= (Heads * 1024)) {
					Sectors = 63;
					Heads = 16;
					cylinderTimesHeads = (ushort)(totalSectors / Sectors);
				}
			}

			Cylinders = (ushort)(cylinderTimesHeads / Heads);
		}
	}
}
