// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;

// PCI ISA IDE Xcelerator (PIIX4)
// http://www.intel.com/assets/pdf/datasheet/290562.pdf

namespace Mosa.DeviceDriver.PCI.Intel
{
	/// <summary>
	/// </summary>
	//[PCIDeviceDriver(VendorID = 0x8086, DeviceID = 0x7113, Platforms = PlatformArchitecture.X86AndX64)]
	public class IntelPIIX4 : DeviceSystem.DeviceDriver
	{
		protected override void Initialize()
		{
			Device.Name = "IntelPCI_ISA_IDE_Xcelerator_PIIX4";
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
