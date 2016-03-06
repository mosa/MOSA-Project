// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.HardwareSystem;

namespace Mosa.DeviceSystem
{
	/// <summary>
	///
	/// </summary>
	public class PartitionManager
	{
		/// <summary>
		///
		/// </summary>
		protected IDeviceManager deviceManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="PartitionManager"/> class.
		/// </summary>
		/// <param name="deviceManager">The device manager.</param>
		public PartitionManager(IDeviceManager deviceManager)
		{
			this.deviceManager = deviceManager;
		}

		/// <summary>
		/// Creates the partition devices.
		/// </summary>
		public void CreatePartitionDevices()
		{
			// FIXME: Do not create multiple partition devices if this method executed more than once

			// Find all online disk devices
			foreach (var device in deviceManager.GetDevices(new IsDiskDevice(), new IsOnline()))
			{
				var diskDevice = device as IDiskDevice;

				var mbr = new MasterBootBlock(diskDevice);

				if (!mbr.Valid)
					return;

				for (uint i = 0; i < MasterBootBlock.MaxMBRPartitions; i++)
				{
					if (mbr.Partitions[i].PartitionType != PartitionType.Empty)
					{
						deviceManager.Add(new PartitionDevice(diskDevice, mbr.Partitions[i], false));
					}
				}
			}
		}
	}
}
