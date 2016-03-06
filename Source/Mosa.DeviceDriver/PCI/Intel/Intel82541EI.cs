// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;

// 82541EI Gigabit Ethernet Controller
// http://www.intel.com/content/dam/doc/datasheet/82541-gbe-controller-datasheet.pdf

namespace Mosa.DeviceDriver.PCI.Intel
{
	/// <summary>
	/// </summary>
	//[PCIDeviceDriver(VendorID = 0x8086, DeviceID = 0x1013, Platforms = PlatformArchitecture.X86AndX64)]
	public class Intel82541EI : HardwareDevice
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Intel82541EI"/> class.
		/// </summary>
		public Intel82541EI()
		{
		}

		/// <summary>
		/// Setups this hardware device driver
		/// </summary>
		/// <returns></returns>
		public override bool Setup(HardwareResources hardwareResources)
		{
			this.HardwareResources = hardwareResources;
			base.Name = "Intel82541EI";

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
