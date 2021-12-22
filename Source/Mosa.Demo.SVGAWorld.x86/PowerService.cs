// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.DeviceSystem;
using Mosa.DeviceSystem.PCI;
using Mosa.DeviceDriver.ISA;
using Mosa.Runtime.x86;

namespace Mosa.Demo.VBEWorld.x86
{
	/// <summary>
	/// Power Service
	/// </summary>
	public static class PowerService
	{
		private static ACPI ACPI;

		/// <summary>
		/// Initializes this instance.
		/// </summary>
		public static void Initialize()
		{
			ACPI = Boot.DeviceService.GetFirstDevice<ACPI>(DeviceStatus.Online).DeviceDriver as ACPI;
		}

		public static bool Reset()
		{
			/*if (ACPI == null)
				return false;

			var address = ACPI.ResetAddress;
			var value = ACPI.ResetValue;

			if (address == null)
				return false;

			address.Write8(value);

			// Map memory
			var ptr = Mosa.DeviceSystem.HAL.GetPhysicalMemory((Pointer)(uint)address.Address, 1).Address;
			var mappedResetReg = Mosa.DeviceSystem.HAL.GetWriteIOPort((ushort)ptr);

			mappedResetReg.Write8(value);

			// Write to PCI Configuration Space (we're actually writing on the host bridge controller)
			var controller = Boot.DeviceService.GetFirstDevice<PCIGenericHostBridgeController>(DeviceStatus.Online).DeviceDriver as PCIGenericHostBridgeController;
			if (controller == null)
				return false;

			controller.SetCPUResetInformation((byte)address.Address, value);
			controller.CPUReset();*/

			//Use PS2 Controller To Reboot
			while ((Native.In8(0x64) & 0x02) != 0) ;
			Native.Out8(0x64, 0xFE);
			Native.Hlt();

			return true;
		}

		public static bool Shutdown()
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
