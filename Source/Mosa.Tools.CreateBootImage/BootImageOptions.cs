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
using System.Linq;
using System.Text;

namespace Mosa.Tools.BootImage
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
	class BootImageOptions
	{
		public Guid MediaGuid = Guid.NewGuid();
		public Guid MediaLastSnapGuid = Guid.NewGuid();
		public string MBRFileName = string.Empty;
		public string FatCodeFileName = string.Empty;
		public string VolumeLabel = string.Empty;
		public ImageFormatType ImageFormat = ImageFormatType.VHD;
		public bool MBROption = true;
		public bool PatchSyslinuxOption = false;
		public bool FloppyMedia = false;
		public uint BlockCount = 1024 * 1024 / 512;
		public FileSystemType FileSystem = FileSystemType.FAT12;
		public List<IncludeFile> IncludeFiles = new List<IncludeFile>();

		public string DiskImageFileName = null;

		public BootImageOptions()
		{
		}


	}
}
