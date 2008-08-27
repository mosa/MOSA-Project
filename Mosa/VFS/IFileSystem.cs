/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.VFS
{
	public interface IFileSystem
	{

		bool IsReadOnly { get; }

		IVfsNode Root { get; }
	}
}
