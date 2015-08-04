// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.DeviceSystem
{
	/// <summary>
	///
	/// </summary>
	public class DiskControllerManager
	{
		/// <summary>
		///
		/// </summary>
		protected IDeviceManager deviceManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="DiskControllerManager"/> class.
		/// </summary>
		/// <param name="deviceManager">The device manager.</param>
		public DiskControllerManager(IDeviceManager deviceManager)
		{
			this.deviceManager = deviceManager;
		}

		/// <summary>
		/// Creates the devices.
		/// </summary>
		/// <param name="diskControllerDevice">The disk controller device.</param>
		/// <returns></returns>
		private LinkedList<IDevice> CreateDevices(IDiskControllerDevice diskControllerDevice)
		{
			LinkedList<IDevice> devices = new LinkedList<IDevice>();

			for (uint drive = 0; drive < diskControllerDevice.MaximunDriveCount; drive++)
			{
				if (diskControllerDevice.Open(drive))
				{
					IDiskDevice diskDevice = new DiskDevice(diskControllerDevice, drive, false);
					devices.AddLast(diskDevice as IDevice);
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
			LinkedList<IDevice> devices = deviceManager.GetDevices(new FindDevice.IsDiskControllerDevice(), new FindDevice.IsOnline());

			// For each controller
			foreach (IDevice device in devices)
			{
				IDiskControllerDevice controller = device as IDiskControllerDevice;

				// Create disk devices
				LinkedList<IDevice> disks = CreateDevices(controller);

				// Add them to the device manager
				foreach (IDevice disk in disks)
					deviceManager.Add(disk);
			}
		}
	}
}
