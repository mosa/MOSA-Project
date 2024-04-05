// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

	protected override void Initialize() => deviceService = ServiceManager.GetFirstService<DeviceService>();

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void PostEvent(ServiceEvent serviceEvent)
	{
		var device = MatchEvent<IPCIController>(serviceEvent, ServiceEventType.Start);
		if (device == null)
			return;

		var pciController = device.DeviceDriver as IPCIController;
		for (byte bus = 0; bus < byte.MaxValue; bus++)
			for (byte slot = 0; slot < 16; slot++)
				for (byte function = 0; function < 7; function++)
					CreatePCIDevice(bus, slot, function, device, pciController);
	}

	private void CreatePCIDevice(byte bus, byte slot, byte function, Device device, IPCIController pciController)
	{
		var pciDevice = new PCIDevice(bus, slot, function);
		var value = pciController.ReadConfig32(pciDevice, 0);

		if (value == 0xFFFFFFFF)
			return;

		// TODO: Check for duplicates

		var parentDevice = deviceService.Initialize(pciDevice, device);

		// Find the best matching driver
		PCIDeviceDriverRegistryEntry matchedDriver = null;
		var matchPriority = 0;

		var drivers = deviceService.GetDeviceDrivers(DeviceBusType.PCI);
		foreach (var driver in drivers)
		{
			if (driver is not PCIDeviceDriverRegistryEntry pciDriver || !IsMatch(pciDriver, pciDevice))
				continue;

			var priority = GetMatchedPriority(pciDriver);
			if (priority <= 0 || (priority >= matchPriority && matchPriority != 0))
				continue;

			matchedDriver = pciDriver;
			matchPriority = priority;
		}

		// No driver found
		if (matchedDriver == null)
			return;

		StartDevice(matchedDriver, parentDevice, pciDevice);
	}

	private void StartDevice(PCIDeviceDriverRegistryEntry driver, Device device, PCIDevice pciDevice)
	{
		var ioPortRegions = new List<IOPortRegion>();
		var memoryRegions = new List<AddressRegion>();

		foreach (var pciBaseAddress in pciDevice.BaseAddresses)
		{
			if (pciBaseAddress == null || pciBaseAddress.Size == 0)
				continue;

			switch (pciBaseAddress.Region)
			{
				case AddressType.PortIO: ioPortRegions.Add(new IOPortRegion((ushort)pciBaseAddress.Address, (ushort)pciBaseAddress.Size)); break;
				case AddressType.Memory: memoryRegions.Add(new AddressRegion(pciBaseAddress.Address, pciBaseAddress.Size)); break;
			}
		}

		var hardwareResources = new HardwareResources(ioPortRegions, memoryRegions, pciDevice.IRQ);

		HAL.DebugWriteLine(" > PCI Driver: ");
		HAL.DebugWriteLine(driver.Name);

		deviceService.Initialize(driver, device, driver.AutoStart, null, hardwareResources);
	}

	private static bool HasFlag(PCIField list, PCIField match) => (int)(list & match) != 0;

	private static bool IsMatch(PCIDeviceDriverRegistryEntry driver, PCIDevice pciDevice)
	{
		if (HasFlag(driver.PCIFields, PCIField.VendorID) && driver.VendorID != pciDevice.VendorID)
			return false;

		if (HasFlag(driver.PCIFields, PCIField.DeviceID) && driver.DeviceID != pciDevice.DeviceID)
			return false;

		if (HasFlag(driver.PCIFields, PCIField.SubSystemID) && driver.SubSystemID != pciDevice.SubSystemID)
			return false;

		if (HasFlag(driver.PCIFields, PCIField.SubSystemVendorID) && driver.SubSystemVendorID != pciDevice.SubSystemVendorID)
			return false;

		if (HasFlag(driver.PCIFields, PCIField.ClassCode) && driver.ClassCode != pciDevice.ClassCode)
			return false;

		if (HasFlag(driver.PCIFields, PCIField.SubClassCode) && driver.SubClassCode != pciDevice.SubClassCode)
			return false;

		if (HasFlag(driver.PCIFields, PCIField.ProgIF) && driver.ProgIF != pciDevice.ProgIF)
			return false;

		if (HasFlag(driver.PCIFields, PCIField.RevisionID) && driver.RevisionID != pciDevice.RevisionID)
			return false;

		return true;
	}

	private static int GetMatchedPriority(PCIDeviceDriverRegistryEntry driver)
	{
		var vendorID = HasFlag(driver.PCIFields, PCIField.VendorID);
		var deviceID = HasFlag(driver.PCIFields, PCIField.DeviceID);
		var subSystemID = HasFlag(driver.PCIFields, PCIField.SubSystemID);
		var subSystemVendorID = HasFlag(driver.PCIFields, PCIField.SubSystemVendorID);
		var classCode = HasFlag(driver.PCIFields, PCIField.ClassCode);
		var subClassCode = HasFlag(driver.PCIFields, PCIField.SubClassCode);
		var progIF = HasFlag(driver.PCIFields, PCIField.ProgIF);
		var revisionID = HasFlag(driver.PCIFields, PCIField.RevisionID);

		switch (vendorID)
		{
			case true when deviceID && classCode && subClassCode && progIF && revisionID: return 1;
			case true when deviceID && classCode && subClassCode && progIF: return 2;
			case true when deviceID && subSystemVendorID && subSystemID && revisionID: return 3;
			case true when deviceID && subSystemVendorID && subSystemID: return 4;
			case true when deviceID && revisionID: return 5;
			case true when deviceID: return 6;
			default:
				{
					switch (classCode)
					{
						case true when subClassCode && progIF && revisionID: return 7;
						case true when subClassCode && progIF: return 8;
						case true when subClassCode && revisionID: return 9;
						case true when subClassCode: return 10;
					}
					break;
				}
		}

		return 0;
	}
}
