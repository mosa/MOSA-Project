// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;

// http://download.intel.com/support/processors/quark/sb/quarkdatasheetrev02.pdf

namespace Mosa.DeviceDriver.PCI.Intel.QuarkSoC
{
	/// <summary>
	/// </summary>
	//[PCIDeviceDriver(VendorID = 0x8086, DeviceID = 0x0934, ClassCode = 0X0C, SubClassCode = 0x80, ProgIF = 0x00, RevisionID = 0x10, Platforms = PlatformArchitecture.X86AndX64)]
	public class IntelGPIOController : BaseDeviceDriver
	{
		public override void Initialize()
		{
			Device.Name = "IntelGPIOController";
		}
	}
}
