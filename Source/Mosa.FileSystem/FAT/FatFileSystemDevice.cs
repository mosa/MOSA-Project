// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;

namespace Mosa.FileSystem.FAT
{
	/// <summary>
	/// Fat File System Device
	/// </summary>
	public class FatFileSystemDevice : Device, IFileSystemDevice
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FatFileSystemDevice"/> class.
		/// </summary>
		private FatFileSystemDevice(IPartitionDevice partition)
		{
			base.Parent = partition as Device;
			base.Name = base.Parent.Name + "/FS/Fat"; // need to give it a unique name
			base.Status = DeviceStatus.Online;
		}
	}
}
