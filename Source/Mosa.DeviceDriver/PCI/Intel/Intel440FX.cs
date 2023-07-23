// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;

// Intel® 440FX PCIset 82441FX (PMC) and 82442FX (DBX)
// http://download.intel.com/design/chipsets/specupdt/29765406.pdf

namespace Mosa.DeviceDriver.PCI.Intel;

/// <summary>
/// </summary>
//[PCIDeviceDriver(VendorID = 0x8086, DeviceID = 0x1237, Platforms = PlatformArchitecture.X86AndX64)]
public class Intel440FX : GenericHostBridgeController
{
	public override void Initialize()
	{
		Device.Name = "Intel440FX";
	}
}
