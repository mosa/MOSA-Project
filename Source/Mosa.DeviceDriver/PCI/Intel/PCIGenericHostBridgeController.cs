// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;
using Mosa.DeviceSystem.PCI;

namespace Mosa.DeviceDriver.PCI.Intel
{
	/// <summary>
	/// Generic PCI Host Bridge Controller
	/// </summary>
	//[PCIDeviceDriver(ClassCode = 0x06, SubClassCode = 0x00, Platforms = PlatformArchitecture.X86AndX64)]
	public class PCIGenericHostBridgeController : BaseDeviceDriver, IHostBridgeController
	{
		protected int ResetAddress { get; set; }
		protected int ResetValue { get; set; }

		public override void Initialize()
		{
			Device.Name = "PCIGenericHostBridgeController";
		}

		bool IHostBridgeController.CPUReset()
		{
			return false;
		}

		void IHostBridgeController.SetCPUResetInformation(int address, int value)
		{
			ResetAddress = address;
			ResetValue = value;
		}
	}
}
