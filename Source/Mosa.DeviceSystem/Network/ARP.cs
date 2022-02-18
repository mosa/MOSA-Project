using Mosa.Runtime;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Mosa.DeviceSystem.Network
{
	public static unsafe class ARP
	{
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct ARPHeader
		{
			public ushort HardwareType;
			public ushort Protocol;
			public byte HardwareAddrLen;
			public byte ProtocolAddrLen;
			public ushort Operation;
			public fixed byte SourceMAC[6];
			public fixed byte SourceIP[4];
			public fixed byte DestMAC[6];
			public fixed byte DestIP[4];
		}

		public enum ARPOperation
		{
			Request = 1,
			Reply = 2
		}

		public struct ARPEntry
		{
			public byte[] IP;
			public byte[] MAC;
		}

		public static List<ARPEntry> ARPEntries;

		public static void Initialise()
		{
			ARPEntries = new List<ARPEntry>(32);
		}

		internal static void HandlePacket(INetworkDevice device, byte* data, int length)
		{
			ARPHeader* hdr = (ARPHeader*)data;

			if (Ethernet.SwapLeftRight(hdr->Operation) == (ushort)ARPOperation.Reply)
			{
				byte[] IP = new byte[4];
				IP[0] = hdr->SourceIP[0];
				IP[1] = hdr->SourceIP[1];
				IP[2] = hdr->SourceIP[2];
				IP[3] = hdr->SourceIP[3];
				byte[] MAC = new byte[6];
				MAC[0] = hdr->SourceMAC[0];
				MAC[1] = hdr->SourceMAC[1];
				MAC[2] = hdr->SourceMAC[2];
				MAC[3] = hdr->SourceMAC[3];
				MAC[4] = hdr->SourceMAC[4];
				MAC[5] = hdr->SourceMAC[5];
				ARPEntries.Add(new ARPEntry() { IP = IP, MAC = MAC });
				HAL.DebugWriteLine("IP handled!");
			}
			if (
				hdr->DestIP[0] == Network.IP[0] &&
				hdr->DestIP[1] == Network.IP[1] &&
				hdr->DestIP[2] == Network.IP[2] &&
				hdr->DestIP[3] == Network.IP[3]
				)
			{
				if (Ethernet.SwapLeftRight(hdr->Operation) == (ushort)ARPOperation.Request)
				{
					var ptr = HAL.AllocateMemory((uint)sizeof(ARPHeader), 0).Address;
					Internal.MemoryCopy(ptr, (Pointer)hdr, (uint)sizeof(ARPHeader));

					var _hdr = (ARPHeader*)ptr;

					_hdr->Operation = Ethernet.SwapLeftRight((uint)ARPOperation.Reply);
					_hdr->DestMAC[0] = _hdr->SourceMAC[0];
					_hdr->DestMAC[1] = _hdr->SourceMAC[1];
					_hdr->DestMAC[2] = _hdr->SourceMAC[2];
					_hdr->DestMAC[3] = _hdr->SourceMAC[3];
					_hdr->DestMAC[4] = _hdr->SourceMAC[4];
					_hdr->DestMAC[5] = _hdr->SourceMAC[5];
					_hdr->DestIP[0] = _hdr->SourceIP[0];
					_hdr->DestIP[1] = _hdr->SourceIP[1];
					_hdr->DestIP[2] = _hdr->SourceIP[2];
					_hdr->DestIP[3] = _hdr->SourceIP[3];
					_hdr->SourceMAC[0] = Network.MAC[0];
					_hdr->SourceMAC[1] = Network.MAC[1];
					_hdr->SourceMAC[2] = Network.MAC[2];
					_hdr->SourceMAC[3] = Network.MAC[3];
					_hdr->SourceMAC[4] = Network.MAC[4];
					_hdr->SourceMAC[5] = Network.MAC[5];
					_hdr->SourceIP[0] = Network.IP[0];
					_hdr->SourceIP[1] = Network.IP[1];
					_hdr->SourceIP[2] = Network.IP[2];
					_hdr->SourceIP[3] = Network.IP[3];
					byte[] DestMAC = new byte[6];
					DestMAC[0] = _hdr->DestMAC[0];
					DestMAC[1] = _hdr->DestMAC[1];
					DestMAC[2] = _hdr->DestMAC[2];
					DestMAC[3] = _hdr->DestMAC[3];
					DestMAC[4] = _hdr->DestMAC[4];
					DestMAC[5] = _hdr->DestMAC[5];
					Ethernet.SendPacket(device, DestMAC, (ushort)Ethernet.EthernetType.ARP, _hdr, sizeof(ARPHeader));
				}
			}
		}

		internal static byte[] Lookup(INetworkDevice device, byte[] destIP)
		{
			while (true)
			{
				for (int i = 0; i < ARPEntries.Count; i++)
				{
					if (
						ARPEntries[i].IP[0] == destIP[0] &&
						ARPEntries[i].IP[1] == destIP[1] &&
						ARPEntries[i].IP[2] == destIP[2] &&
						ARPEntries[i].IP[3] == destIP[3]
						) return ARPEntries[i].MAC;
				}
				HAL.DebugWriteLine("ARP entry not found! Making requests");
				Require(device, destIP);
			}
		}

		public static void Require(INetworkDevice device, byte[] IP)
		{
			ARPHeader* hdr = (ARPHeader*)HAL.AllocateMemory((uint)sizeof(ARPHeader), 0).Address;
			hdr->SourceMAC[0] = Network.MAC[0];
			hdr->SourceMAC[1] = Network.MAC[1];
			hdr->SourceMAC[2] = Network.MAC[2];
			hdr->SourceMAC[3] = Network.MAC[3];
			hdr->SourceMAC[4] = Network.MAC[4];
			hdr->SourceMAC[5] = Network.MAC[5];
			hdr->DestMAC[0] = Network.Broadcast[0];
			hdr->DestMAC[1] = Network.Broadcast[1];
			hdr->DestMAC[2] = Network.Broadcast[2];
			hdr->DestMAC[3] = Network.Broadcast[3];
			hdr->DestMAC[4] = Network.Broadcast[4];
			hdr->DestMAC[5] = Network.Broadcast[5];
			hdr->SourceIP[0] = Network.IP[0];
			hdr->SourceIP[1] = Network.IP[1];
			hdr->SourceIP[2] = Network.IP[2];
			hdr->SourceIP[3] = Network.IP[3];
			hdr->DestIP[0] = IP[0];
			hdr->DestIP[1] = IP[1];
			hdr->DestIP[2] = IP[2];
			hdr->DestIP[3] = IP[3];
			hdr->Operation = Ethernet.SwapLeftRight((uint)ARPOperation.Request);
			hdr->HardwareAddrLen = 6;
			hdr->ProtocolAddrLen = 4;
			hdr->HardwareType = Ethernet.SwapLeftRight(1);
			hdr->Protocol = Ethernet.SwapLeftRight((uint)Ethernet.EthernetType.IPv4);
			Ethernet.SendPacket(device, Network.Broadcast, (ushort)Ethernet.EthernetType.ARP, hdr, sizeof(ARPHeader));
		}
	}
}
