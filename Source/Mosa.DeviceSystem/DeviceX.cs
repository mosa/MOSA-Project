// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Device
	/// </summary>
	public class DeviceX
	{
		public string Name { get; set; }
		public DeviceDriverRegistryEntry Driver { get; set; }
		public DeviceStatus Status { get; set; }
		public IService Service { get; set; }
		public DeviceX Parent { get; set; }
		public List<DeviceX> Children { get; } = new List<DeviceX>();
		public HardwareResources Resources;
	}
}
