// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.PCI
{
	/// <summary>
	/// Generic PCI Host Bridge Controller
	/// </summary>
	//[PCIDeviceDriver(ClassCode = 0x06, SubClassCode = 0x00, Platforms = PlatformArchitecture.X86AndX64)]
	public class PCIGenericHostBridgeController : BaseDeviceDriver, IHostBridgeController
	{
		protected byte ResetAddress { get; set; }
		protected byte ResetValue { get; set; }

		public override void Initialize()
		{
			Device.Name = "PCIGenericHostBridgeController";
		}

		// TODO: We have to test
		bool IHostBridgeController.CPUReset()
		{
			var pciDevice = Device.Parent.DeviceDriver as PCIDevice;

			if (pciDevice == null)
				return false;

			var pciController = Device.Parent.Parent.DeviceDriver as IPCIControllerLegacy;

			if (pciController == null)
				return false;

			pciController.WriteConfig8(pciDevice.Bus, (byte)((ResetAddress >> 32) & 0xFFFF), (byte)((ResetAddress >> 16) & 0xFFFF), (byte)(ResetAddress & 0xFFFF), ResetValue);

			return false;
		}

		void IHostBridgeController.SetCPUResetInformation(byte address, byte value)
		{
			ResetAddress = address;
			ResetValue = value;
		}
	}
}
