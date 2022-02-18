/*// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;

namespace Mosa.FileSystem.FAT
{
	/// <summary>
	/// Fat File System Device
	/// </summary>
	public class FatFileSystemDevice : BaseDeviceDriver, IFileSystemDevice
	{
		// TODO: Incomplete

		public override void Initialize()
		{
			Device.Name = Device.Parent.Name + "/FS/Fat"; // need to give it a unique name
		}

		public override void Probe() => Device.Status = DeviceStatus.Available;

		public override void Start()
		{
			Device.Status = DeviceStatus.Online;
		}

		public override bool OnInterrupt() => true;
	}
}
*/
