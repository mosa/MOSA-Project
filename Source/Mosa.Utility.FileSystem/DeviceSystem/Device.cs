// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.Framework;

/// <summary>
/// Device
/// </summary>
public class Device
{
	public string Name { get; set; }

	public BaseDeviceDriver DeviceDriver { get; set; }

	public DeviceStatus Status { get; set; }

	public Device Parent { get; set; }

	public List<Device> Children { get; } = new List<Device>();

	public BaseDeviceConfiguration Configuration { get; set; }

	public ulong ComponentID { get; set; }
}
