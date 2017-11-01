// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Device
	/// </summary>
	public class Device
	{
		public string Name { get; set; }
		public DeviceDriverRegistryEntry Driver { get; set; }
		public DeviceStatus Status { get; set; }
		public IService Service { get; set; }
		public Device Parent { get; set; }
		public List<Device> Children { get; } = new List<Device>();
		public HardwareResources Resources;
	}
}
