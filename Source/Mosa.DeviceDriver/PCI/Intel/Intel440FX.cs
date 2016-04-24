// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.HardwareSystem;

// Intel® 440FX PCIset 82441FX (PMC) and 82442FX (DBX)
// http://download.intel.com/design/chipsets/specupdt/29765406.pdf

namespace Mosa.DeviceDriver.PCI.Intel
{
	/// <summary>
	/// </summary>
	//[PCIDeviceDriver(VendorID = 0x8086, DeviceID = 0x1237, Platforms = PlatformArchitecture.X86AndX64)]
	public class Intel440FX : HardwareDevice
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Intel440FX"/> class.
		/// </summary>
		public Intel440FX()
		{
		}

		/// <summary>
		/// Setups this hardware device driver
		/// </summary>
		/// <returns></returns>
		public override bool Setup(HardwareResources hardwareResources)
		{
			this.HardwareResources = hardwareResources;
			base.Name = "Intel440FX";

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
