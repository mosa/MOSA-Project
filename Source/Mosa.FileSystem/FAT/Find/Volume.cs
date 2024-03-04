// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.Misc;

namespace Mosa.FileSystem.FAT.Find;

/// <summary>
/// Volume
/// </summary>
internal class Volume : FatFileSystem.ICompare
{
	/// <summary>
	/// Compares the specified data.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <param name="offset">The offset.</param>
	/// <param name="type">The type.</param>
	/// <returns></returns>
	public bool Compare(byte[] data, uint offset, FatType type)
	{
		var entry = new DataBlock(data);

		byte first = entry.GetByte(Entry.DOSName + offset);

		if (first == FileNameAttribute.LastEntry)
			return false;

		if ((first == FileNameAttribute.Deleted) | (first == FileNameAttribute.Dot))
			return false;

		if (first == FileNameAttribute.Escape)
			return false;

		FatFileAttributes attribute = (FatFileAttributes)entry.GetByte(Entry.FileAttributes + offset);

		if ((attribute & FatFileAttributes.VolumeLabel) == FatFileAttributes.VolumeLabel)
			return true;

		return false;
	}
}
