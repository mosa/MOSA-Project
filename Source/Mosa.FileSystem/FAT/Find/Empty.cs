// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;
using Mosa.FileSystem.FAT;

namespace Mosa.FileSystem.Find
{
	/// <summary>
	/// Empty
	/// </summary>
	internal class Empty : FatFileSystem.ICompare
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

			byte first = entry.GetByte(offset + Entry.DOSName);

			if (first == FileNameAttribute.LastEntry)
				return true;

			if ((first == FileNameAttribute.Deleted) | (first == FileNameAttribute.Dot))
				return true;

			return false;
		}
	}
}
