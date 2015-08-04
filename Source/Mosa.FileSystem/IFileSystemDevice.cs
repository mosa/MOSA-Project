// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
