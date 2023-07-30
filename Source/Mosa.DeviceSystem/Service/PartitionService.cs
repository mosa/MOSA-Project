// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.Service;

/// <summary>
/// Partition Manager
/// </summary>
public class PartitionService : BaseService
{
	/// <summary>
	/// The device service
	/// </summary>
	protected DeviceService DeviceService;

	/// <summary>
	/// Initializes this instance.
	/// </summary>
	protected override void Initialize()
	{
		DeviceService = ServiceManager.GetFirstService<DeviceService>();
	}

	/// <summary>
	/// Creates the partition devices.
	/// </summary>
	public void CreatePartitionDevices()
	{
		// FIXME: Do not create multiple partition devices if this method executed more than once

		var disks = DeviceService.GetDevices<IDiskDevice>(DeviceStatus.Online);

		// Find all online disk devices
		foreach (var device in disks)
		{
			var diskDevice = device.DeviceDriver as IDiskDevice;

			var mbr = new MasterBootBlock(diskDevice);

			if (!mbr.Valid)
				return;

			for (uint i = 0; i < MasterBootBlock.MaxMBRPartitions; i++)
			{
				if (mbr.Partitions[i].PartitionType == PartitionType.Empty)
					continue;

				var configuration = new DiskPartitionConfiguration
				{
					Index = i,
					StartLBA = mbr.Partitions[i].StartLBA,
					TotalBlocks = mbr.Partitions[i].TotalBlocks,
					ReadOnly = false,
				};

				DeviceService.Initialize(new PartitionDeviceDriver(), device, true, configuration, null, null);
			}
		}
	}
}
