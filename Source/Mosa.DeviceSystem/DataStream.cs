// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	public class DataStream
	{
		private byte[] Array;

		private int Position;

		public DataStream(byte[] source)
		{
			Array = source;
			Position = 0;
		}

		public byte[] ReadBytes(int offset, int length)
		{
			byte[] buffer = new byte[length];

			for (var i = 0; i < length; i++)
				buffer[i] = Array[Position + i + offset];

			Skip(length);
			return buffer;
		}

		public byte[] ReadEnd()
		{
			var length = Array.Length - Position;
			var buffer = new byte[length];

			for (var i = 0; i < length; i++)
				buffer[i] = Array[Position + i];

			Skip(length);
			return buffer;
		}

		public byte ReadByte()
		{
			var b = Array[Position];
			Skip(1);

			return b;
		}

		public int ReadInt(bool bigEndian = true)
		{
			var i = Array[Position] << 24 | Array[Position + 1] << 16 | Array[Position + 2] << 8 | Array[Position + 3];
			Skip(4);

			return i;
		}

		public void Skip(int pos)
		{
			Position += pos;
		}
	}
}
