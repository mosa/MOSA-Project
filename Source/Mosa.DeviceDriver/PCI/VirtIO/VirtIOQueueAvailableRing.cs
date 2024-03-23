// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceDriver.PCI.VirtIO;

/// <summary>
/// Describes a generic VirtIO virtqueue used ring. It is used by the driver to query filled in data.
/// </summary>
public static class VirtIOQueueAvailableRing
{
	public const byte Flags = 0;
	public const byte Index = 2;
	public const byte Rings = 4;
	public const byte UsedEvent = 6;
}
