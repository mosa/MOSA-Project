// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;

// Intel82540EM Gigabit Ethernet Controller
// http://www.intel.com/content/dam/doc/datasheet/82540ep-gbe-controller-datasheet.pdf

namespace Mosa.DeviceDriver.PCI.Intel
{
	/// <summary>
	/// </summary>
	//[PCIDeviceDriver(VendorID = 0x8086, DeviceID = 0x100E, Platforms = PlatformArchitecture.X86AndX64)]
	public class Intel82540EM : DeviceSystem.DeviceDriver
	{
		protected override void Initialize()
		{
			Device.Name = "Intel82540EM";
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
