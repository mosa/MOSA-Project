// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.Framework;

namespace Mosa.DeviceDriver.PCI.QEMU;

// QEMU Standard VGA
// https://www.qemu.org/docs/master/specs/standard-vga.html#pci-spec

// Bochs VBE Extensions
// https://wiki.osdev.org/Bochs_VBE_Extensions

//[PCIDeviceDriver(VendorID = 0x1234, DeviceID = 0x1111, Platforms = PlatformArchitecture.X86AndX64)]
public class QEMUStandardVGA : BaseDeviceDriver
{
	public override void Initialize() => Device.Name = "QEMUStandardVGA";
}
