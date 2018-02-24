// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Partition Manager
	/// </summary>
	public class PartitionManager
	{
		/// <summary>
		/// The device manager
		/// </summary>
		protected DeviceManager deviceManager;

		/// <summary>
		/// Initializes a new instance of the <see cref="PartitionManager"/> class.
		/// </summary>
		/// <param name="deviceManager">The device manager.</param>
		public PartitionManager(DeviceManager deviceManager)
		{
			this.deviceManager = deviceManager;
		}

		/// <summary>
		/// Creates the partition devices.
		/// </summary>
		public void CreatePartitionDevices()
		{
			// FIXME: Do not create multiple partition devices if this method executed more than once

			var disks = deviceManager.GetDevices<IDiskDevice>(DeviceStatus.Online);

			// Find all online disk devices
			foreach (var device in disks)
			{
				var diskDevice = device as IDiskDevice;

				var mbr = new MasterBootBlock(diskDevice);

				if (!mbr.Valid)
					return;

				for (uint i = 0; i < MasterBootBlock.MaxMBRPartitions; i++)
				{
					if (mbr.Partitions[i].PartitionType != PartitionType.Empty)
					{
						var configuration = new DiskPartitionConfiguration()
						{
							Index = i,
							StartLBA = mbr.Partitions[i].StartLBA,
							TotalBlocks = mbr.Partitions[i].TotalBlocks,
							ReadOnly = false,
						};

						deviceManager.Initialize(new PartitionDeviceDriver(), device, configuration, null, null);
					}
				}
			}
		}
	}
}
