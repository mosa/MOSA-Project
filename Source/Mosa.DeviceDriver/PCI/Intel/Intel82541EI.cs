// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;

// Intel82540EM Gigabit Ethernet Controller
// http://www.intel.com/content/dam/doc/datasheet/82540ep-gbe-controller-datasheet.pdf

namespace Mosa.DeviceDriver.PCI.Intel
{
	/// <summary>
	/// </summary>
	//[PCIDeviceDriver(VendorID = 0x8086, DeviceID = 0x100E, Platforms = PlatformArchitecture.X86AndX64)]
	public class Intel82540EM : HardwareDevice
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Intel82540EM"/> class.
		/// </summary>
		public Intel82540EM()
		{
		}

		/// <summary>
		/// Setups this hardware device driver
		/// </summary>
		/// <returns></returns>
		public override bool Setup(IHardwareResources hardwareResources)
		{
			this.hardwareResources = hardwareResources;
			base.name = "Intel82540EM";

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
