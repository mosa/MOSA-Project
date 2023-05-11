// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.VirtIO;

public static class VirtIoQueueDescriptor
{
	public const byte Phys = 0;
	public const byte Len = 8;
	public const byte Flags = 12;
	public const byte NextDescIdx = 14;
}
