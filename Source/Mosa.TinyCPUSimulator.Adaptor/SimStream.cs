/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.IO;

namespace Mosa.TinyCPUSimulator.Adaptor
{
	/// <summary>
	///
	/// </summary>
	public class SimStream : Stream
	{
		private SimCPU simCPU;
		private long position;
		private ulong offset;

		public SimStream(SimCPU simCPU, ulong offset)
		{
			this.simCPU = simCPU;
			this.offset = offset;
		}

		public override bool CanRead { get { return false; } }

		public override bool CanSeek { get { return true; } }

		public override bool CanTimeout { get { return false; } }

		public override bool CanWrite { get { return true; } }

		public override long Length { get { return 0; } }

		public override long Position { get { return position; } set { position = value; } }

		public override void Flush()
		{
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			for (int i = 0; i < count; i++)
				buffer[offset++] = (byte)ReadByte();

			return count;
		}

		public override int ReadByte()
		{
			return (int)simCPU.DirectRead8(offset + ((ulong)position++));
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			if (origin == SeekOrigin.Begin)
				position = offset;
			else if (origin == SeekOrigin.Current)
				position = position + offset;
			else
				throw new NotSupportedException();

			return position;
		}

		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			foreach (var b in buffer)
				WriteByte(b);
		}

		public override void WriteByte(byte value)
		{
			simCPU.Write8(offset + ((ulong)position++), value);
		}
	}
}