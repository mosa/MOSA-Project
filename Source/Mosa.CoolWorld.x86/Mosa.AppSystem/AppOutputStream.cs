// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.IO;
using Mosa.Kernel.x86;

namespace Mosa.AppSystem
{
	/// <summary>
	///
	/// </summary>
	public class AppOutputStream : Stream
	{
		public ConsoleSession Session { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="AppOutputStream"/> class.
		/// </summary>
		/// <param name="session">The session.</param>
		public AppOutputStream(ConsoleSession session)
		{
			Session = session;
		}

		#region Overrides

		/// <summary>
		/// </summary>
		public override bool CanRead { get { return false; } }

		/// <summary>
		/// </summary>
		public override bool CanSeek { get { return false; } }

		/// <summary>
		/// </summary>
		public override bool CanWrite { get { return true; } }

		/// <summary>
		/// </summary>
		public override bool CanTimeout { get { return false; } }

		/// <summary>
		/// </summary>
		public override long Length { get { return 0; } }

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
			return -1;
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
		{
			for (int i = 0; i < count; i++)
			{
				WriteByte(buffer[offset + i]);
			}
		}

		/// <summary>
		/// Writes the byte.
		/// </summary>
		/// <param name="value">The value.</param>
		public override void WriteByte(byte value)
		{
			if (value == 10)
			{
				Session.WriteLine();
				return;
			}
			else if (value == 12)
			{
				Session.Clear();
				return;
			}
			else if (value == 8)
			{
				if (Session.Column > 0)
				{
					Session.Column--;
					Session.Write(' '); // bug: doesn't work on last column
					Session.Column--;
				}
			}
			else
			{
				Session.Write((char)value);
			}
		}

		#endregion Overrides
	}
}
