// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	public class DiskDeviceMountDeamon : BaseMountDaemon
	{
		public override void OnChange(Device device)
		{
			var controller = device.DeviceDriver as IDiskControllerDevice;

			if (controller == null)
				return;

			if (device == null)
				return; // BAD

			var devicemanager = device.DeviceManager;

			if (devicemanager == null)
				return; // BAD

			for (uint drive = 0; drive < controller.MaximunDriveCount; drive++)
			{
				if (!controller.Open(drive))
					continue;

				if (controller.GetTotalSectors(drive) == 0)
					continue;

				if (devicemanager.CheckExists(device, drive))
					return;

				var configuration = new DiskDeviceConfiguration()
				{
					DriveNbr = drive,
					ReadOnly = false
				};

				devicemanager.Initialize(new DiskDeviceDriver(), device, configuration);

				//HAL.DebugWriteLine("DiskDeviceMountDeamon::OnChange():Exit");
			}
		}
	}
}
