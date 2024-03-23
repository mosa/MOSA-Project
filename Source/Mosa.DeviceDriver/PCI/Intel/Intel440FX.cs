// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.Framework;

namespace Mosa.DeviceDriver.PCI.Intel;

// Intel® 440FX PCIset 82441FX (PMC) and 82442FX (DBX)
//http://download.intel.com/design/chipsets/specupdt/29765406.pdf

/// <summary>
/// A generic Intel chipset, implemented by QEMU (also known as i440FX). It uses the <see cref="IntelPIIX3"/> southbridge.
/// </summary>
//[PCIDeviceDriver(VendorID = 0x8086, DeviceID = 0x1237, Platforms = PlatformArchitecture.X86AndX64)]
public class Intel440FX : BaseDeviceDriver
{
	public override void Initialize() => Device.Name = "Intel440FX";
}
