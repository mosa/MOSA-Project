// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.FileSystem.VFS
{
	/// <summary>
	///
	/// </summary>
	public interface IFileSystem
	{
		/// <summary>
		///
		/// </summary>
		bool IsReadOnly { get; }

		/// <summary>
		///
		/// </summary>
		IVfsNode Root { get; }
	}
}
