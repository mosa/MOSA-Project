// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.PCI;
using Mosa.Runtime;

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// PC Service
	/// </summary>
	public class PCService : BaseService
	{
		/// <summary>
		/// The device service
		/// </summary>
		protected DeviceService DeviceService;

		/// <summary>
		/// Initializes this instance.
		/// </summary>
		public override void Initialize()
		{
			DeviceService = ServiceManager.GetFirstService<DeviceService>();
		}

		public bool Reset()
		{
			var acpi = DeviceService.GetFirstDevice<IACPI>(DeviceStatus.Online).DeviceDriver as IACPI;

			if (acpi == null)
				return false;

			BaseIOPortWrite address = acpi.ResetAddress;
			byte value = acpi.ResetValue;

			if (address != null)
			{
				address.Write8(value);

				// Map memory
				Pointer ptr = HAL.GetPhysicalMemory((Pointer)(uint)address.Address, 1).Address;
				BaseIOPortWrite mappedResetReg = HAL.GetWriteIOPort((ushort)ptr);

				mappedResetReg.Write8(value);

				// Write to PCI Configuration Space (we're actually writing on the host bridge controller)
				// TODO: Fix
				var controller = DeviceService.GetFirstDevice<IHostBridgeController>(DeviceStatus.Online).DeviceDriver as IHostBridgeController;

				controller.SetCPUResetInformation((byte)address.Address, value);
				controller.CPUReset();

				return true;
			}

			return false;
		}

		public bool Shutdown()
		{
			var acpi = DeviceService.GetFirstDevice<IACPI>(DeviceStatus.Online).DeviceDriver as IACPI;

			if (acpi == null)
				return false;

			acpi.PM1aControlBlock.Write16((ushort)(acpi.SLP_TYPa | acpi.SLP_EN));
			if (acpi.PM1bControlBlock != null)
				acpi.PM1bControlBlock.Write16((ushort)(acpi.SLP_TYPb | acpi.SLP_EN));

			return true;
		}
	}
}
