// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	public class DiskDeviceService : BaseService
	{
		public override void PostEvent(ServiceEvent serviceEvent)
		{
			if (serviceEvent.ServiceEventType != ServiceEventType.Start)
				return;

			var device = serviceEvent.Subject as Device;

			if (device == null)
				return;

			// this mounts everything
			var controller = device.DeviceDriver as IDiskControllerDevice;

			if (controller == null)
				return;

			var deviceServe = device.DeviceService;

			for (uint drive = 0; drive < controller.MaximunDriveCount; drive++)
			{
				if (!controller.Open(drive))
					continue;

				if (controller.GetTotalSectors(drive) == 0)
					continue;

				// don't mount twice
				if (deviceServe.CheckExists(device, drive))
					return;

				var configuration = new DiskDeviceConfiguration()
				{
					DriveNbr = drive,
					ReadOnly = false
				};

				deviceServe.Initialize(new DiskDeviceDriver(), device, configuration);

				//HAL.DebugWriteLine("DiskDeviceService::OnChange():Exit");
			}
		}
	}
}
