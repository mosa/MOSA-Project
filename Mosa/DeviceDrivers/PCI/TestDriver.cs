/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.ClassLib;
using Mosa.DeviceSystem;
using Mosa.DeviceSystem.PCI;

namespace Mosa.DeviceDrivers.PCI
{
    /// <summary>
    /// 
    /// </summary>
	[PCIDeviceSignature(VendorID = 0xABCD, DeviceID = 0x1000, Platforms = PlatformArchitecture.Both_x86_and_x64)]
	[PCIDeviceSignature(VendorID = 0xABCD, DeviceID = 0x2000, Platforms = PlatformArchitecture.Both_x86_and_x64)]
	[PCIDeviceSignature(ClassCode = 0xFFFF, SubClassCode = 0xFF, Platforms = PlatformArchitecture.Both_x86_and_x64)]
	public class TestDriver : PCIHardwareDevice, IHardwareDevice
	{
        /// <summary>
        /// 
        /// </summary>
		protected IReadWriteIOPort TestPort;

        /// <summary>
        /// 
        /// </summary>
		protected SpinLock spinLock;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pciDevice"></param>
		public TestDriver(PCIDevice pciDevice) : base(pciDevice) { }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public override bool Setup()
		{
			base.name = "TEST_0x" + busResources.GetIOPortRegion(0).BaseIOPort.ToString("X");

			TestPort = busResources.GetIOPort(0,0);

			return true;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public override bool Probe() { return true; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public override bool Start() { return true; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public override bool OnInterrupt() { return false; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public override LinkedList<IDevice> CreateSubDevices() { return null; }
	}
}
