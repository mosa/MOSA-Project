// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;

// PCI ISA IDE Xcelerator (PIIX4)
// http://www.intel.com/assets/pdf/datasheet/290562.pdf

namespace Mosa.DeviceDriver.PCI.Intel
{
	/// <summary>
	/// </summary>
	//[PCIDeviceDriver(VendorID = 0x8086, DeviceID = 0x7000, Platforms = PlatformArchitecture.X86AndX64)]
	public class IntelPIIX4 : HardwareDevice
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="IntelPIIX4"/> class.
		/// </summary>
		public IntelPIIX4()
		{
		}

		/// <summary>
		/// Setups this hardware device driver
		/// </summary>
		/// <returns></returns>
		public override bool Setup(IHardwareResources hardwareResources)
		{
			this.hardwareResources = hardwareResources;
			base.name = "IntelPCI_ISA_IDE_Xcelerator_PIIX4";

			return true;
		}

		/// <summary>
		/// Starts this hardware device.
		/// </summary>
		/// <returns></returns>
		public override DeviceDriverStartStatus Start()
		{
			return DeviceDriverStartStatus.Started;
		}

		/// <summary>
		/// Called when an interrupt is received.
		/// </summary>
		/// <returns></returns>
		public override bool OnInterrupt()
		{
			return true;
		}
	}
}
