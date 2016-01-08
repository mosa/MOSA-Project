// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.IO;
using System.Collections.Generic;

namespace Mosa.AppSystem
{
	/// <summary>
	///
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

		/// <summary>
		///
		/// </summary>
		public override bool CanRead { get { return true; } }

		/// <summary>
		/// </summary>
		public override bool CanSeek { get { return false; } }

		/// <summary>
		/// </summary>
		public override bool CanWrite { get { return false; } }

		/// <summary>
		/// </summary>
		public override bool CanTimeout { get { return false; } }

		/// <summary>
		/// </summary>
		public override long Length { get { return length; } }

		/// <summary>
		/// </summary>
		public override long Position { get { return 0; } set { return; } }

		/// <summary>
		/// </summary>
		public override void Flush()
		{ }

		/// <summary>
		/// Reads the specified buffer.
		/// </summary>
		/// <param name="buffer">The buffer.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="count">The count.</param>
		/// <returns></returns>
		public override int Read(byte[] buffer, int offset, int count)
		{
			return -1;
		}

		/// <summary>
		/// </summary>
		/// <returns></returns>
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

		/// <summary>
		/// Seeks the specified offset.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <param name="origin">The origin.</param>
		/// <returns></returns>
		public override long Seek(long offset, SeekOrigin origin)
		{
			return Position;
		}

		/// <summary>
		/// Sets the length.
		/// </summary>
		/// <param name="value">The value.</param>
		public override void SetLength(long value)
		{ }

		/// <summary>
		/// Writes the specified buffer.
		/// </summary>
		/// <param name="buffer">The buffer.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="count">The count.</param>
		public override void Write(byte[] buffer, int offset, int count)
		{ }

		/// <summary>
		/// Writes the byte.
		/// </summary>
		/// <param name="value">The value.</param>
		public override void WriteByte(byte value)
		{ }

		#endregion Overrides

		/// <summary>
		/// Writes the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
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
