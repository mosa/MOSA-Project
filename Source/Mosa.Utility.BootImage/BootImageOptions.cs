// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using System;
using System.Collections.Generic;

namespace Mosa.Utility.BootImage
{
	/// <summary>
	///
	/// </summary>
	public class BootImageOptions
	{
		public Guid MediaGuid = Guid.NewGuid();
		public Guid MediaLastSnapGuid = Guid.NewGuid();
		public byte[] MBRCode = null;
		public byte[] FatBootCode = null;
		public string VolumeLabel = string.Empty;
		public ImageFormat ImageFormat = ImageFormat.IMG;
		public bool MBROption = true;
		public bool PatchSyslinuxOption = false;
		public uint BlockCount = 0;
		public FileSystem FileSystem = FileSystem.FAT12;
		public List<IncludeFile> IncludeFiles = new List<IncludeFile>();
		public BootLoader BootLoader = BootLoader.Syslinux_3_72;

		public string DiskImageFileName = null;

		public BootImageOptions()
		{
		}
	}
}
