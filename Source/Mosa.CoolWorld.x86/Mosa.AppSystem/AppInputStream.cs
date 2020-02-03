// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.IO;

namespace Mosa.AppSystem
{
	/// <summary>
	/// AppInputStream
	/// </summary>
	public class AppInputStream : Stream
	{
		// todo: replace with specialized buffer class
		private byte[] buffer;

		private int position;
		private int length;

		/// <summary>
		/// Initializes a new instance of the <see cref="AppInputStream"/> class.
		/// </summary>
		public AppInputStream()
		{
			buffer = new byte[1024];
			Position = 0;
			length = 0;
		}

		#region Overrides

		public override bool CanRead { get { return true; } }

		public override bool CanSeek { get { return false; } }

		public override bool CanWrite { get { return false; } }

		public override bool CanTimeout { get { return false; } }

		public override long Length { get { return length; } }

		public override long Position { get { return 0; } set { return; } }

		public override void Flush()
		{ }

		public override int Read(byte[] buffer, int offset, int count)
		{
			return -1;
		}

		public override int ReadByte()
		{
			if (length == 0)
				return -1;

			int value = buffer[position];

			position--;

			if (position < 0)
				position = buffer.Length;

			length--;

			return value;
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return Position;
		}

		public override void SetLength(long value)
		{ }

		public override void Write(byte[] buffer, int offset, int count)
		{ }

		public override void WriteByte(byte value)
		{ }

		#endregion Overrides

		public void Write(byte value)
		{
			if (length == buffer.Length)
				return; // byte will be dropped

			position++;

			if (position == buffer.Length)
				position = 0;

			buffer[position] = value;

			length++;
		}
	}
}
