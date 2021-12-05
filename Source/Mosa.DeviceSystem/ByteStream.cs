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

		public byte Read8(uint offset)
		{
			return data[offset];
		}

		public ushort Read16(uint offset)
		{
			return (ushort)(data[offset] | data[offset + 1] << 8);
		}

		public uint Read24(uint offset)
		{
			return (uint)(data[offset] | data[offset + 1] << 8 | data[offset + 2] << 16);
		}

		public uint Read32(uint offset)
		{
			return (uint)(data[offset] | data[offset + 1] << 8 | data[offset + 2] << 16 | data[offset + 3] << 24);
		}
	}
}
