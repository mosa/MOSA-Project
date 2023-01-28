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
	public readonly byte[] MBRCode = null;
	public readonly byte[] FatBootCode = null;
	public string VolumeLabel = string.Empty;
	public ImageFormat ImageFormat = ImageFormat.IMG;
	public readonly bool MBROption = true;
	public readonly uint BlockCount = 0;
	public FileSystem FileSystem = FileSystem.FAT12;
	public readonly List<IncludeFile> IncludeFiles = new List<IncludeFile>();

	public string DiskImageFileName = null;

	public BootImageOptions()
	{
	}
}
