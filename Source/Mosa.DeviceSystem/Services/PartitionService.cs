// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.Services;

/// <summary>
/// Initializes all partitions within a disk.
/// </summary>
public class PartitionService : BaseService
{
	private DeviceService deviceService;

	protected override void Initialize()
	{
		deviceService = ServiceManager.GetFirstService<DeviceService>();
	}

	public void CreatePartitionDevices()
	{
		// FIXME: Do not create multiple partition devices if this method executed more than once

		var disks = deviceService.GetDevices<IDiskDevice>(DeviceStatus.Online);

		// Find all online disk devices
		foreach (var device in disks)
		{
			var diskDevice = device.DeviceDriver as IDiskDevice;

			var mbr = new MasterBootBlock(diskDevice);
			if (!mbr.Valid)
				return;

			for (var i = 0U; i < MasterBootBlock.MaxMBRPartitions; i++)
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

				deviceService.Initialize(new PartitionDeviceDriver(), device, true, configuration, null, null);
			}
		}
	}
}
