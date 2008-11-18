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
	/// PCI Test Driver
	/// </summary>
	//[DeviceSignature(VendorID = 0xABCD, DeviceID = 0x1000, Platforms = PlatformArchitecture.Both_x86_and_x64)]
	//[DeviceSignature(VendorID = 0xABCD, DeviceID = 0x2000, Platforms = PlatformArchitecture.Both_x86_and_x64)]
	//[DeviceSignature(ClassCode = 0xFFFF, SubClassCode = 0xFF, Platforms = PlatformArchitecture.Both_x86_and_x64)]
	public class TestDriver : HardwareDevice, IHardwareDevice
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
		/// Initializes a new instance of the <see cref="TestDriver"/> class.
		/// </summary>
		/// <param name="pciDevice"></param>
		public TestDriver(PCIDevice pciDevice) /* : base(pciDevice) */ { }

		/// <summary>
		/// Setups this hardware device driver
		/// </summary>
		/// <returns></returns>
		public override bool Setup(IHardwareResources hardwareResources)
		{
			this.hardwareResources = hardwareResources;
			base.name = "TEST_0x" + hardwareResources.GetIOPortRegion(0).BaseIOPort.ToString("X");

			TestPort = hardwareResources.GetIOPort(0, 0);

			return true;
		}

		/// <summary>
		/// Starts this hardware device.
		/// </summary>
		/// <returns></returns>
		public override DeviceDriverStartStatus Start() { return DeviceDriverStartStatus.Failed; }

		/// <summary>
		/// Called when an interrupt is received.
		/// </summary>
		/// <returns></returns>
		public override bool OnInterrupt() { return false; }

		/// <summary>
		/// Creates the sub devices.
		/// </summary>
		/// <returns></returns>
		public override LinkedList<IDevice> CreateSubDevices() { return null; }
	}
}
