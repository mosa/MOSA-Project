// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using Mosa.Compiler.Common;

namespace Mosa.Utility.BootImage;

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
	public uint BlockCount = 0;
	public FileSystem FileSystem = FileSystem.FAT12;
	public List<IncludeFile> IncludeFiles = new List<IncludeFile>();

	public string DiskImageFileName = null;

	public BootImageOptions()
	{
	}
}
