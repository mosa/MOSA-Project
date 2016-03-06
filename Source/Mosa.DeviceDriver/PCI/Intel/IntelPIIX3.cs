// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.HardwareSystem;

// PCI ISA IDE Xcelerator (PIIX3)
// http://download.intel.com/design/intarch/datashts/29055002.pdf

namespace Mosa.DeviceDriver.PCI.Intel
{
	/// <summary>
	/// </summary>
	//[PCIDeviceDriver(VendorID = 0x8086, DeviceID = 0x7000, Platforms = PlatformArchitecture.X86AndX64)]
	public class IntelPIIX3 : HardwareDevice
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="IntelPIIX3"/> class.
		/// </summary>
		public IntelPIIX3()
		{
		}

		/// <summary>
		/// Setups this hardware device driver
		/// </summary>
		/// <returns></returns>
		public override bool Setup(HardwareResources hardwareResources)
		{
			this.HardwareResources = hardwareResources;
			base.Name = "IntelPIIX3";

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
