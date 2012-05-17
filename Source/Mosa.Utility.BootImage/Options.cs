/*
* (c) 2012 MOSA - The Managed Operating System Alliance
*
* Licensed under the terms of the New BSD License.
*
* Authors:
*  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using System;
using System.Collections.Generic;

namespace Mosa.Utility.BootImage
{
	/// <summary>
	/// 
	/// </summary>
	public enum FileSystemType { FAT12, FAT16, FAT32 };
	/// <summary>
	/// 
	/// </summary>
	public enum ImageFormatType { IMG, VHD, VDI };

	/// <summary>
	/// 
	/// </summary>
	public class Options
	{
		public Guid MediaGuid = Guid.NewGuid();
		public Guid MediaLastSnapGuid = Guid.NewGuid();
		public byte[] MBRCode = null;
		public byte[] FatBootCode = null;
		public string VolumeLabel = string.Empty;
		public ImageFormatType ImageFormat = ImageFormatType.VHD;
		public bool MBROption = true;
		public bool PatchSyslinuxOption = false;
		public uint BlockCount = 0;
		public FileSystemType FileSystem = FileSystemType.FAT12;
		public List<IncludeFile> IncludeFiles = new List<IncludeFile>();

		public string DiskImageFileName = null;

		public Options()
		{
		}


	}
}
