// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.Framework;

namespace Mosa.DeviceDriver.PCI.Bochs;

/// <summary>
/// The southbridge of the generic Intel chipset <see cref="BochsGraphicsAdaptor"/>.
/// </summary>
//[PCIDeviceDriver(VendorID = 0x1234, DeviceID = 0x1111, Platforms = PlatformArchitecture.X86AndX64)]
public class BochsGraphicsAdaptor : BaseDeviceDriver
{
	public override void Initialize() => Device.Name = "BochsGraphicsAdaptor";
}
