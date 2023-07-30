// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Mosa.DeviceSystem.PCI;

namespace Mosa.DeviceSystem.Service;

/// <summary>
/// PCI Controller Service
/// </summary>
public class PCIDeviceService : BaseService
{
	/// <summary>
	/// The device service
	/// </summary>
	protected DeviceService DeviceService;

	/// <summary>
	/// Initializes this instance.
	/// </summary>
	protected override void Initialize()
	{
		DeviceService = ServiceManager.GetFirstService<DeviceService>();
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
		int matchPriorty = 0;

		// Start ISA Drivers
		var drivers = DeviceService.GetDeviceDrivers(DeviceBusType.PCI);

		foreach (var driver in drivers)
		{
			if (!(driver is PCIDeviceDriverRegistryEntry pciDriver))
				continue;

			if (!IsMatch(pciDriver, pciDevice))
				continue;

			int priority = GetMatchedPriority(pciDriver, pciDevice);

			if (priority <= 0)
				continue;

			if (priority < matchPriorty || matchPriorty == 0)
			{
				matchedDriver = pciDriver;
				matchPriorty = priority;
			}
		}

		if (matchedDriver == null)
			return; // no driver found

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
				case AddressType.IO: ioPortRegions.Add(new IOPortRegion((ushort)pciBaseAddress.Address, (ushort)pciBaseAddress.Size)); break;
				case AddressType.Memory: memoryRegions.Add(new AddressRegion(pciBaseAddress.Address, pciBaseAddress.Size)); break;
				default: break;
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

		DeviceService.Initialize(driver, device, driver.AutoStart, null, hardwareResources);
	}

	private static bool HasFlag(PCIField list, PCIField match)
	{
		return (int)(list & match) != 0;
	}

	protected static bool IsMatch(PCIDeviceDriverRegistryEntry driver, PCIDevice pciDevice)
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

	protected static int GetMatchedPriority(PCIDeviceDriverRegistryEntry driver, PCIDevice pciDevice)
	{
		bool VendorID = HasFlag(driver.PCIFields, PCIField.VendorID);
		bool DeviceID = HasFlag(driver.PCIFields, PCIField.DeviceID);
		bool SubSystemID = HasFlag(driver.PCIFields, PCIField.SubSystemID);
		bool SubSystemVendorID = HasFlag(driver.PCIFields, PCIField.SubSystemVendorID);
		bool ClassCode = HasFlag(driver.PCIFields, PCIField.ClassCode);
		bool SubClassCode = HasFlag(driver.PCIFields, PCIField.SubClassCode);
		bool ProgIF = HasFlag(driver.PCIFields, PCIField.ProgIF);
		bool RevisionID = HasFlag(driver.PCIFields, PCIField.RevisionID);

		if (VendorID && DeviceID && ClassCode && SubClassCode && ProgIF && RevisionID)
			return 1;
		else if (VendorID && DeviceID && ClassCode && SubClassCode && ProgIF)
			return 2;
		else if (VendorID && DeviceID && SubSystemVendorID && SubSystemID && RevisionID)
			return 3;
		else if (VendorID && DeviceID && SubSystemVendorID && SubSystemID)
			return 4;
		else if (VendorID && DeviceID && RevisionID)
			return 5;
		else if (VendorID && DeviceID)
			return 6;
		else if (ClassCode && SubClassCode && ProgIF && RevisionID)
			return 7;
		else if (ClassCode && SubClassCode && ProgIF)
			return 8;
		else if (ClassCode && SubClassCode && RevisionID)
			return 9;
		else if (ClassCode && SubClassCode)
			return 10;

		return 0;
	}
}
