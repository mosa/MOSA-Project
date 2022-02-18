// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.DeviceSystem.PCI;

namespace Mosa.DeviceSystem.Service
{
	/// <summary>
	/// PC Service
	/// </summary>
	// TODO: Fix
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
		}

		public bool Reset()
		{
			if (ACPI == null)
			{
				ACPI = DeviceService.GetFirstDevice<IACPI>(DeviceStatus.Online).DeviceDriver as IACPI;

				if (ACPI == null)
					return false;
			}

			var address = ACPI.ResetAddress;
			var value = ACPI.ResetValue;

			if (address == null)
				return false;

			address.Write8(value);

			// Map memory
			var ptr = HAL.GetPhysicalMemory((Pointer)(uint)address.Address, 1).Address;
			var mappedResetReg = HAL.GetWriteIOPort((ushort)ptr);

			mappedResetReg.Write8(value);

			// Write to PCI Configuration Space (we're actually writing on the host bridge controller)
			// TODO: Fix
			var controller = DeviceService.GetFirstDevice<IHostBridgeController>(DeviceStatus.Online)
				.DeviceDriver as IHostBridgeController;
			if (controller == null)
				return false;

			controller.SetCPUResetInformation((byte)address.Address, value);
			controller.CPUReset();

			return true;
		}

		public bool Shutdown()
		{
			if (ACPI == null)
			{
				ACPI = DeviceService.GetFirstDevice<IACPI>(DeviceStatus.Online).DeviceDriver as IACPI;

				if (ACPI == null)
					return false;
			}

			ACPI.PM1aControlBlock.Write16((ushort)(ACPI.SLP_TYPa | ACPI.SLP_EN));
			if (ACPI.PM1bControlBlock != null)
				ACPI.PM1bControlBlock.Write16((ushort)(ACPI.SLP_TYPb | ACPI.SLP_EN));

			return true;
		}
	}
}
