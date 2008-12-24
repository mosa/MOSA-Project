/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.ClassLib;

namespace Mosa.FileSystem.FAT.Find
{
	/// <summary>
	/// 
	/// </summary>
	public class ByCluster : FATFileSystem.ICompare
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
		public bool Compare(byte[] data, uint offset, FATType type)
		{
			BinaryFormat entry = new BinaryFormat(data);

			byte first = entry.GetByte(offset + Entry.DOSName);

			if (first == FileNameAttribute.LastEntry)
				return false;

			if ((first == FileNameAttribute.Deleted) | (first == FileNameAttribute.Dot))
				return false;

			if (first == FileNameAttribute.Escape)
				return false;

			uint startcluster = FATFileSystem.GetClusterEntry(data, offset, type);

			if (startcluster == cluster)
				return true;

			return false;
		}
	}
}
