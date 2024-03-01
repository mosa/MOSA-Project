// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.Disks;

/// <summary>
/// An interface used for interacting directly with a disk controller device. It allows operating every disk this controller supports.
/// </summary>
public interface IDiskControllerDevice
{
	uint MaximumDriveCount { get; }

	bool Open(uint drive);

	bool Release(uint drive);

	bool ReadBlock(uint drive, uint block, uint count, byte[] data);

	bool WriteBlock(uint drive, uint block, uint count, byte[] data);

	uint GetSectorSize(uint drive);

	uint GetTotalSectors(uint drive);

	bool CanWrite(uint drive);
}
