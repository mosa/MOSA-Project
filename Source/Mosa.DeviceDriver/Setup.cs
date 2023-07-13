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
				Name = "StandardKeyboard",
				Platform = PlatformArchitecture.X86AndX64,
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
				Platform = PlatformArchitecture.X86AndX64,
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
				Platform = PlatformArchitecture.X86AndX64,
				AutoLoad = true,
				BasePort = 0x0CF8,
				PortRange = 8,
				Factory = () => new ISA.PCIController()
			},

			new PCIDeviceDriverRegistryEntry
			{
				Name = "PCIGenericHostBridge",
				Platform = PlatformArchitecture.X86AndX64,
				BusType = DeviceBusType.PCI,
				ClassCode = 0x06,
				SubClassCode = 0x00,
				PCIFields =  PCIField.ClassCode | PCIField.SubClassCode,
				Factory = () => new PCI.GenericHostBridgeController()
			},

			new ISADeviceDriverRegistryEntry
			{
				Name = "IDEController",
				Platform = PlatformArchitecture.X86AndX64,
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
				Platform = PlatformArchitecture.X86AndX64,
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
				Name = "Intel4SeriesChipsetDRAMController",
				Platform = PlatformArchitecture.X86AndX64,
				BusType = DeviceBusType.PCI,
				VendorID = 0x8086,
				DeviceID = 0x2E10,
				PCIFields = PCIField.VendorID | PCIField.DeviceID,
				Factory = () => new PCI.Intel.Intel4SeriesChipsetDRAMController()
			},

			new PCIDeviceDriverRegistryEntry
			{
				Name = "Intel4SeriesChipsetIntegratedGraphicsController",
				Platform = PlatformArchitecture.X86AndX64,
				BusType = DeviceBusType.PCI,
				VendorID = 0x8086,
				DeviceID = 0x2E10,
				PCIFields = PCIField.VendorID | PCIField.DeviceID,
				Factory = () => new PCI.Intel.Intel4SeriesChipsetIntegratedGraphicsController()
			},

			new PCIDeviceDriverRegistryEntry
			{
				Name = "Intel4SeriesChipsetIntegratedGraphicsController2E13",
				Platform = PlatformArchitecture.X86AndX64,
				BusType = DeviceBusType.PCI,
				VendorID = 0x8086,
				DeviceID = 0x2E10,
				PCIFields = PCIField.VendorID | PCIField.DeviceID,
				Factory = () => new PCI.Intel.Intel4SeriesChipsetIntegratedGraphicsController2E13()
			},

			new PCIDeviceDriverRegistryEntry
			{
				Name = "Intel4SeriesChipsetPCIExpressRootPort",
				Platform = PlatformArchitecture.X86AndX64,
				BusType = DeviceBusType.PCI,
				VendorID = 0x8086,
				DeviceID = 0x2E10,
				PCIFields = PCIField.VendorID | PCIField.DeviceID,
				Factory = () => new PCI.Intel.Intel4SeriesChipsetPCIExpressRootPort()
			},

			new PCIDeviceDriverRegistryEntry
			{
				Name = "Intel4SeriesChipsetPCIExpressRootPort",
				Platform = PlatformArchitecture.X86AndX64,
				BusType = DeviceBusType.PCI,
				VendorID = 0x8086,
				DeviceID = 0x2E10,
				PCIFields = PCIField.VendorID | PCIField.DeviceID,
				Factory = () => new PCI.Intel.Intel4SeriesChipsetPCIExpressRootPort()
			},

			new PCIDeviceDriverRegistryEntry
			{
				Name = "Intel440FX",
				Platform = PlatformArchitecture.X86AndX64,
				BusType = DeviceBusType.PCI,
				VendorID = 0x8086,
				DeviceID = 0x1237,
				ClassCode = 0x06,
				SubClassCode = 0x00,
				PCIFields = PCIField.VendorID | PCIField.DeviceID | PCIField.ClassCode | PCIField.SubClassCode,
				Factory = () => new PCI.Intel.Intel440FX()
			},

			new PCIDeviceDriverRegistryEntry
			{
				Name = "IntelPIIX3",
				Platform = PlatformArchitecture.X86AndX64,
				BusType = DeviceBusType.PCI,
				VendorID = 0x8086,
				DeviceID = 0x7000,
				PCIFields = PCIField.VendorID | PCIField.DeviceID,
				Factory = () => new PCI.Intel.IntelPIIX3()
			},

			new PCIDeviceDriverRegistryEntry
			{
				Name = "IntelPIIX4",
				Platform = PlatformArchitecture.X86AndX64,
				BusType = DeviceBusType.PCI,
				VendorID = 0x8086,
				DeviceID = 0x7113,
				PCIFields = PCIField.VendorID | PCIField.DeviceID,
				Factory = () => new PCI.Intel.IntelPIIX4()
			},

			new PCIDeviceDriverRegistryEntry
			{
				Name = "IntelGPIOController",
				Platform = PlatformArchitecture.X86AndX64,
				BusType = DeviceBusType.PCI,
				VendorID = 0x8086,
				DeviceID = 0x0934,
				ClassCode = 0X0C,
				SubClassCode = 0x80,
				ProgIF = 0x00,
				RevisionID = 0x10,
				PCIFields = PCIField.VendorID | PCIField.DeviceID | PCIField.ClassCode | PCIField.SubClassCode | PCIField.ProgIF | PCIField.RevisionID,
				Factory = () => new PCI.Intel.QuarkSoC.IntelGPIOController()
			},

			new PCIDeviceDriverRegistryEntry
			{
				Name = "IntelHSUART",
				Platform = PlatformArchitecture.X86AndX64,
				BusType = DeviceBusType.PCI,
				VendorID = 0x8086,
				DeviceID = 0x0936,
				ClassCode = 0X07,
				SubClassCode = 0x80,
				ProgIF = 0x02,
				RevisionID = 0x10,
				PCIFields = PCIField.VendorID | PCIField.DeviceID | PCIField.ClassCode | PCIField.SubClassCode | PCIField.ProgIF | PCIField.RevisionID,
				Factory = () => new PCI.Intel.QuarkSoC.IntelHSUART()
			},

			new PCIDeviceDriverRegistryEntry
			{
				Name = "PCIIDEInterface",
				Platform = PlatformArchitecture.X86AndX64,
				BusType = DeviceBusType.PCI,
				VendorID = 0x8086,
				DeviceID = 0x7010,
				ClassCode = 0X01,
				SubClassCode = 0x01,
				ProgIF = 0x80,
				PCIFields = PCIField.VendorID | PCIField.DeviceID | PCIField.ClassCode | PCIField.SubClassCode | PCIField.ProgIF,
				Factory = () => new PCI.Intel.PCIIDEInterface()
			},

			new PCIDeviceDriverRegistryEntry
			{
				Name = "VirtIOGPU",
				Platform = PlatformArchitecture.X86AndX64 | PlatformArchitecture.ARMv8A32,
				BusType = DeviceBusType.PCI,
				VendorID = 0x1AF4,
				DeviceID = 0x1050,
				PCIFields = PCIField.VendorID | PCIField.DeviceID,
				Factory = () => new PCI.VirtIO.VirtIOGPU()
			},

			new PCIDeviceDriverRegistryEntry
			{
				Name = "VMwareSVGA2",
				Platform = PlatformArchitecture.X86AndX64,
				BusType = DeviceBusType.PCI,
				VendorID = 0x15AD,
				DeviceID = 0x0405,
				PCIFields = PCIField.VendorID | PCIField.DeviceID,
				Factory = () => new PCI.VMware.VMwareSVGA2()
			},

			new ISADeviceDriverRegistryEntry
			{
				Name = "ACPI",
				Platform = PlatformArchitecture.X86,
				AutoLoad = true,
				Factory = () => new ISA.ACPI.ACPIDriver()
			},
		};
	}
}
