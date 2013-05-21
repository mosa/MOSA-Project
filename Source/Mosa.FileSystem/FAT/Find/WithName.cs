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
	public class WithName : FatFileSystem.ICompare
	{
		/// <summary>
		///
		/// </summary>
		protected string name;

		/// <summary>
		/// Initializes a new instance of the <see cref="WithName"/> class.
		/// </summary>
		/// <param name="name">The name.</param>
		public WithName(string name)
		{
			this.name = name;
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

			byte first = entry.GetByte(offset + Entry.DOSName);

			if (first == FileNameAttribute.LastEntry)
				return false;

			if ((first == FileNameAttribute.Deleted) | (first == FileNameAttribute.Dot))
				return false;

			if (first == FileNameAttribute.Escape)
				return false;

			string entryname = FatFileSystem.ExtractFileName(data, offset);

			if (entryname == name)
				return true;

			return false;
		}
	}
}