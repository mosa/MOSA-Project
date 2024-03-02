// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.Framework;
using Mosa.DeviceSystem.HardwareAbstraction;
using Mosa.DeviceSystem.PCI;

namespace Mosa.DeviceSystem.Services;

/// <summary>
/// Manages power-related tasks via ACPI.
/// </summary>
public class PCService : BaseService
{
	private DeviceService deviceService;
	private IACPI ACPI;

	protected override void Initialize() => deviceService = ServiceManager.GetFirstService<DeviceService>();

	public bool Reset()
	{
		var device = deviceService.GetFirstDevice<IACPI>(DeviceStatus.Online);
		if (device == null)
			return false;

		ACPI ??= device.DeviceDriver as IACPI;
		if (ACPI == null)
			return false;

		var address = ACPI.ResetAddress;
		if (address.Address == 0)
			return false;

		// 1st method
		var value = ACPI.ResetValue;
		address.Write8(value);

		// 2nd method: write to PCI Configuration Space (we're actually writing on the host bridge controller)
		// TODO: Fix
		var controller = deviceService.GetFirstDevice<IHostBridgeController>(DeviceStatus.Online).DeviceDriver as IHostBridgeController;
		if (controller == null)
			return false;

		controller.SetCPUResetInformation((byte)address.Address, value);
		return controller.CPUReset();
	}

	public bool Shutdown()
	{
		var device = deviceService.GetFirstDevice<IACPI>(DeviceStatus.Online);
		if (device == null)
			return false;

		ACPI ??= device.DeviceDriver as IACPI;
		if (ACPI == null)
			return false;

		ACPI.PM1aControlBlock.Write16((ushort)(ACPI.SLP_TYPa | ACPI.SLP_EN));
		if (ACPI.PM1bControlBlock.Address != 0)
			ACPI.PM1bControlBlock.Write16((ushort)(ACPI.SLP_TYPb | ACPI.SLP_EN));

		return true;
	}
}
