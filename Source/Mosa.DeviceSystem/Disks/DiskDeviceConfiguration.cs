// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.Framework;

namespace Mosa.DeviceSystem.Disks;

/// <summary>
/// Describes the device configuration for a disk. Implements <see cref="BaseDeviceConfiguration"/>.
/// </summary>
public class DiskDeviceConfiguration : BaseDeviceConfiguration
{
	public uint DriveNbr { get; set; }

	public bool ReadOnly { get; set; }
}
