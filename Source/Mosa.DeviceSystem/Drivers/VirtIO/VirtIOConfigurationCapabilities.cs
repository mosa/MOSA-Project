// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.Drivers.VirtIO;

/// <summary>
/// VirtIO configuration capabilities of a device.
/// </summary>
public static class VirtIOConfigurationCapabilities
{
	public const byte Common = 1;
	public const byte Notify = 2;
	public const byte ISR = 3;
	public const byte Device = 4;
	public const byte PCI = 5;
	public const byte SharedMemory = 8;
	public const byte Vendor = 9;
}
