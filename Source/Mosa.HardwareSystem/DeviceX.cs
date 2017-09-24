// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.HardwareSystem
{
	/// <summary>
	/// Device
	/// </summary>
	public class DeviceX
	{
		public string Name { get; internal set; }
		public IDeviceDriver Driver { get; internal set; }
		public DeviceStatus Status { get; internal set; }
		public IService Service { get; internal set; }
		public DeviceX Parent { get; internal set; }
		public List<DeviceX> Children { get; } = new List<DeviceX>();
		public HardwareResources Resources;
	}
}
