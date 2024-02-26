// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.VirtIO;

/// <summary>
/// Describes a generic VirtIO virtqueue used ring. It is used by the device to fill in data.
/// </summary>
public static class VirtIOQueueUsedRing
{
	public const byte Flags = 0;
	public const byte Index = 2;
	public const byte Id = 4;
	public const byte Length = 8;
	public const byte AvailableEvent = 12;
}
