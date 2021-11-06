// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	public class ByteStream
	{
		private byte[] data;

		public ByteStream(byte[] data)
		{
			this.data = data;
		}

		public byte Read8(int offset)
		{
			return data[offset];
		}

		public int Read16(int offset)
		{
			return data[offset] | data[offset + 1] << 8;
		}

		public int Read32(int offset)
		{
			return data[offset] | data[offset + 1] << 8 | data[offset + 2] << 16 | data[offset + 3] << 24;
		}
	}
}
