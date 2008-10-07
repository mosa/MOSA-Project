/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;

namespace System.IO
{
	/// Abstract implementation of the "IO.Stream" class
	[Serializable]
	public abstract class Stream : IDisposable // MarshalByRefObject, 
	{
		//public static readonly Stream Null = new NullStream ();

		protected Stream()
		{
		}

		public abstract bool CanRead
		{
			get;
		}

		public abstract bool CanSeek
		{
			get;
		}

		public abstract bool CanWrite
		{
			get;
		}

		public virtual bool CanTimeout
		{
			get
			{
				return false;
			}
		}

		public abstract long Length
		{
			get;
		}

		public abstract long Position
		{
			get;
			set;
		}

		public abstract void Flush();

		public abstract int Read(byte[] buffer, int offset, int count);

		public abstract int ReadByte();

		public abstract long Seek(long offset, SeekOrigin origin);

		public abstract void SetLength(long value);

		public abstract void Write(byte[] buffer, int offset, int count);

		public abstract void WriteByte(byte value);

		//		delegate void WriteDelegate (byte[] buffer, int offset, int count);

		public virtual int ReadTimeout
		{
			get
			{
				throw new InvalidOperationException("Timeouts are not supported on this stream.");
			}
			set
			{
				throw new InvalidOperationException("Timeouts are not supported on this stream.");
			}
		}

		public virtual int WriteTimeout
		{
			get
			{
				throw new InvalidOperationException("Timeouts are not supported on this stream.");
			}
			set
			{
				throw new InvalidOperationException("Timeouts are not supported on this stream.");
			}
		}

		public static Stream Synchronized(Stream stream)
		{
			throw new NotSupportedException();
		}


		public virtual IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback cback, object state)
		{
			throw new NotSupportedException("This stream does not support async");
		}

		public virtual IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback cback, object state)
		{
			throw new NotSupportedException("This stream does not support async");
		}

		public virtual int EndRead(IAsyncResult async_result)
		{
			throw new NotSupportedException("This stream does not support async");
		}

		public virtual void EndWrite(IAsyncResult async_result)
		{
			throw new NotSupportedException("This stream does not support async");
		}

		public void Dispose()
		{
			Close();
		}

		protected virtual void Dispose(bool disposing)
		{
		}

		public virtual void Close()
		{
			Dispose(true);
		}

		void IDisposable.Dispose()
		{
			Close();
		}
	}

	public class AsyncResult
	{
	}

	public class IAsyncResult
	{
	}

	public class AsyncCallback
	{
	}
}
