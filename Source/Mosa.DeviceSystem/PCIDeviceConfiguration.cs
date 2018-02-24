// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Disk Device Configuration
	/// </summary>
	public class PCIDeviceConfiguration : BaseDeviceConfiguration
	{
		public byte Bus { get; set; }
		public byte Slot { get; set; }
		public byte Function { get; set; }
	}
}
