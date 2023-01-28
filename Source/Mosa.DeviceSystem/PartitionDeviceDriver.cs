// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem;

/// <summary>
/// Partition Device
/// </summary>
public class PartitionDeviceDriver : BaseDeviceDriver, IPartitionDevice
{
	/// <summary>
	/// The disk device
	/// </summary>
	private IDiskDevice diskDevice;

	/// <summary>
	/// The read only
	/// </summary>
	public bool ReadOnly { get; protected set; }

	/// <summary>
	/// Gets the start block.
	/// </summary>
	/// <value>The start block.</value>
	public uint StartBlock { get; protected set; }

	/// <summary>
	/// Gets the block count.
	/// </summary>
	/// <value>The block count.</value>
	public uint BlockCount { get; protected set; }

	/// <summary>
	/// Gets the size of the block.
	/// </summary>
	/// <value>The size of the block.</value>
	public uint BlockSize { get; private set; }

	/// <summary>
	/// Gets a value indicating whether this instance can write.
	/// </summary>
	/// <value><c>true</c> if this instance can write; otherwise, <c>false</c>.</value>
	public bool CanWrite { get { return !ReadOnly; } }

	public override void Initialize()
	{
		var configuration = Device.Configuration as DiskPartitionConfiguration;

		StartBlock = configuration.StartLBA;
		BlockCount = configuration.TotalBlocks;
		ReadOnly = configuration.ReadOnly;

		diskDevice = Device.Parent.DeviceDriver as IDiskDevice;
		BlockSize = diskDevice.BlockSize;

		Device.ComponentID = StartBlock;

		if (StartBlock == 0)
			Device.Name = Device.Parent.Name + "/Raw";
		else
			Device.Name = Device.Parent.Name + "/Partition" + (configuration.Index + 1).ToString();
	}

	public override void Probe() => Device.Status = DeviceStatus.Available;

	public override void Start()
	{
		Device.Status = DeviceStatus.Online;
	}

	/// <summary>
	/// Called when an interrupt is received.
	/// </summary>
	/// <returns></returns>
	public override bool OnInterrupt()
	{
		// TODO
		return true;
	}

	/// <summary>
	/// Reads the block.
	/// </summary>
	/// <param name="block">The block.</param>
	/// <param name="count">The count.</param>
	/// <returns></returns>
	public byte[] ReadBlock(uint block, uint count)
	{
		return diskDevice.ReadBlock(block + StartBlock, count);
	}

	/// <summary>
	/// Reads the block.
	/// </summary>
	/// <param name="block">The block.</param>
	/// <param name="count">The count.</param>
	/// <param name="data">The data.</param>
	/// <returns></returns>
	public bool ReadBlock(uint block, uint count, byte[] data)
	{
		return diskDevice.ReadBlock(block + StartBlock, count, data);
	}

	/// <summary>
	/// Writes the block.
	/// </summary>
	/// <param name="block">The block.</param>
	/// <param name="count">The count.</param>
	/// <param name="data">The data.</param>
	/// <returns></returns>
	public bool WriteBlock(uint block, uint count, byte[] data)
	{
		return diskDevice.WriteBlock(block + StartBlock, count, data);
	}
}
