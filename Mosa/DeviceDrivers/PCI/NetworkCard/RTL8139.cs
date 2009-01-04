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

namespace Mosa.DeviceDrivers.PCI.VideoCard
{
	/// <summary>
	/// RTL8139 Network Chip
	/// </summary>
	[PCIDeviceDriver(VendorID = 0x10EC, DeviceID = 0x8139, Platforms = PlatformArchitecture.Both_x86_and_x64)]
	[DeviceDriverMemory(MemorySize = 4 * 1024, MemoryAlignment = 1)]
	[DeviceDriverMemory(MemorySize = 4 * 1024, MemoryAlignment = 1)]
	[DeviceDriverMemory(MemorySize = 4 * 1024, MemoryAlignment = 1)]
	[DeviceDriverMemory(MemorySize = 4 * 1024, MemoryAlignment = 1)]
	public class RTL8139 : HardwareDevice
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RTL8139"/> class.
		/// </summary>
		public RTL8139() { }

		/// <summary>
		/// Setups this hardware device driver
		/// </summary>
		/// <returns></returns>
		public override bool Setup(IHardwareResources hardwareResources)
		{
			this.hardwareResources = hardwareResources;
			base.name = "RTL8139_0x" + hardwareResources.GetIOPortRegion(0).BaseIOPort.ToString("X");

			return true;
		}

		/// <summary>
		/// Starts this hardware device.
		/// </summary>
		/// <returns></returns>
		public override DeviceDriverStartStatus Start()
		{
			return DeviceDriverStartStatus.Started;
		}

		/// <summary>
		/// Called when an interrupt is received.
		/// </summary>
		/// <returns></returns>
		public override bool OnInterrupt() { return true; }

	}
}
