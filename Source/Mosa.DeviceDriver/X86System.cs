// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceDriver.ISA;
using Mosa.DeviceSystem;

namespace Mosa.DeviceDriver
{
	/// <summary>
	/// X86 System
	/// </summary>
	public class X86System : BaseDeviceDriver
	{
		protected SpinLock spinLock;

		protected override void Initialize()
		{
			Device.Name = "X86System";
			Device.Status = DeviceStatus.Available;
		}

		public override void Probe() => Device.Status = DeviceStatus.Available;

		public override void Start()
		{
			if (Device.Status != DeviceStatus.Available)
				return;

			Device.Status = DeviceStatus.Online;

			CreateISABusDevices();
		}

		public override bool OnInterrupt() => true;

		protected void CreateISABusDevices()
		{
			var deviceDriver = new ISABus();
			DeviceManager.Initialize(deviceDriver, Device, null, null, null);
			deviceDriver.Start();
		}
	}
}
