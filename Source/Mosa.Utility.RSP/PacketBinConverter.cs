// Copyright (c) MOSA Project. Licensed under the New BSD License.

// Author:
//       Benito Palacios Sánchez <benito356@gmail.com>
//
// Copyright (c) 2016 Benito Palacios Sánchez
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Text;

namespace Mosa.Utility.RSP
{
	public static class PacketBinConverter
	{
		private static readonly Encoding TextEncoding = Encoding.ASCII;

		private const string Prefix = "$";
		private const string Suffix = "#";

		public static byte[] ToBinary(CommandPacket packet)
		{
			string dataText = packet.Pack;
			byte[] dataBin = TextEncoding.GetBytes(dataText);

			StringBuilder binPacket = new StringBuilder();
			binPacket.Append(PacketBinConverter.Prefix);
			binPacket.Append(dataText);
			binPacket.Append(PacketBinConverter.Suffix);
			binPacket.Append(Checksum.Calculate(dataBin).ToString("x2"));

			return TextEncoding.GetBytes(binPacket.ToString());
		}

		//public static ReplyPacket FromBinary(byte[] data, CommandPacket commandSent = null)
		//{
		//	if (!ValidateBinary(data))
		//		throw new FormatException("Invalid packet");

		//	string packet = TextEncoding.GetString(data);
		//	string packetData = packet.Substring(1, packet.Length - 4);
		//	packetData = Rle.Decode(packetData);

		//	// Compare checksum
		//	string receivedChecksum = packet.Substring(packet.Length - 2, 2);
		//	uint calculatedChecksum = Checksum.Calculate(packetData);
		//	if (receivedChecksum != calculatedChecksum.ToString("x2"))
		//		throw new FormatException("Invalid checksum");

		//	return ReplyPacketFactory.CreateReplyPacket(packetData, commandSent);
		//}

		private static bool ValidateBinary(byte[] data)
		{
			if (data.Length < 4)
				return false;

			if (data[0] != Prefix[0])
				return false;

			if (data[data.Length - 3] != Suffix[0])
				return false;

			return true;
		}
	}
}
