// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Disk Controller Manager
	/// </summary>
	public class DiskControllerManager
	{
		/// <summary>
		/// The device manager
		/// </summary>
		protected DeviceManager deviceManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="DiskControllerManager"/> class.
		/// </summary>
		/// <param name="deviceManager">The device manager.</param>
		public DiskControllerManager(DeviceManager deviceManager)
		{
			this.deviceManager = deviceManager;
		}

		/// <summary>
		/// Creates the devices.
		/// </summary>
		/// <param name="diskControllerDevice">The disk controller device.</param>
		/// <returns></returns>
		private List<Device> CreateDisk(Device diskControllerDevice)
		{
			var controller = diskControllerDevice as IDiskControllerDevice;
			var devices = new List<Device>();

			for (uint drive = 0; drive < controller.MaximunDriveCount; drive++)
			{
				if (controller.Open(drive))
				{
					if (controller.GetTotalSectors(drive) == 0)
						continue;

					var diskDevice = new DiskDevice(drive, false);
					diskDevice.Setup(diskControllerDevice);

					devices.Add(diskDevice);
				}
			}

			return devices;
		}

		/// <summary>
		/// Creates the disk devices.
		/// </summary>
		public void CreateDiskDevices()
		{
			// FIXME: Do not create disk devices if this method executed more than once

			// Find disk controller devices
			var controllers = deviceManager.GetDeviceServices<IDiskControllerDevice>(DeviceStatus.Online);

			// For each controller
			foreach (var device in controllers)
			{
				// Create disk devices
				var disks = CreateDisk(device);

				// Add them to the device manager
				foreach (var disk in disks)
				{
					deviceManager.Add(disk);
				}
			}
		}
	}
}
