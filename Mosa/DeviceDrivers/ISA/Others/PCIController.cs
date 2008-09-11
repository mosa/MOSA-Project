/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceDrivers;
using Mosa.DeviceDrivers.Kernel;
using Mosa.DeviceDrivers.PCI;
using Mosa.ClassLib;

namespace Mosa.DeviceDrivers.ISA
{
    /// <summary>
    /// 
    /// </summary>
	[ISADeviceSignature(AutoLoad = true, BasePort = 0x0CF8, PortRange = 8, Platforms = PlatformArchitecture.Both_x86_and_x64)]
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
        /// 
        /// </summary>
		public PCIController() { }

        /// <summary>
        /// 
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
        /// 
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
        /// 
        /// </summary>
        /// <returns></returns>
		public override bool Start() { return true; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public override LinkedList<IDevice> CreateSubDevices()
		{
			LinkedList<IDevice> devices = new LinkedList<IDevice>();

			for (byte bus = 0; bus <= 255; bus++) {
				for (byte slot = 0; slot < 16; slot++) {
					for (byte fun = 0; fun < 7; fun++) {
						if (ProbeDevice(bus, slot, fun)) {
							devices.Add(new PCIDevice(this, bus, slot, fun));
						}
					}
				}
			}

			return devices;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public override bool OnInterrupt() { return false; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bus"></param>
        /// <param name="slot"></param>
        /// <param name="function"></param>
        /// <param name="register"></param>
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
        /// 
        /// </summary>
        /// <param name="bus"></param>
        /// <param name="slot"></param>
        /// <param name="function"></param>
        /// <param name="register"></param>
        /// <param name="value"></param>
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
        /// 
        /// </summary>
        /// <param name="bus"></param>
        /// <param name="slot"></param>
        /// <param name="fun"></param>
        /// <returns></returns>
		public bool ProbeDevice(byte bus, byte slot, byte fun)
		{
			return (ReadConfig(bus, slot, fun, 0) != 0xFFFFFFFF);
		}

	}
}
