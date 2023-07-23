// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;
using Mosa.DeviceSystem.PCI;

namespace Mosa.DeviceDriver.PCI;

/// <summary>
/// Generic PCI Host Bridge Controller
/// </summary>
//[PCIDeviceDriver(ClassCode = 0x06, SubClassCode = 0x00, Platforms = PlatformArchitecture.X86AndX64)]
public class GenericHostBridgeController : BaseDeviceDriver, IHostBridgeController
{
	private byte resetAddress;
	private byte resetValue;

	public override void Initialize()
	{
		Device.Name = "GenericHostBridgeController";
	}

	public override void Probe() => Device.Status = DeviceStatus.Available;

	public override void Start() => Device.Status = DeviceStatus.Online;

	public override void Stop() => Device.Status = DeviceStatus.Offline;

	// TODO: Fix
	public bool CPUReset()
	{
		var pciDevice = (PCIDevice)Device.Parent.DeviceDriver;
		var pciController = (IPCIControllerLegacy)Device.Parent.Parent.DeviceDriver;

		pciController.WriteConfig8(pciDevice.Bus,
			(byte)((resetAddress >> 32) & 0xFFFF),
			(byte)((resetAddress >> 16) & 0xFFFF),
			(byte)(resetAddress & 0xFFFF),
			resetValue);

		return false;
	}

	public void SetCPUResetInformation(byte address, byte value)
	{
		resetAddress = address;
		resetValue = value;
	}
}
