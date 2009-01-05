/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

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
		public FatType FATType;

		/// <summary>
		/// 
		/// </summary>
		public byte[] SerialID;

		/// <summary>
		/// 
		/// </summary>
		public bool FloppyMedia;

		/// <summary>
		/// 
		/// </summary>
		protected byte[] osBootCode;

		/// <summary>
		/// 
		/// </summary>
		public byte[] OSBootCode
		{
			get
			{
				if (osBootCode == null) return null;
				byte[] clone = new byte[osBootCode.Length];
				osBootCode.CopyTo(clone, 0);
				return clone;
			}
			set
			{
				if (value == null) {
					osBootCode = null;
					return;
				}

				osBootCode = new byte[value.Length];
				value.CopyTo(osBootCode, 0);				
			}
		}

		/// <summary>
		/// Sectors Per Track
		/// </summary>
		public ushort SectorsPerTrack;
		/// <summary>
		/// Number of Heads
		/// </summary>
		public ushort NumberOfHeads;
		/// <summary>
		/// Hidden Sectors
		/// </summary>
		public ushort HiddenSectors;

		/// <summary>
		/// Initializes a new instance of the <see cref="FatSettings"/> class.
		/// </summary>
		public FatSettings()
		{
			this.FATType = FatType.FAT16;	// default
			this.SerialID = new byte[0];
			this.FloppyMedia = false;
			this.OSBootCode = null;
		}
	}
}