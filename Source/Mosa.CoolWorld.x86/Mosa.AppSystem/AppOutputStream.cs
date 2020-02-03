// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.x86;
using System.IO;

namespace Mosa.AppSystem
{
	/// <summary>
	/// AppOutputStream
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

		public override bool CanRead { get { return false; } }

		public override bool CanSeek { get { return false; } }

		public override bool CanWrite { get { return true; } }

		public override bool CanTimeout { get { return false; } }

		public override long Length { get { return 0; } }

		public override long Position { get { return 0; } set { return; } }

		public override void Flush()
		{ }

		public override int Read(byte[] buffer, int offset, int count)
		{
			return -1;
		}

		public override int ReadByte()
		{
			return -1;
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return Position;
		}

		public override void SetLength(long value)
		{ }

		public override void Write(byte[] buffer, int offset, int count)
		{
			for (int i = 0; i < count; i++)
			{
				WriteByte(buffer[offset + i]);
			}
		}

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
