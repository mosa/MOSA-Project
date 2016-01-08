// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.ClassLib;

namespace Mosa.FileSystem.FAT.Find
{
	/// <summary>
	///
	/// </summary>
	public class ByCluster : FatFileSystem.ICompare
	{
		/// <summary>
		///
		/// </summary>
		protected uint cluster;

		/// <summary>
		/// Initializes a new instance of the <see cref="ByCluster"/> class.
		/// </summary>
		/// <param name="cluster">The cluster.</param>
		public ByCluster(uint cluster)
		{
			this.cluster = cluster;
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
			var entry = new BinaryFormat(data);

			byte first = entry.GetByte(offset + Entry.DOSName);

			if (first == FileNameAttribute.LastEntry)
				return false;

			if ((first == FileNameAttribute.Deleted) | (first == FileNameAttribute.Dot))
				return false;

			if (first == FileNameAttribute.Escape)
				return false;

			uint startcluster = FatFileSystem.GetClusterEntry(data, offset, type);

			if (startcluster == cluster)
				return true;

			return false;
		}
	}
}
