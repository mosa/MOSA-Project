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
	[PCIDeviceDriver(VendorID = 0xABCD, DeviceID = 0x1000, Platforms = PlatformArchitecture.X86AndX64)]
	[PCIDeviceDriver(VendorID = 0xABCD, DeviceID = 0x2000, Platforms = PlatformArchitecture.X86AndX64)]
	[PCIDeviceDriver(ClassCode = 0xFFFF, SubClassCode = 0xFF, Platforms = PlatformArchitecture.X86AndX64)]
	public class TestDriver : HardwareDevice, IHardwareDevice
	{
		/// <summary>
		/// 
		/// </summary>
		public class CommandRegister : IOPortRegister
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="CommandRegister"/> class.
			/// </summary>
			/// <param name="ioPort">The io port.</param>
			/// <param name="bits">The bits.</param>
			public CommandRegister(IReadWriteIOPort ioPort, byte bits) : base(ioPort, bits) { }

			/// <summary>
			/// Gets or sets a value indicating whether this <see cref="CommandRegister"/> is enabled.
			/// </summary>
			/// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
			public bool Enabled { get { return GetBit(0); } set { SetBit(0, value); } }

			/// <summary>
			/// Sets the drive.
			/// </summary>
			/// <value>The drive.</value>
			public byte Drive { get { return (byte)GetBits(1, 2); } set { SetBits(value, 1, 2); } }
		}

		/// <summary>
		/// 
		/// </summary>
		protected IReadWriteIOPort TestPort;

		/// <summary>
		/// 
		/// </summary>
		protected CommandRegister Command;

		/// <summary>
		/// 
		/// </summary>
		protected SpinLock spinLock;

		/// <summary>
		/// Initializes a new instance of the <see cref="TestDriver"/> class.
		/// </summary>
		public TestDriver() { }

		/// <summary>
		/// Setups this hardware device driver
		/// </summary>
		/// <returns></returns>
		public override bool Setup(IHardwareResources hardwareResources)
		{
			this.hardwareResources = hardwareResources;
			base.name = "TEST_" + hardwareResources.GetIOPortRegion(0).BaseIOPort.ToString("X");

			TestPort = hardwareResources.GetIOPort(0, 0);

			Command = new CommandRegister(TestPort, 8);

			Command.Enabled = true;

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
	}
}
