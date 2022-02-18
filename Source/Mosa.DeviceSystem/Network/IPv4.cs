﻿using Mosa.Runtime;
using System.Runtime.InteropServices;

namespace Mosa.DeviceSystem.Network
{
	public static unsafe class IPv4
	{
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public unsafe struct IPv4Header
		{
			public byte VersionAndIHL;
			public byte DSCPAndECN;
			public ushort TotalLength;
			public ushort ID;
			public ushort FlagAndFragmentOffset;
			public byte TimeToLive;
			public byte Protocol;
			public ushort HeaderChecksum;
			public fixed byte SourceIP[4];
			public fixed byte DestIP[4];
		}

		public enum IPv4Protocol
		{
			ICMP = 1,
			TCP = 6,
			UDP = 17,
		}

		internal static unsafe void HandlePacket(INetworkDevice device, byte* data, int length)
		{
			IPv4Header* hdr = (IPv4Header*)data;
			data += sizeof(IPv4Header);
			length -= sizeof(IPv4Header);

			if (
				hdr->DestIP[0] == Network.IP[0] &&
				hdr->DestIP[1] == Network.IP[1] &&
				hdr->DestIP[2] == Network.IP[2] &&
				hdr->DestIP[3] == Network.IP[3]
				)
			{
				if (hdr->Protocol == (byte)IPv4Protocol.ICMP)
				{
					if (data[0] == 8)
					{
						var ptr = HAL.AllocateMemory((uint)length, 0).Address;
						Internal.MemoryCopy(ptr, (Pointer)data, (uint)length);

						byte* p = (byte*)ptr;
						p[0] = 0;
						*(ushort*)(p + 2) = 0;
						*(ushort*)(p + 2) = CalculateChecksum(p, length);

						byte[] srcIP = new byte[]
						{
							hdr->SourceIP[0],
							hdr->SourceIP[1],
							hdr->SourceIP[2],
							hdr->SourceIP[3]
						};
						SendPacket(device, srcIP, 1, p, length);
					}
				}
				/*else if (hdr->Protocol == (byte)IPv4Protocol.UDP)
				{
					UDP.HandlePacket(data, length);
				}
				else if (hdr->Protocol == (byte)IPv4Protocol.TCP)
				{
					TCP.TcpRecv(data, length);
				}*/
			}
		}

		public static void SendPacket(INetworkDevice device, byte[] DestIP, byte Protocol, byte* Data, int Length)
		{
			var ptr = HAL.AllocateMemory((uint)(sizeof(IPv4Header) + Length), 0).Address;
			IPv4Header* hdr = (IPv4Header*)ptr;
			hdr->VersionAndIHL = 0x45;
			hdr->TotalLength = Ethernet.SwapLeftRight((uint)(sizeof(IPv4Header) + Length));
			hdr->TimeToLive = 255;
			hdr->Protocol = Protocol;
			hdr->SourceIP[0] = Network.IP[0];
			hdr->SourceIP[1] = Network.IP[1];
			hdr->SourceIP[2] = Network.IP[2];
			hdr->SourceIP[3] = Network.IP[3];
			hdr->DestIP[0] = DestIP[0];
			hdr->DestIP[1] = DestIP[1];
			hdr->DestIP[2] = DestIP[2];
			hdr->DestIP[3] = DestIP[3];
			hdr->HeaderChecksum = CalculateChecksum((byte*)hdr, sizeof(IPv4Header));
			Internal.MemoryCopy(ptr + sizeof(IPv4Header), (Pointer)Data, (uint)Length);
			byte[] MAC = ARP.Lookup(device, DestIP);
			Ethernet.SendPacket(device, MAC, (ushort)Ethernet.EthernetType.IPv4, hdr, sizeof(IPv4Header) + Length);
		}

		public static ushort CalculateChecksum(byte* addr, int count)
		{
			uint sum = 0;
			ushort* ptr = (ushort*)addr;

			while (count > 1)
			{
				sum += *ptr++;
				count -= 2;
			}

			if (count > 0)
				sum += *(byte*)ptr;

			while ((sum >> 16) != 0)
				sum = (sum & 0xffff) + (sum >> 16);

			return (ushort)~sum;
		}
	}
}
