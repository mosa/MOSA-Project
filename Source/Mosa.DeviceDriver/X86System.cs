// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceDriver.ISA;
using Mosa.DeviceSystem;

namespace Mosa.DeviceDriver;

/// <summary>
/// X86 System
/// </summary>
public class X86System : BaseDeviceDriver
{
	public override void Initialize()
	{
		Device.Name = "X86System";
	}

	public override void Probe()
	{
		Device.Status = DeviceStatus.Available;
	}

	public override void Start()
	{
		HAL.DebugWriteLine("X86System:Start()");

		CreateISABusDevices();

		Device.Status = DeviceStatus.Online;

		HAL.DebugWriteLine("X86System:Start() [Exit]");
	}

	public override bool OnInterrupt() => true;

	protected void CreateISABusDevices()
	{
		HAL.DebugWriteLine("X86System:CreateISABusDevices()");

		DeviceService.Initialize(new ISABus(), Device, true, null, null, null);

		HAL.DebugWriteLine("X86System:CreateISABusDevices() [Exit]");
	}
}
