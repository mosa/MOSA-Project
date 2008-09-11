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
	[ISADeviceSignature(AutoLoad = true, BasePort = 0x0CF8, PortRange = 8, Platforms = PlatformArchitecture.Both_x86_and_x64)]
	public class PCIController : ISAHardwareDevice, IDevice, IHardwareDevice, IPCIController
	{

		#region Definitions

		private static readonly uint BaseValue = 0x80000000;

		#endregion

		protected SpinLock spinLock;

		protected IReadWriteIOPort configAddress;
		protected IReadWriteIOPort configData;

		public PCIController() { }

		public override bool Setup()
		{
			base.name = "PCI_0x" + base.busResources.GetIOPort(0, 0).Address.ToString("X");

			configAddress = base.busResources.GetIOPort(0, 0);
			configData = base.busResources.GetIOPort(0, 4);

			return true;
		}

		public override bool Probe()
		{
			configAddress.Write32(BaseValue);

			if (configAddress.Read32() != BaseValue)
				return false;

			return true;
		}

		public override bool Start() { return true; }

		public override LinkedList<IDevice> CreateSubDevices()
		{
			LinkedList<IDevice> devices = new LinkedList<IDevice>();

			for (byte bus = 0; bus < 256; bus++) {
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

		public override bool OnInterrupt() { return false; }

		public uint ReadConfig(byte bus, byte slot, byte function, byte register)
		{
			configAddress.Write32((uint)(BaseValue
					   | ((bus & 0xFF) << 16)
					   | ((slot & 0x0F) << 11)
					   | ((function & 0x07) << 8)
					   | (register & 0xFC)));

			return configData.Read32();
		}

		public void WriteConfig(byte bus, byte slot, byte function, byte register, uint value)
		{
			configAddress.Write32((uint)(BaseValue
					   | ((bus & 0xFF) << 16)
					   | ((slot & 0x0F) << 11)
					   | ((function & 0x07) << 8)
					   | (register & 0xFC)));

			configData.Write32(value);
		}

		public bool ProbeDevice(byte bus, byte slot, byte fun)
		{
			return (ReadConfig(bus, slot, fun, 0) != 0xFFFFFFFF);
		}

	}
}
