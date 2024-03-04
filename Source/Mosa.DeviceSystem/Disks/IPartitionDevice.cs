// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.Disks;

/// <summary>
/// An interface used for interacting directly with a partition on a disk. Unlike the <see cref="GenericPartition"/> class, which is only
/// used by the <see cref="MasterBootBlock"/>, this interface is used by the device driver framework (and subsequently, by the kernel) to
/// list partitions on a disk.
/// </summary>
public interface IPartitionDevice
{
	uint StartBlock { get; }

	uint BlockCount { get; }

	uint BlockSize { get; }

	bool CanWrite { get; }

	byte[] ReadBlock(uint block, uint count);

	bool ReadBlock(uint block, uint count, byte[] data);

	bool WriteBlock(uint block, uint count, byte[] data);
}
