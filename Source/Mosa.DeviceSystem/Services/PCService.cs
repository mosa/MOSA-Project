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

		var value = ACPI.ResetValue;
		address.Write8(value);
		return true;
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
