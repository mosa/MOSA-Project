namespace Mosa.DeviceSystem
{
	public class MemoryStream
	{
		private byte[] Array;

		private int Position;

		public MemoryStream(byte[] source)
		{
			Array = source;
			Position = 0;
		}

		public void Read(byte[] buffer, int offset, int length)
		{
			for (var i = 0; i < length; i++)
				buffer[i] = Array[Position + i + offset];

			Position += length;
		}

		public byte[] ReadEnd()
		{
			var length = Array.Length - Position;
			var buffer = new byte[length];

			for (var i = 0; i < length; i++)
				buffer[i] = Array[Position + i];

			Position += length;
			return buffer;
		}

		public byte ReadByte()
		{
			var b = Array[Position];
			Position++;

			return b;
		}
	}
}
