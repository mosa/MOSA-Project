// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.Disks;

/// <summary>
/// An interface used for interacting directly with a disk device.
/// </summary>
public interface IDiskDevice
{
	bool CanWrite { get; }

	uint TotalBlocks { get; }

	uint BlockSize { get; }

	byte[] ReadBlock(uint block, uint count);

	bool ReadBlock(uint block, uint count, byte[] data);

	bool WriteBlock(uint block, uint count, byte[] data);
}
