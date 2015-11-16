// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.FileSystem.FAT
{
	/// <summary>
	///
	/// </summary>
	public class FatSettings : GenericFileSystemSettings
	{
		/// <summary>
		///
		/// </summary>
		public FatType FATType { get; set; }

		/// <summary>
		///
		/// </summary>
		public byte[] SerialID { get; set; }

		/// <summary>
		///
		/// </summary>
		public bool FloppyMedia { get; set; }

		/// <summary>
		///
		/// </summary>
		public byte[] OSBootCode { get; set; }

		/// <summary>
		/// Sectors Per Track
		/// </summary>
		public ushort SectorsPerTrack { get; set; }

		/// <summary>
		/// Number of Heads
		/// </summary>
		public ushort NumberOfHeads { get; set; }

		/// <summary>
		/// Hidden Sectors
		/// </summary>
		public ushort HiddenSectors { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="FatSettings"/> class.
		/// </summary>
		public FatSettings()
		{
			this.FATType = FatType.FAT16;   // default
			this.SerialID = new byte[0];
			this.FloppyMedia = false;
			this.OSBootCode = null;
		}
	}
}
