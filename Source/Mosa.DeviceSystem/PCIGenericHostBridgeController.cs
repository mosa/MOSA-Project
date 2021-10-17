// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;
using Mosa.DeviceSystem.PCI;

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

		bool IHostBridgeController.CPUReset()
		{
			
			
			return true;
		}

		void IHostBridgeController.SetCPUResetInformation(byte address, byte value)
		{
			ResetAddress = address;
			ResetValue = value;
		}
	}
}
