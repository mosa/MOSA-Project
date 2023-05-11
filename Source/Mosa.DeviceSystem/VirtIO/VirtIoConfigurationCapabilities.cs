// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.VirtIO;

public static class VirtIoConfigurationCapabilities
{
	public const byte Common = 1;
	public const byte Notify = 2;
	public const byte Isr = 3;
	public const byte Device = 4;
	public const byte Pci = 5;
	public const byte SharedMemory = 8;
	public const byte Vendor = 9;
}
