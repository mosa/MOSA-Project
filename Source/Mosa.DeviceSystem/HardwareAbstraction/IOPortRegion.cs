// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.HardwareAbstraction;

/// <summary>
/// Describes a sized region in a port-mapped I/O device.
/// </summary>
public struct IOPortRegion
{
	public ushort BaseIOPort { get; }

	public ushort Size { get; }

	public IOPortRegion(ushort baseIOPort, ushort size)
	{
		BaseIOPort = baseIOPort;
		Size = size;
	}
}
