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

namespace Mosa.DeviceDrivers.PCI
{

	[PCIDeviceSignature(VendorID = 0xABCD, DeviceID = 0x1000)]
	[PCIDeviceSignature(VendorID = 0xABCD, DeviceID = 0x2000)]
	[PCIDeviceSignature(ClassCode = 0xFFFF, SubClassCode = 0xFF)]
	public class TestDriver : PCIHardwareDevice, IHardwareDevice
	{
		protected IReadWriteIOPort TestPort;
		protected SpinLock spinLock;

		public TestDriver(PCIDevice pciDevice) : base(pciDevice) { }
		public void Dispose() { }

		public override bool Setup()
		{
			base.name = "TEST_0x" + busResources.GetIOPortRegion(0).BaseIOPort.ToString("X");

			TestPort = busResources.GetIOPortRegion(0).GetIOPort(0);

			return true;
		}

		public override bool Probe() { return true; }
		public override bool Start() { return true; }
		public override bool OnInterrupt() { return false; }
		public override LinkedList<IDevice> CreateSubDevices() { return null; }

	}
}
