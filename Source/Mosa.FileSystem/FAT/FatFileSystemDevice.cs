// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;

namespace Mosa.FileSystem.FAT
{
	/// <summary>
	///
	/// </summary>
	public class FatFileSystemDevice : Device, IDevice, IFileSystemDevice
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FatFileSystemDevice"/> class.
		/// </summary>
		private FatFileSystemDevice(IPartitionDevice partition)
		{
			base.parent = partition as Device;
			base.name = base.parent.Name + "/FS/Fat"; // need to give it a unique name
			base.deviceStatus = DeviceStatus.Online;
		}
	}
}
