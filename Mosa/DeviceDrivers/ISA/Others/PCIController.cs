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
	[ISADeviceSignature(AutoLoad = true, BasePort = 0x0CF8, PortRange = 8)]
	public class PCIController : ISAHardwareDevice, IDevice, IHardwareDevice, IPCIController
	{

		#region Definitions

		private static readonly uint BaseValue = 0x80000000;

		#endregion

		protected SpinLock spinLock;

		protected IReadWriteIOPort ConfigAddress;
		protected IReadWriteIOPort ConfigData;

		protected ushort ioBase;

		public PCIController() { }
		public void Dispose() { }

		public override bool Setup()
		{
			base.name = "PCI_0x" + base.isaBusResources.GetIOPortRegion(0).GetPort(0).Address.ToString("X");

			ConfigAddress = base.isaBusResources.GetIOPortRegion(0).GetPort(0);
			ConfigData = base.isaBusResources.GetIOPortRegion(0).GetPort(4);

			return true;
		}

		public override bool Probe()
		{
			ConfigAddress.Write32(BaseValue);

			if (ConfigAddress.Read32() != BaseValue)
				return false;

			return true;
		}

		public override bool Start() { return true; }

		public override LinkedList<IDevice> CreateSubDevices()
		{
			LinkedList<IDevice> devices = new LinkedList<IDevice>();

			for (uint bus = 0; bus < 256; bus++) {
				for (uint slot = 0; slot < 16; slot++) {
					for (uint fun = 0; fun < 7; fun++) {
						if (ProbeDevice(bus, slot, fun)) {
							devices.Add(new PCIDevice(this, bus, slot, fun));
						}
					}
				}
			}

			return devices;
		}

		public override bool OnInterrupt() { return false; }

		public uint ReadConfig(uint bus, uint slot, uint function, uint register)
		{
			ConfigAddress.Write32(BaseValue
					   | ((bus & 0xFF) << 16)
					   | ((slot & 0x0F) << 11)
					   | ((function & 0x07) << 8)
					   | (register & 0xFC));

			return ConfigData.Read32();
		}

		public void WriteConfig(uint bus, uint slot, uint function, uint register, uint value)
		{

			ConfigAddress.Write32(BaseValue
					   | ((bus & 0xFF) << 16)
					   | ((slot & 0x0F) << 11)
					   | ((function & 0x07) << 8)
					   | (register & 0xFC));

			ConfigData.Write32(value);
		}

		public bool ProbeDevice(uint bus, uint slot, uint fun)
		{
			return (ReadConfig(bus, slot, fun, 0) != 0xFFFFFFFF);
		}

	}
}
