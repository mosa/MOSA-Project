using Mosa.DeviceSystem;
using Mosa.DeviceSystem.Network;
using Mosa.Runtime;
using System.Net;

namespace Mosa.DeviceDriver.PCI.Realtek
{
	public class RTL8139 : BaseDeviceDriver, INetworkDevice
	{
		private byte[] TSAD, TSD;

		private ushort CurrentPointer = 0;
		private int TXIndex = 0;

		private ConstrainedPointer BAR0, RX;

		public override void Initialize()
		{
			Device.Name = "RTL8139";

			TSAD = new byte[] { 0x20, 0x24, 0x28, 0x2C };
			TSD = new byte[] { 0x10, 0x14, 0x18, 0x1C };

			HAL.DebugWriteLine("RTL8139 found!");

			Network.Initialise(IPAddress.Parse(192, 168, 137, 188), IPAddress.Parse(192, 168, 137, 1));

			BAR0 = Device.Resources.GetMemory(0);
			BAR0.Write8(0x52, 0);
			BAR0.Write8(0x37, 0x10);
			while ((BAR0.Read8(0x37) & 0x10) != 0) ;

			HAL.DebugWriteLine("Soft reset done!");

			RX = HAL.AllocateMemory(8192 + 16 + 1500 + 0xF, ~0xFU);

			BAR0.Write32(0x30, (uint)RX.Address);

			BAR0.Write16(0x3C, 0x5);
			BAR0.Write32(0x44, 0xF | (1 << 7));
			BAR0.Write8(0x37, 0xC);

			HAL.DebugWriteLine("Config done!");

			Network.MAC = new byte[6];
			Network.MAC[0] = BAR0.Read8(0);
			Network.MAC[1] = BAR0.Read8(1);
			Network.MAC[2] = BAR0.Read8(2);
			Network.MAC[3] = BAR0.Read8(3);
			Network.MAC[4] = BAR0.Read8(4);
			Network.MAC[5] = BAR0.Read8(5);

			ARP.Require(this, Network.Gateway);

			HAL.DebugWriteLine("RTL8139 initialized!");
		}

		public override unsafe bool OnInterrupt()
		{
			HAL.DebugWriteLine("Interrupt!");

			var status = BAR0.Read16(0x3E);

			if ((status & (1 << 2)) != 0)
				HAL.DebugWriteLine("Transmit OK!");

			if ((status & (1 << 0)) != 0)
			{
				HAL.DebugWriteLine("Receive OK!");

				var t = (ushort*)(RX.Address + CurrentPointer);
				var length = *(t + 1);
				t += 2;

				Ethernet.HandlePacket(this, (byte*)t, length);

				CurrentPointer = (ushort)((CurrentPointer + length + 4 + 3) & ~3);
				BAR0.Write16(0x38, (ushort)(CurrentPointer - 0x10));
			}

			BAR0.Write16(0x3E, 0x05);

			return true;
		}

		void INetworkDevice.SendPacket(Pointer data, uint length)
		{
			BAR0.Write32(TSAD[TXIndex], (uint)data);
			BAR0.Write32(TSD[TXIndex++], length);

			if (TXIndex > 3)
				TXIndex = 0;
		}
	}
}
