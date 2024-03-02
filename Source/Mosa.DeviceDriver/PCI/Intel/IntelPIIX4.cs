// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.Framework;

namespace Mosa.DeviceDriver.PCI.Intel;

// PCI ISA IDE Xcelerator (PIIX4)
//http://www.intel.com/assets/pdf/datasheet/290562.pdf

/// <summary>
/// </summary>
//[PCIDeviceDriver(VendorID = 0x8086, DeviceID = 0x7113, Platforms = PlatformArchitecture.X86AndX64)]
public class IntelPIIX4 : BaseDeviceDriver
{
	public override void Initialize() => Device.Name = "IntelPIIX4";
}
