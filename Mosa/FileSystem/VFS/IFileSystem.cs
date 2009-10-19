/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

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
