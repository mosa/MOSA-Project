// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.Drivers.VirtIO;

/// <summary>
/// Generic VirtIO flags to send to a device, most notably for querying data and dealing with virtqueues.
/// </summary>
public static class VirtIOPCICommonConfiguration
{
	public const byte DeviceFeatureSelect = 0;
	public const byte DeviceFeature = 4;
	public const byte DriverFeatureSelect = 8;
	public const byte DriverFeature = 12;
	public const byte ConfigMsixVector = 16;
	public const byte NumQueues = 18;
	public const byte DeviceStatus = 20;
	public const byte ConfigGeneration = 21;

	public const byte QueueSelect = 22;
	public const byte QueueSize = 24;
	public const byte QueueMsixVector = 26;
	public const byte QueueEnable = 28;
	public const byte QueueNotifyOff = 30;
	public const byte QueueDesc = 32;
	public const byte QueueDriver = 40;
	public const byte QueueDevice = 48;
	public const byte QueueNotifyData = 56;
	public const byte QueueReset = 58;
}
