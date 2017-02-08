// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.ClassLib;
using System.Collections.Generic;

namespace Mosa.Utility.DebugEngine
{
	internal sealed class Packet
	{
		public List<byte> Data { get; internal set; }

		private uint crc;

		public Packet()
		{
			Data = new List<byte>(64);
			StartCRC();
		}

		public void Add(byte b)
		{
			Data.Add(b);
			crc = CRC.Update(crc, b);
		}

		public void Add(int i)
		{
			Add((byte)(i & 0xFF));
			Add((byte)(i >> 8 & 0xFF));
			Add((byte)(i >> 16 & 0xFF));
			Add((byte)(i >> 24 & 0xFF));
		}

		public void StartCRC()
		{
			crc = CRC.InitialCRC; // initialize the CRC (at the right spot)
		}

		public void AppendCRC()
		{
			Add((int)crc);
		}

		public void AppendPacket(Packet packet)
		{
			foreach (var b in packet.Data)
			{
				Add(b);
			}
		}
	}
}
