// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.Framework;

namespace Mosa.DeviceSystem.Disks;

/// <summary>
/// An implementation of the <see cref="IPartitionDevice"/> interface, using an <see cref="IDiskDevice"/>.
/// </summary>
public class PartitionDeviceDriver : BaseDeviceDriver, IPartitionDevice
{
	public bool ReadOnly { get; protected set; }

	public uint StartBlock { get; private set; }

	public uint BlockCount { get; private set; }

	public uint BlockSize { get; private set; }

	public bool CanWrite => !ReadOnly;

	private IDiskDevice diskDevice;

	public override void Initialize()
	{
		if (Device.Configuration is not DiskPartitionConfiguration configuration)
		{
			Device.Status = DeviceStatus.Error;
			return;
		}

		StartBlock = configuration.StartLBA;
		BlockCount = configuration.TotalBlocks;
		ReadOnly = configuration.ReadOnly;

		diskDevice = Device.Parent.DeviceDriver as IDiskDevice;
		if (diskDevice == null)
		{
			Device.Status = DeviceStatus.Error;
			return;
		}

		BlockSize = diskDevice.BlockSize;

		Device.ComponentID = StartBlock;
		Device.Name = StartBlock == 0 ? $"{Device.Parent.Name}/Raw" : $"{Device.Parent.Name}/Partition{(configuration.Index + 1)}";
	}

	public override void Probe() => Device.Status = DeviceStatus.Available;

	public override void Start() => Device.Status = DeviceStatus.Online;

	public override bool OnInterrupt() => true;

	public byte[] ReadBlock(uint block, uint count) => diskDevice.ReadBlock(block + StartBlock, count);

	public bool ReadBlock(uint block, uint count, byte[] data) => diskDevice.ReadBlock(block + StartBlock, count, data);

	public bool WriteBlock(uint block, uint count, byte[] data) => diskDevice.WriteBlock(block + StartBlock, count, data);
}
