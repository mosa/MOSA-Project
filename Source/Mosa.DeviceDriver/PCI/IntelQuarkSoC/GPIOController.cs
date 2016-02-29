// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;

/*
*
	http://download.intel.com/support/processors/quark/sb/quarkdatasheetrev02.pdf
 *
 */

namespace Mosa.DeviceDriver.PCI.IntelQuarkSoC
{
	/// <summary>
	/// </summary>
	[PCIDeviceDriver(VendorID = 0x8086, DeviceID = 0x0934, SubVendorID = 0x0000, ClassCode = 0X0C, SubClassCode = 0x80, ProgIF = 0x00, RevisionID = 0x10, Platforms = PlatformArchitecture.X86AndX64)]
	public class GPIOController : HardwareDevice
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GenericVGA"/> class.
		/// </summary>
		public GPIOController()
		{
		}

		/// <summary>
		/// Setups this hardware device driver
		/// </summary>
		/// <returns></returns>
		public override bool Setup(IHardwareResources hardwareResources)
		{
			this.hardwareResources = hardwareResources;
			base.name = "GPIOController";

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
