﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.Framework;

namespace Mosa.DeviceDriver.PCI.Intel;

// PCI ISA IDE Xcelerator (PIIX4)
//http://www.intel.com/assets/pdf/datasheet/290562.pdf

/// <summary>
/// The IDE controller for the <see cref="IntelPIIX4"/> southbridge, interfacing via PCI.
/// </summary>
//[PCIDeviceDriver(VendorID = 0x8086, DeviceID = 0x7010, Platforms = PlatformArchitecture.X86AndX64)]
//Base Class Code: 01h=Mass storage device.
//Sub-Class Code: 01h=IDE controller.
//Programming Interface: 80h= Capable of IDE bus master operation.
public class PCIIDEInterface : BaseDeviceDriver
{
	public override void Initialize() => Device.Name = "IDEInterface";
}
