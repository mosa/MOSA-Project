// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;

// PCI ISA IDE Xcelerator (PIIX3)
// http://download.intel.com/design/intarch/datashts/29055002.pdf

namespace Mosa.DeviceDriver.PCI.Intel
{
	/// <summary>
	/// </summary>
	//[PCIDeviceDriver(VendorID = 0x8086, DeviceID = 0x7000, Platforms = PlatformArchitecture.X86AndX64)]
	public class IntelPIIX3 : DeviceSystem.DeviceDriver
	{
		protected override void Initialize()
		{
			Device.Name = "IntelPIIX3";
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
