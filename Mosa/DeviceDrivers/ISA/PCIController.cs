/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceSystem;
using Mosa.DeviceSystem.ISA;
using PCI = Mosa.DeviceSystem.PCI;
using Mosa.ClassLib;

namespace Mosa.DeviceDrivers.ISA
{
    /// <summary>
    /// 
    /// </summary>
	[DeviceSignature(AutoLoad = true, BasePort = 0x0CF8, PortRange = 8, Platforms = PlatformArchitecture.Both_x86_and_x64)]
	public class PCIController : ISAHardwareDevice, IDevice, IHardwareDevice, IPCIController
	{

		#region Definitions

		private static readonly uint BaseValue = 0x80000000;

		#endregion

        /// <summary>
        /// 
        /// </summary>
		protected SpinLock spinLock;

        /// <summary>
        /// 
        /// </summary>
		protected IReadWriteIOPort configAddress;

        /// <summary>
        /// 
        /// </summary>
		protected IReadWriteIOPort configData;

		/// <summary>
		/// Initializes a new instance of the <see cref="PCIController"/> class.
		/// </summary>
		public PCIController() { }

		/// <summary>
		/// Setups this hardware device driver
		/// </summary>
		/// <returns></returns>
		public override bool Setup()
		{
			base.name = "PCI_0x" + base.busResources.GetIOPort(0, 0).Address.ToString("X");

			configAddress = base.busResources.GetIOPort(0, 0);
			configData = base.busResources.GetIOPort(0, 4);

			return true;
		}

		/// <summary>
		/// Probes for this device.
		/// </summary>
		/// <returns></returns>
		public override bool Probe()
		{
			configAddress.Write32(BaseValue);

			if (configAddress.Read32() != BaseValue)
				return false;

			return true;
		}

		/// <summary>
		/// Starts this hardware device.
		/// </summary>
		/// <returns></returns>
		public override bool Start() { return true; }

		/// <summary>
		/// Creates the sub devices.
		/// </summary>
		/// <returns></returns>
		public override LinkedList<IDevice> CreateSubDevices()
		{
			LinkedList<IDevice> devices = new LinkedList<IDevice>();

			for (byte bus = 0; bus < 255; bus++) {
				for (byte slot = 0; slot < 16; slot++) {
					for (byte fun = 0; fun < 7; fun++) {
						if (ProbeDevice(bus, slot, fun)) {
							devices.Add(new Mosa.DeviceSystem.PCI.PCIDevice(this, bus, slot, fun));
						}
					}
				}
			}

			return devices;
		}

		/// <summary>
		/// Called when an interrupt is received.
		/// </summary>
		/// <returns></returns>
		public override bool OnInterrupt() { return false; }

		/// <summary>
		/// Reads from configuraton space
		/// </summary>
		/// <param name="bus">The bus.</param>
		/// <param name="slot">The slot.</param>
		/// <param name="function">The function.</param>
		/// <param name="register">The register.</param>
		/// <returns></returns>
		public uint ReadConfig(byte bus, byte slot, byte function, byte register)
		{
			configAddress.Write32((uint)(BaseValue
					   | (uint)((bus & 0xFF) << 16)
                       | (uint)((slot & 0x0F) << 11)
                       | (uint)((function & 0x07) << 8)
                       | (uint)(register & 0xFC)));

			return configData.Read32();
		}

		/// <summary>
		/// Writes to configuraton space
		/// </summary>
		/// <param name="bus">The bus.</param>
		/// <param name="slot">The slot.</param>
		/// <param name="function">The function.</param>
		/// <param name="register">The register.</param>
		/// <param name="value">The value.</param>
		public void WriteConfig(byte bus, byte slot, byte function, byte register, uint value)
		{
			configAddress.Write32((uint)(BaseValue
                       | (uint)((bus & 0xFF) << 16)
                       | (uint)((slot & 0x0F) << 11)
                       | (uint)((function & 0x07) << 8)
                       | (uint)(register & 0xFC)));

			configData.Write32(value);
		}

		/// <summary>
		/// Probes the device.
		/// </summary>
		/// <param name="bus">The bus.</param>
		/// <param name="slot">The slot.</param>
		/// <param name="fun">The fun.</param>
		/// <returns></returns>
		public bool ProbeDevice(byte bus, byte slot, byte fun)
		{
			return (ReadConfig(bus, slot, fun, 0) != 0xFFFFFFFF);
		}

	}
}
