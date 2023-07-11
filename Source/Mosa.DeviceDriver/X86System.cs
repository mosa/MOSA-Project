// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
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
		Debug.WriteLine("X86System:Start()");

		CreateISABusDevices();

		Device.Status = DeviceStatus.Online;

		Debug.WriteLine("X86System:Start() [Exit]");
	}

	public override bool OnInterrupt() => true;

	protected void CreateISABusDevices()
	{
		Debug.WriteLine("X86System:CreateISABusDevices()");

		DeviceService.Initialize(new ISABus(), Device, true, null, null, null);

		Debug.WriteLine("X86System:CreateISABusDevices() [Exit]");
	}
}
