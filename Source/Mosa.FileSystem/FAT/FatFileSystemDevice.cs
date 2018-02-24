// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;

namespace Mosa.FileSystem.FAT
{
	/// <summary>
	/// Fat File System Device
	/// </summary>
	public class FatFileSystemDevice : BaseDeviceDriver, IFileSystemDevice
	{
		// TODO: Incomplete

		protected override void Initialize()
		{
			Device.Name = Device.Parent.Name + "/FS/Fat"; // need to give it a unique name
			Device.Status = DeviceStatus.Available;
		}

		public override void Probe() => Device.Status = DeviceStatus.Available;

		public override void Start()
		{
			if (Device.Status != DeviceStatus.Available)
				return;

			Device.Status = DeviceStatus.Online;
		}

		public override bool OnInterrupt() => true;
	}
}
