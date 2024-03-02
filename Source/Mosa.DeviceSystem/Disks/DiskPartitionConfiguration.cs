// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.Framework;

namespace Mosa.DeviceSystem.Disks;

/// <summary>
/// Describes the partition configuration of a disk. Implements <see cref="BaseDeviceConfiguration"/>.
/// </summary>
public class DiskPartitionConfiguration : BaseDeviceConfiguration
{
	public uint StartLBA { get; set; }

	public uint TotalBlocks { get; set; }

	public bool ReadOnly { get; set; }

	public uint Index { get; set; }
}
