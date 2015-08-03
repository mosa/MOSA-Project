// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.ClassLib;

namespace Mosa.FileSystem.FAT.Find
{
	/// <summary>
	///
	/// </summary>
	public class Volume : FatFileSystem.ICompare
	{
		/// <summary>
		///
		/// </summary>
		protected uint cluster;

		/// <summary>
		///
		/// </summary>
		public Volume()
		{
		}

		/// <summary>
		/// Compares the specified data.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public bool Compare(byte[] data, uint offset, FatType type)
		{
			BinaryFormat entry = new BinaryFormat(data);

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
}