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
	public class FATSettings : GenericFileSystemSettings
	{
		/// <summary>
		/// 
		/// </summary>
		public FATType FATType;

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

				byte[] copy = new byte[osBootCode.Length];

				for (int i = 0; i < osBootCode.Length; i++)
					copy[i] = osBootCode[i];

				return copy;
			}
			set
			{
				if (value == null) {
					osBootCode = null;
					return;
				}

				osBootCode = new byte[value.Length];

				for (int i = 0; i < value.Length; i++)
					osBootCode[i] = value[i];
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
		/// Initializes a new instance of the <see cref="FATSettings"/> class.
		/// </summary>
		public FATSettings()
		{
			this.FATType = FATType.FAT16;	// default
			this.SerialID = new byte[0];
			this.FloppyMedia = false;
			this.OSBootCode = null;
		}
	}
}