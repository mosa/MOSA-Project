// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem;

/// <summary>
/// Disk Device Configuration
/// </summary>
public class DiskDeviceConfiguration : BaseDeviceConfiguration
{
	public uint DriveNbr { get; set; }

	public bool ReadOnly { get; set; }
}
