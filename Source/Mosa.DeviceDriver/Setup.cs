// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceDriver.ISA;
using Mosa.DeviceDriver.PCI.Intel;
using Mosa.DeviceDriver.PCI.Intel.QuarkSoC;
using Mosa.DeviceSystem;
using System.Collections.Generic;

namespace Mosa.DeviceDriver
{
	public static class Setup
	{
		public static List<DeviceDriverRegistryEntry> GetDeviceDriverRegistryEntries()
		{
			return new List<DeviceDriverRegistryEntry>()
			{
				new ISADeviceDriverRegistryEntry()
				{
					Name = "CMOS",
					Platforms = PlatformArchitecture.X86,
					AutoLoad = true,
					BasePort = 0x0070,
					PortRange = 2,
					Factory = delegate { return new ISA.CMOS(); }
				},

				new ISADeviceDriverRegistryEntry()
				{
					Name = "StandardKeyboard",
					Platforms = PlatformArchitecture.X86AndX64,
					AutoLoad = true,
					BasePort = 0x60,
					PortRange = 1,
					AltBasePort = 0x64,
					AltPortRange = 1,
					IRQ = 1,
					Factory = delegate { return new ISA.StandardKeyboard(); }
				},

				new ISADeviceDriverRegistryEntry()
				{
					Name = "PCIController",
					Platforms = PlatformArchitecture.X86AndX64,
					AutoLoad = true,
					BasePort = 0x0CF8,
					PortRange = 8,
					Factory = delegate { return new ISA.PCIController(); }
				},

				new ISADeviceDriverRegistryEntry()
				{
					Name = "IDEController",
					Platforms = PlatformArchitecture.X86AndX64,
					AutoLoad = true,
					BasePort = 0x1F0,
					PortRange = 8,
					AltBasePort = 0x3F6,
					AltPortRange = 8,
					Factory = delegate { return new ISA.IDEController(); }
				},

				new ISADeviceDriverRegistryEntry()
				{
					Name = "IDEController (Secondary)",
					Platforms = PlatformArchitecture.X86AndX64,
					AutoLoad = true,
					BasePort = 0x170,
					PortRange = 8,
					AltBasePort = 0x376,
					AltPortRange = 8,
					ForceOption = "ide2",
					Factory = delegate { return new ISA.IDEController(); }
				},

				//new ISADeviceDriverRegistryEntry()
				//{
				//	Name = "VGAText",
				//	Platforms = PlatformArchitecture.X86AndX64,
				//	AutoLoad = true,
				//	BasePort = 0x03B0,
				//	PortRange = 0x1F,
				//	BaseAddress = 0xB0000,
				//	AddressRange = 0x10000,
				//	Factory = delegate { return new VGAText(); }
				//},

				new PCIDeviceDriverRegistryEntry()
				{
					Name = "VMwareSGAII",
					Platforms = PlatformArchitecture.X86AndX64,
					BusType = DeviceBusType.PCI,
					VendorID = 0x15AD,
					DeviceID = 0x0405,
					PCIFields = PCIField.VendorID | PCIField.DeviceID,
					Factory = delegate { return new PCI.VMware.VMwareSVGAII(); }
				},

				new PCIDeviceDriverRegistryEntry()
				{
					Name = "Intel4SeriesChipsetDRAMController",
					Platforms = PlatformArchitecture.X86AndX64,
					BusType = DeviceBusType.PCI,
					VendorID = 0x8086,
					DeviceID = 0x2E10,
					PCIFields = PCIField.VendorID | PCIField.DeviceID,
					Factory = delegate { return new Intel4SeriesChipsetDRAMController(); }
				},

				new PCIDeviceDriverRegistryEntry()
				{
					Name = "Intel4SeriesChipsetIntegratedGraphicsController",
					Platforms = PlatformArchitecture.X86AndX64,
					BusType = DeviceBusType.PCI,
					VendorID = 0x8086,
					DeviceID = 0x2E10,
					PCIFields = PCIField.VendorID | PCIField.DeviceID,
					Factory = delegate { return new Intel4SeriesChipsetIntegratedGraphicsController(); }
				},

				new PCIDeviceDriverRegistryEntry()
				{
					Name = "Intel4SeriesChipsetIntegratedGraphicsController2E13",
					Platforms = PlatformArchitecture.X86AndX64,
					BusType = DeviceBusType.PCI,
					VendorID = 0x8086,
					DeviceID = 0x2E10,
					PCIFields = PCIField.VendorID | PCIField.DeviceID,
					Factory = delegate { return new Intel4SeriesChipsetIntegratedGraphicsController2E13(); }
				},

				new PCIDeviceDriverRegistryEntry()
				{
					Name = "Intel4SeriesChipsetPCIExpressRootPort",
					Platforms = PlatformArchitecture.X86AndX64,
					BusType = DeviceBusType.PCI,
					VendorID = 0x8086,
					DeviceID = 0x2E10,
					PCIFields = PCIField.VendorID | PCIField.DeviceID,
					Factory = delegate { return new Intel4SeriesChipsetPCIExpressRootPort(); }
				},

				new PCIDeviceDriverRegistryEntry()
				{
					Name = "Intel4SeriesChipsetPCIExpressRootPort",
					Platforms = PlatformArchitecture.X86AndX64,
					BusType = DeviceBusType.PCI,
					VendorID = 0x8086,
					DeviceID = 0x2E10,
					PCIFields = PCIField.VendorID | PCIField.DeviceID,
					Factory = delegate { return new Intel4SeriesChipsetPCIExpressRootPort(); }
				},

				new PCIDeviceDriverRegistryEntry()
				{
					Name = "Intel440FX",
					Platforms = PlatformArchitecture.X86AndX64,
					BusType = DeviceBusType.PCI,
					VendorID = 0x8086,
					DeviceID = 0x1237,
					PCIFields = PCIField.VendorID | PCIField.DeviceID,
					Factory = delegate { return new Intel440FX(); }
				},

				new PCIDeviceDriverRegistryEntry()
				{
					Name = "Intel82540EM",
					Platforms = PlatformArchitecture.X86AndX64,
					BusType = DeviceBusType.PCI,
					VendorID = 0x8086,
					DeviceID = 0x100E,
					PCIFields = PCIField.VendorID | PCIField.DeviceID,
					Factory = delegate { return new Intel82540EM(); }
				},

				new PCIDeviceDriverRegistryEntry()
				{
					Name = "Intel82541EI",
					Platforms = PlatformArchitecture.X86AndX64,
					BusType = DeviceBusType.PCI,
					VendorID = 0x8086,
					DeviceID = 0x1013,
					PCIFields = PCIField.VendorID | PCIField.DeviceID,
					Factory = delegate { return new Intel82541EI(); }
				},

				new PCIDeviceDriverRegistryEntry()
				{
					Name = "IntelPIIX3",
					Platforms = PlatformArchitecture.X86AndX64,
					BusType = DeviceBusType.PCI,
					VendorID = 0x8086,
					DeviceID = 0x7000,
					PCIFields = PCIField.VendorID | PCIField.DeviceID,
					Factory = delegate { return new IntelPIIX3(); }
				},

				new PCIDeviceDriverRegistryEntry()
				{
					Name = "IntelPIIX4",
					Platforms = PlatformArchitecture.X86AndX64,
					BusType = DeviceBusType.PCI,
					VendorID = 0x8086,
					DeviceID = 0x7113,
					PCIFields = PCIField.VendorID | PCIField.DeviceID,
					Factory = delegate { return new IntelPIIX4(); }
				},

				new PCIDeviceDriverRegistryEntry()
				{
					Name = "IntelGPIOController",
					Platforms = PlatformArchitecture.X86AndX64,
					BusType = DeviceBusType.PCI,
					VendorID = 0x8086,
					DeviceID = 0x0934,
					ClassCode = 0X0C,
					SubClassCode = 0x80,
					ProgIF = 0x00,
					RevisionID = 0x10,
					PCIFields = PCIField.VendorID | PCIField.DeviceID | PCIField.ClassCode | PCIField.SubClassCode | PCIField.ProgIF | PCIField.RevisionID,
					Factory = delegate { return new IntelGPIOController(); }
				},

				new PCIDeviceDriverRegistryEntry()
				{
					Name = "IntelHSUART",
					Platforms = PlatformArchitecture.X86AndX64,
					BusType = DeviceBusType.PCI,
					VendorID = 0x8086,
					DeviceID = 0x0936,
					ClassCode = 0X07,
					SubClassCode = 0x80,
					ProgIF = 0x02,
					RevisionID = 0x10,
					PCIFields = PCIField.VendorID | PCIField.DeviceID | PCIField.ClassCode | PCIField.SubClassCode | PCIField.ProgIF | PCIField.RevisionID,
					Factory = delegate { return new IntelHSUART(); }
				},

				new PCIDeviceDriverRegistryEntry()
				{
					Name = "PCIIDEInterface",
					Platforms = PlatformArchitecture.X86AndX64,
					BusType = DeviceBusType.PCI,
					VendorID = 0x8086,
					DeviceID = 0x7010,
					ClassCode = 0X01,
					SubClassCode = 0x01,
					ProgIF = 0x80,
					PCIFields = PCIField.VendorID | PCIField.DeviceID | PCIField.ClassCode | PCIField.SubClassCode | PCIField.ProgIF,
					Factory = delegate { return new PCIIDEInterface(); }
				},
			};
		}
	}
}
