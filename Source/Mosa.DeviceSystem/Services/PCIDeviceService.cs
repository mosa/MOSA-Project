// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using Mosa.DeviceSystem.Framework;
using Mosa.DeviceSystem.Framework.PCI;
using Mosa.DeviceSystem.HardwareAbstraction;
using Mosa.DeviceSystem.PCI;

namespace Mosa.DeviceSystem.Services;

/// <summary>
/// Initializes and starts all PCI devices in the system.
/// </summary>
public class PCIDeviceService : BaseService
{
	private DeviceService deviceService;

	protected override void Initialize()
	{
		HAL.DebugWriteLine("PCIDeviceService::Initialize()");

		deviceService = ServiceManager.GetFirstService<DeviceService>();

		var controllerDevice = deviceService.GetFirstDevice<IPCIController>();
		if (controllerDevice?.DeviceDriver is not IPCIController pciController)
			return;

		for (byte bus = 0; bus < byte.MaxValue; bus++)
			for (byte slot = 0; slot < 16; slot++)
				for (byte function = 0; function < 7; function++)
					CreateDevice(bus, slot, function, controllerDevice, pciController);

		HAL.DebugWriteLine("PCIDeviceService::Initialize() [Exit]");
	}

	private void CreateDevice(byte bus, byte slot, byte function, Device pciControllerDevice, IPCIController pciController)
	{
		var configuration = new PCIDeviceConfiguration(pciControllerDevice.Name, pciController, bus, slot, function);
		var value = pciController.ReadConfig32(configuration, 0);

		if (value == 0xFFFFFFFF)
			return;

		// TODO: Check for duplicates

		configuration.Initialize();

		// Find the best matching driver
		PCIDeviceDriverRegistryEntry matchedDriver = null;
		var matchPriority = 0;

		var drivers = deviceService.GetDeviceDrivers(DeviceBusType.PCI);
		foreach (var driver in drivers)
		{
			if (driver is not PCIDeviceDriverRegistryEntry pciDriver || !IsMatch(pciDriver, configuration))
				continue;

			var priority = GetMatchedPriority(pciDriver);
			if (priority <= 0 || (priority >= matchPriority && matchPriority != 0))
				continue;

			matchedDriver = pciDriver;
			matchPriority = priority;
		}

		StartDevice(matchedDriver, pciControllerDevice, configuration);
	}

	private void StartDevice(PCIDeviceDriverRegistryEntry driver, Device parentDevice, PCIDeviceConfiguration configuration)
	{
		var ioPortRegions = new List<IOPortRegion>();
		var memoryRegions = new List<AddressRegion>();

		foreach (var baseAddress in configuration.BaseAddresses)
		{
			if (baseAddress == null || baseAddress.Size == 0)
				continue;

			switch (baseAddress.Region)
			{
				case AddressType.PortIO: ioPortRegions.Add(new IOPortRegion((ushort)baseAddress.Address, (ushort)baseAddress.Size)); break;
				case AddressType.Memory: memoryRegions.Add(new AddressRegion(baseAddress.Address, baseAddress.Size)); break;
			}
		}

		var hardwareResources = new HardwareResources(ioPortRegions, memoryRegions, configuration.IRQ);

		// No driver was found previously
		if (driver == null)
		{
			HAL.DebugWriteLine(" > Unknown PCI Device: ");
			HAL.DebugWriteLine(configuration.VendorID.ToString("x") + ":" + configuration.DeviceID.ToString("x"));

			// It must be set to auto start, or else the device isn't registered in the framework
			deviceService.Initialize(null, parentDevice, true, configuration, hardwareResources, DeviceBusType.PCI);
			return;
		}

		HAL.DebugWriteLine(" > PCI Device: ");
		HAL.DebugWriteLine(driver.Name);

		configuration.EnableDevice();
		deviceService.Initialize(driver, parentDevice, driver.AutoStart, configuration, hardwareResources, DeviceBusType.PCI);
	}

	private static bool HasFlag(PCIField list, PCIField match) => (int)(list & match) != 0;

	private static bool IsMatch(PCIDeviceDriverRegistryEntry driver, PCIDeviceConfiguration pciDeviceConfiguration)
	{
		if (HasFlag(driver.PCIFields, PCIField.VendorID) && driver.VendorID != pciDeviceConfiguration.VendorID)
			return false;

		if (HasFlag(driver.PCIFields, PCIField.DeviceID) && driver.DeviceID != pciDeviceConfiguration.DeviceID)
			return false;

		if (HasFlag(driver.PCIFields, PCIField.SubSystemID) && driver.SubSystemID != pciDeviceConfiguration.SubSystemID)
			return false;

		if (HasFlag(driver.PCIFields, PCIField.SubSystemVendorID) && driver.SubSystemVendorID != pciDeviceConfiguration.SubSystemVendorID)
			return false;

		if (HasFlag(driver.PCIFields, PCIField.ClassCode) && driver.ClassCode != pciDeviceConfiguration.ClassCode)
			return false;

		if (HasFlag(driver.PCIFields, PCIField.SubClassCode) && driver.SubClassCode != pciDeviceConfiguration.SubClassCode)
			return false;

		if (HasFlag(driver.PCIFields, PCIField.ProgIF) && driver.ProgIF != pciDeviceConfiguration.ProgIF)
			return false;

		if (HasFlag(driver.PCIFields, PCIField.RevisionID) && driver.RevisionID != pciDeviceConfiguration.RevisionID)
			return false;

		return true;
	}

	private static int GetMatchedPriority(PCIDeviceDriverRegistryEntry driver)
	{
		var vendorId = HasFlag(driver.PCIFields, PCIField.VendorID);
		var deviceId = HasFlag(driver.PCIFields, PCIField.DeviceID);
		var subSystemId = HasFlag(driver.PCIFields, PCIField.SubSystemID);
		var subSystemVendorId = HasFlag(driver.PCIFields, PCIField.SubSystemVendorID);
		var classCode = HasFlag(driver.PCIFields, PCIField.ClassCode);
		var subClassCode = HasFlag(driver.PCIFields, PCIField.SubClassCode);
		var progIf = HasFlag(driver.PCIFields, PCIField.ProgIF);
		var revisionId = HasFlag(driver.PCIFields, PCIField.RevisionID);

		switch (vendorId)
		{
			case true when deviceId && classCode && subClassCode && progIf && revisionId: return 1;
			case true when deviceId && classCode && subClassCode && progIf: return 2;
			case true when deviceId && subSystemVendorId && subSystemId && revisionId: return 3;
			case true when deviceId && subSystemVendorId && subSystemId: return 4;
			case true when deviceId && revisionId: return 5;
			case true when deviceId: return 6;
			default:
				{
					switch (classCode)
					{
						case true when subClassCode && progIf && revisionId: return 7;
						case true when subClassCode && progIf: return 8;
						case true when subClassCode && revisionId: return 9;
						case true when subClassCode: return 10;
					}
					break;
				}
		}

		return 0;
	}
}
