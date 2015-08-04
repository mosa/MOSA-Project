// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;

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
