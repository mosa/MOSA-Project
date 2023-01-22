// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using Mosa.Compiler.Common;

namespace Mosa.Utility.BootImage
{
	public class BootImageOptions
	{
		public Guid MediaGuid = Guid.NewGuid();
		public Guid MediaLastSnapGuid = Guid.NewGuid();
		public byte[] MBRCode = null;
		public byte[] FatBootCode = null;
		public string VolumeLabel = string.Empty;
		public ImageFormat ImageFormat = ImageFormat.IMG;
		public bool MBROption = true;
		public uint BlockCount = 0;
		public FileSystem FileSystem = FileSystem.FAT12;
		public readonly List<IncludeFile> IncludeFiles = new List<IncludeFile>();
		public BootLoader BootLoader = BootLoader.Limine;

		public string DiskImageFileName = null;
	}
}
