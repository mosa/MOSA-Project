// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.VirtIO;

public static class VirtIOFeatures
{
	public const byte IndirectDescriptor = 28;
	public const byte EventIndex = 29;
	public const byte Version1 = 32;
	public const byte AccessPlatform = 33;
	public const byte RingPacked = 34;
	public const byte InOrder = 35;
	public const byte OrderPlatform = 36;
	public const byte SrIov = 37;
	public const byte NotificationData = 38;
	public const byte NotificationConfigurationData = 39;
	public const byte RingReset = 40;
}
