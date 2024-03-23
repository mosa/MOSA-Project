// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceDriver.PCI.VirtIO;

/// <summary>
/// Describes a generic VirtIO queue descriptor structure.
/// </summary>
public static class VirtIOQueueDescriptor
{
	public const byte Phys = 0;
	public const byte Len = 8;
	public const byte Flags = 12;
	public const byte NextDescIdx = 14;
}
