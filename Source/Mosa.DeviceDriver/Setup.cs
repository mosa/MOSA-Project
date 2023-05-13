// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.DeviceSystem;

namespace Mosa.DeviceDriver;

public static class Setup
{
	public static List<DeviceDriverRegistryEntry> GetDeviceDriverRegistryEntries()
	{
		return new List<DeviceDriverRegistryEntry>
		{
			new ISADeviceDriverRegistryEntry
			{
				Name = "ACPI",
				Platforms = PlatformArchitecture.X86,
				AutoLoad = true,
				Factory = () => new ISA.ACPI()
			},

			new ISADeviceDriverRegistryEntry
			{
				Name = "StandardKeyboard",
				Platforms = PlatformArchitecture.X86AndX64,
				AutoLoad = true,
				BasePort = 0x60,
				PortRange = 1,
				AltBasePort = 0x64,
				AltPortRange = 1,
				IRQ = 1,
				Factory = () => new ISA.StandardKeyboard()
			},

			new ISADeviceDriverRegistryEntry
			{
				Name = "StandardMouse",
				Platforms = PlatformArchitecture.X86AndX64,
				AutoLoad = true,
				BasePort = 0x60,
				PortRange = 1,
				AltBasePort = 0x64,
				AltPortRange = 1,
				IRQ = 12,
				Factory = () => new ISA.StandardMouse()
			},

			new ISADeviceDriverRegistryEntry
			{
				Name = "PCIController",
				Platforms = PlatformArchitecture.X86AndX64,
				AutoLoad = true,
				BasePort = 0x0CF8,
				PortRange = 8,
				Factory = () => new ISA.PCIController()
			},

			new PCIDeviceDriverRegistryEntry
			{
				Name = "PCIGenericHostBridge",
				Platforms = PlatformArchitecture.X86AndX64,
				BusType = DeviceBusType.PCI,
				ClassCode = 0x06,
				SubClassCode = 0x00,
				PCIFields =  PCIField.ClassCode | PCIField.SubClassCode,
				Factory = () => new PCI.GenericHostBridgeController()
			},

			new ISADeviceDriverRegistryEntry
			{
				Name = "IDEController",
				Platforms = PlatformArchitecture.X86AndX64,
				AutoLoad = true,
				BasePort = 0x1F0,
				PortRange = 8,
				AltBasePort = 0x3F6,
				AltPortRange = 8,
				Factory = () => new ISA.IDEController()
			},

			new ISADeviceDriverRegistryEntry
			{
				Name = "IDEController (Secondary)",
				Platforms = PlatformArchitecture.X86AndX64,
				AutoLoad = true,
				BasePort = 0x170,
				PortRange = 8,
				AltBasePort = 0x376,
				AltPortRange = 8,
				ForceOption = "ide2",
				Factory = () => new ISA.IDEController()
			},

			new PCIDeviceDriverRegistryEntry
			{
				Name = "VirtIOGPU",
				Platforms = PlatformArchitecture.X86AndX64 | PlatformArchitecture.ARMv8A32,
				BusType = DeviceBusType.PCI,
				VendorID = 0x1AF4,
				DeviceID = 0x1050,
				PCIFields = PCIField.VendorID | PCIField.DeviceID,
				Factory = () => new PCI.VirtIO.VirtIoGpu()
			},

			new PCIDeviceDriverRegistryEntry
			{
				Name = "VMwareSVGA2",
				Platforms = PlatformArchitecture.X86AndX64,
				BusType = DeviceBusType.PCI,
				VendorID = 0x15AD,
				DeviceID = 0x0405,
				PCIFields = PCIField.VendorID | PCIField.DeviceID,
				Factory = () => new PCI.VMware.VMwareSVGA2()
			},
		};
	}
}
