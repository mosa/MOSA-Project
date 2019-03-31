// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;

// http://download.intel.com/support/processors/quark/sb/quarkdatasheetrev02.pdf

namespace Mosa.DeviceDriver.PCI.Intel.QuarkSoC
{
	/// <summary>
	/// </summary>
	//[PCIDeviceDriver(VendorID = 0x8086, DeviceID = 0x0936, ClassCode = 0X07, SubClassCode = 0x80, ProgIF = 0x02, RevisionID = 0x10, Platforms = PlatformArchitecture.X86AndX64)]
	public class IntelHSUART : BaseDeviceDriver
	{
		public override void Initialize()
		{
			Device.Name = "IntelHSUART";
		}
	}
}
