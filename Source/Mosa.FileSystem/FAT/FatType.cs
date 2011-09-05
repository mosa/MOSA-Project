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
	/// Enumeration of FAT types.
	/// </summary>
	public enum FatType : byte
	{
		/// <summary>
		/// Represents a 12-bit FAT.
		/// </summary>
		FAT12 = 12,
		/// <summary>
		/// Represents a 16-bit FAT.
		/// </summary>
		FAT16 = 16,
		/// <summary>
		/// Represents a 32-bit FAT.
		/// </summary>
		FAT32 = 32
	}

}
