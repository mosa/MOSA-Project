// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.VirtIO;

/// <summary>
/// Flags used in a VirtIO descriptor. "HasNext" tells the driver that a next descriptor is available (essentially making it a chain),
/// and "Write" tells the device that it can write to the descriptor.
/// </summary>
public static class VirtIOQueueDescriptorFlags
{
	public const ushort HasNext = 1;
	public const ushort Write = 2;
}
