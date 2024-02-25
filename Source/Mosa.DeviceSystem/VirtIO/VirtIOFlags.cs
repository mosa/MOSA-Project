// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.VirtIO;

/// <summary>
/// Generic VirtIO flags to send to a device for initialization.
/// </summary>
public static class VirtIOFlags
{
	public const byte Reset = 0;
	public const byte Acknowledge = 1;
	public const byte Driver = 2;
	public const byte DriverOk = 4;
	public const byte FeaturesOk = 8;
	public const byte DeviceNeedsReset = 64;
	public const byte Failed = 128;
}
