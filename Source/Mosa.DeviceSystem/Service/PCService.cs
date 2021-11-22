// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.DeviceSystem.PCI;

namespace Mosa.DeviceSystem.Service
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

		private IACPI ACPI;

		/// <summary>
		/// Initializes this instance.
		/// </summary>
		public override void Initialize()
		{
			DeviceService = ServiceManager.GetFirstService<DeviceService>();
			ACPI = DeviceService.GetFirstDevice<IACPI>(DeviceStatus.Online).DeviceDriver as IACPI;
		}

		public bool Reset()
		{
			if (ACPI == null)
				return false;

			BaseIOPortWrite address = ACPI.ResetAddress;
			byte value = ACPI.ResetValue;

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
			if (ACPI == null)
				return false;

			ACPI.PM1aControlBlock.Write16((ushort)(ACPI.SLP_TYPa | ACPI.SLP_EN));
			if (ACPI.PM1bControlBlock != null)
				ACPI.PM1bControlBlock.Write16((ushort)(ACPI.SLP_TYPb | ACPI.SLP_EN));

			return true;
		}
	}
}
