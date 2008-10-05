/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.FileSystem.VFS
{
	/// <summary>
	/// Specifies the type of the node in the virtual file system.
	/// </summary>
	public enum VfsNodeType
	{

		/// <summary>
		/// An unknown node type.
		/// </summary>
		Unknown,

		/// <summary>
		/// Represents a classic file (represented by a System.IO.Stream or derived class.
		/// </summary>
		File,

		/// <summary>
		/// Represents a folder (aka directory) in the file system.
		/// </summary>
		Directory,

		/// <summary>
		/// A symbolic link in the (virtual) filesystem.
		/// </summary>
		SymbolicLink,

		/// <summary>
		/// Represents a basic device node.
		/// </summary>
		Device
	}
}
