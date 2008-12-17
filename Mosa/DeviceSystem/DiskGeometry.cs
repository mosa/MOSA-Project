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

	}
}
