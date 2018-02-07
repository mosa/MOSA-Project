// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;

// 82541EI Gigabit Ethernet Controller
// http://www.intel.com/content/dam/doc/datasheet/82541-gbe-controller-datasheet.pdf

namespace Mosa.DeviceDriver.PCI.Intel
{
	/// <summary>
	/// </summary>
	//[PCIDeviceDriver(VendorID = 0x8086, DeviceID = 0x1013, Platforms = PlatformArchitecture.X86AndX64)]
	public class Intel82541EI : DeviceSystem.DeviceDriver
	{
		protected override void Initialize()
		{
			Device.Name = "Intel82541EI";
		}

		public override void Start()
		{
			if (Device.Status != DeviceStatus.Available)
				return;

			Device.Status = DeviceStatus.Online;
		}

		public override bool OnInterrupt()
		{
			return true;
		}
	}
}
