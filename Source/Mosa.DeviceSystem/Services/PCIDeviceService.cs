// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Mosa.DeviceSystem.PCI;

namespace Mosa.DeviceSystem.Services;

/// <summary>
/// Properly initializes and starts the dummy PCI devices created by the <see cref="PCIControllerService"/>.
/// </summary>
public class PCIDeviceService : BaseService
{
	private DeviceService deviceService;

	protected override void Initialize()
	{
		deviceService = ServiceManager.GetFirstService<DeviceService>();
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void PostEvent(ServiceEvent serviceEvent)
	{
		//HAL.DebugWriteLine("PCIDeviceService:PostEvent()");
		//HAL.Pause();

		var device = MatchEvent<PCIDevice>(serviceEvent, ServiceEventType.Start);
		if (device == null)
			return;

		var pciDevice = device.DeviceDriver as PCIDevice;

		// Find the best matching driver
		PCIDeviceDriverRegistryEntry matchedDriver = null;
		var matchPriority = 0;

		// Start PCI Drivers
		var drivers = deviceService.GetDeviceDrivers(DeviceBusType.PCI);

		foreach (var driver in drivers)
		{
			if (driver is not PCIDeviceDriverRegistryEntry pciDriver)
				continue;

			if (!IsMatch(pciDriver, pciDevice))
				continue;

			var priority = GetMatchedPriority(pciDriver);

			if (priority <= 0)
				continue;

			if (priority >= matchPriority && matchPriority != 0)
				continue;

			matchedDriver = pciDriver;
			matchPriority = priority;
		}

		if (matchedDriver == null) // No driver found
			return;

		StartDevice(matchedDriver, device, pciDevice);
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

		//foreach (var ioportregion in ioPortRegions)
		//{
		//	HAL.DebugWriteLine("  I/O: 0x" + ioportregion.BaseIOPort.ToString("X") + " [" + ioportregion.Size.ToString("X") + "]");
		//}
		//foreach (var memoryregion in memoryRegions)
		//{
		//	HAL.DebugWriteLine("  Memory: 0x" + memoryregion.BaseAddress.ToString("X") + " [" + memoryregion.Size.ToString("X") + "]");
		//}

		var hardwareResources = new HardwareResources(ioPortRegions, memoryRegions, pciDevice.IRQ);
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
