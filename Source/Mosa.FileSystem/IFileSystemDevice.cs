/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceSystem;

namespace Mosa.FileSystem
{
	/// <summary>
	/// 
	/// </summary>
	public interface IFileSystemDevice
	{
		/// <summary>
		/// Creates the specified partition.
		/// </summary>
		/// <param name="partition">The partition.</param>
		/// <returns></returns>
		GenericFileSystem Create(IPartitionDevice partition);
	}
}
