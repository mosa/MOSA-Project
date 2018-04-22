// Copyright (c) MOSA Project. Licensed under the New BSD License.

#pragma warning disable 169, 414

namespace System.IO
{
	public class MemoryStream : Stream
	{
		private byte[] internalBuffer;
		private int initialIndex;
		private int position;
		private int count;
		private bool writable;
		private bool publiclyVisible;
		private bool streamClosed;

		public MemoryStream(byte[] buffer, int index, int count, bool writable, bool publiclyVisible)
		{
			internalBuffer = buffer;
			initialIndex = index;
			position = index;
			this.count = count;
			this.writable = writable;
			this.publiclyVisible = publiclyVisible;
		}

		public override bool CanRead
		{
			get { return !streamClosed; }
		}

		public override bool CanSeek
		{
			get { return !streamClosed; }
		}

		public override bool CanWrite
		{
			get { return (!streamClosed && writable); }
		}

		public override long Length
		{
			get { throw new NotImplementedException(); }
		}

		public override long Position
		{
			get
			{
				return position;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public override void Flush()
		{
			throw new NotImplementedException();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		public override int ReadByte()
		{
			throw new NotImplementedException();
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		public override void WriteByte(byte value)
		{
			throw new NotImplementedException();
		}
	}
}
