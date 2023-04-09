// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using Mosa.Compiler.Common;

namespace Mosa.Utility.BootImage;

public class BootImageOptions
{
	public Guid MediaGuid = Guid.NewGuid();
	public Guid MediaLastSnapGuid = Guid.NewGuid();
	public string VolumeLabel = string.Empty;
	public ImageFirmware ImageFirmware = ImageFirmware.Bios;
	public ImageFormat ImageFormat = ImageFormat.IMG;
	public uint BlockCount = 0;
	public FileSystem FileSystem = FileSystem.FAT12;
	public List<IncludeFile> IncludeFiles = new List<IncludeFile>();
	public string DiskImageFileName = null;
}
