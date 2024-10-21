// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.Framework;

namespace Mosa.DeviceDriver.PCI.Bochs;

// Bochs VBE Extensions
// https://wiki.osdev.org/Bochs_VBE_Extensions

//[PCIDeviceDriver(VendorID = 0x1234, DeviceID = 0x1111, Platforms = PlatformArchitecture.X86AndX64)]
public class BochsGraphicsAdaptor : BaseDeviceDriver
{
	public override void Initialize() => Device.Name = "BochsGraphicsAdaptor";
}
