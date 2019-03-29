// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	public class DiskDeviceService : BaseService
	{
		public override void PostEvent(ServiceEvent serviceEvent)
		{
			var device = MatchEvent<IDiskControllerDevice>(serviceEvent, ServiceEventType.Start);

			if (device == null)
				return;

			// This mounts everything detected

			var controller = device.DeviceDriver as IDiskControllerDevice;

			var deviceService = device.DeviceService;

			for (uint drive = 0; drive < controller.MaximunDriveCount; drive++)
			{
				if (!controller.Open(drive))
					continue;

				if (controller.GetTotalSectors(drive) == 0)
					continue;

				// don't mount twice
				if (deviceService.CheckExists(device, drive))
					return;

				var configuration = new DiskDeviceConfiguration()
				{
					DriveNbr = drive,
					ReadOnly = false
				};

				deviceService.Initialize(new DiskDeviceDriver(), device, configuration);
			}
		}
	}
}
