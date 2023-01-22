// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	public class DataStream
	{
		public DataBlock Block { get; }

		private uint position;

		public DataStream(byte[] source)
		{
			Block = new(source);

			position = 0;
		}

		public byte ReadByte() => Block.GetByte(position++);

		public char ReadChar() => Block.GetChar(position++);

		public byte[] ReadBytes(uint length)
		{
			var bytes = Block.GetBytes(position, length);
			position += length;
			return bytes;
		}

		public byte[] ReadEnd() => ReadBytes(Block.Length - position);
	}
}
