// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;

namespace Mosa.DeviceSystem.Services;

/// <summary>
/// Initializes all disks, using a custom disk device driver.
/// </summary>
public class DiskDeviceService : BaseService
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void PostEvent(ServiceEvent serviceEvent)
	{
		//HAL.DebugWriteLine("DiskDeviceService:PostEvent()");

		var device = MatchEvent<IDiskControllerDevice>(serviceEvent, ServiceEventType.Start);

		// This mounts everything detected

		if (device?.DeviceDriver is not IDiskControllerDevice controller)
			return;

		var deviceService = device.DeviceService;

		for (var drive = 0U; drive < controller.MaximumDriveCount; drive++)
		{
			if (!controller.Open(drive))
				continue;

			if (controller.GetTotalSectors(drive) == 0)
				continue;

			if (deviceService.CheckExists(device, drive)) // Don't mount twice
				return;

			var configuration = new DiskDeviceConfiguration
			{
				DriveNbr = drive,
				ReadOnly = false
			};

			deviceService.Initialize(new DiskDeviceDriver(), device, true, configuration);
		}
	}
}
