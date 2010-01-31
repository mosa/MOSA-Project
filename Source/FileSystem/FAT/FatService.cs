/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceSystem;
using Mosa.FileSystem;
using Mosa.FileSystem.VFS;

namespace Mosa.FileSystem.FAT
{
    /// <summary>
    /// 
    /// </summary>
	public class FatService : Device, IDevice, IFileSystemDevice
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FatService"/> class.
		/// </summary>
		public FatService()
		{
			base.name = "FAT";
			base.parent = null;
			base.deviceStatus = DeviceStatus.Available;
		}

		/// <summary>
		/// Creates the specified file system.
		/// </summary>
		/// <param name="partition">The partition.</param>
		/// <returns></returns>
		public GenericFileSystem Create(IPartitionDevice partition)
		{
			return new FatFileSystem(partition);
		}

	}
}
