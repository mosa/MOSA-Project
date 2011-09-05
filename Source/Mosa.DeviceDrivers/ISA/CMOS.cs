/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceSystem;

namespace Mosa.DeviceDrivers.ISA
{
	/// <summary>
	/// CMOS Device Driver
	/// </summary>
	[ISADeviceDriver(AutoLoad = true, BasePort = 0x0070, PortRange = 2, Platforms = PlatformArchitecture.X86)]
	public class CMOS : HardwareDevice, IDevice, IHardwareDevice
	{
		/// <summary>
		/// 
		/// </summary>
		protected IReadWriteIOPort commandPort;

		/// <summary>
		/// 
		/// </summary>
		protected IReadWriteIOPort dataPort;

		/// <summary>
		/// 
		/// </summary>
		protected SpinLock spinLock;

		/// <summary>
		/// 
		/// </summary>
		public CMOS() { }

		/// <summary>
		/// Setups this hardware device driver
		/// </summary>
		/// <returns></returns>
		public override bool Setup(IHardwareResources hardwareResources)
		{
			this.hardwareResources = hardwareResources;
			base.name = "CMOS";

			commandPort = base.hardwareResources.GetIOPort(0, 0);
			dataPort = base.hardwareResources.GetIOPort(0, 4);

			return true;
		}

		/// <summary>
		/// Starts this hardware device.
		/// </summary>
		/// <returns></returns>
		public override DeviceDriverStartStatus Start()
		{
			base.deviceStatus = DeviceStatus.Online;
			return DeviceDriverStartStatus.Started;
		}

		/// <summary>
		/// Called when an interrupt is received.
		/// </summary>
		/// <returns></returns>
		public override bool OnInterrupt()
		{
			return true;
		}

		/// <summary>
		/// Reads the specified address.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns></returns>
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
