/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceDrivers;
using Mosa.ClassLib;
using Mosa.DeviceDrivers.PCI;
using Mosa.DeviceDrivers.Kernel;

namespace Mosa.DeviceDrivers.ISA
{
	[ISADeviceSignature(AutoLoad = true, BasePort = 0x0070, PortRange = 2)]
	public class CMOSDriver : ISAHardwareDevice, IDevice, IHardwareDevice
	{

		protected IReadWriteIOPort commandPort;
		protected IReadWriteIOPort dataPort;
		protected SpinLock spinLock;

		public CMOSDriver() { }
		public void Dispose() { }

		public override bool Setup()
		{
			base.name = "CMOS";

			commandPort = base.busResources.GetIOPortRegion(0).GetPort(0);
			dataPort = base.busResources.GetIOPortRegion(0).GetPort(4);

			return true;
		}

		public override bool Start() { return true; }
		public override bool Probe() { return true; }
		public override LinkedList<IDevice> CreateSubDevices() { return null; }
		public override bool OnInterrupt() { return true; }

		public byte Read(byte address)
		{
			spinLock.Enter();
			commandPort.Write8(address);
			byte b = dataPort.Read8();
			spinLock.Exit();
			return b;
		}
	}
}
