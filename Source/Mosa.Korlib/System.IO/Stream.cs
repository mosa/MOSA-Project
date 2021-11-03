﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.IO
{
	/// Abstract implementation of the "IO.Stream" class
	[Serializable]
	public abstract class Stream : IDisposable // MarshalByRefObject,
	{
		//public static readonly Stream Null = new NullStream ();

		/// <summary>
		///
		/// </summary>
		protected Stream()
		{
		}

		/// <summary>
		///
		/// </summary>
		public abstract bool CanRead
		{
			get;
		}

		/// <summary>
		///
		/// </summary>
		public abstract bool CanSeek
		{
			get;
		}

		/// <summary>
		///
		/// </summary>
		public abstract bool CanWrite
		{
			get;
		}

		/// <summary>
		///
		/// </summary>
		public virtual bool CanTimeout
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		///
		/// </summary>
		public abstract long Length
		{
			get;
		}

		/// <summary>
		///
		/// </summary>
		public abstract long Position
		{
			get;
			set;
		}

		/// <summary>
		///
		/// </summary>
		public abstract void Flush();

		/// <summary>
		///
		/// </summary>
		public abstract int Read(byte[] buffer, int offset, int count);

		/// <summary>
		///
		/// </summary>
		public abstract int ReadByte();

		/// <summary>
		///
		/// </summary>
		public abstract long Seek(long offset, SeekOrigin origin);

		/// <summary>
		///
		/// </summary>
		public abstract void SetLength(long value);

		/// <summary>
		///
		/// </summary>
		public abstract void Write(byte[] buffer, int offset, int count);

		/// <summary>
		///
		/// </summary>
		public abstract void WriteByte(byte value);

		//		delegate void WriteDelegate (byte[] buffer, int offset, int count);

		public virtual void CopyTo(Stream destination)
		{
			if (!CanRead && !destination.CanWrite)
				return;

			byte[] buffer = new byte[32768];
			
			int read;
			while ((read = Read(buffer, 0, buffer.Length)) > 0)
				destination.Write(buffer, 0, read);
		}

		/// <summary>
		///
		/// </summary>
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

		/// <summary>
		///
		/// </summary>
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

		/// <summary>
		///
		/// </summary>
		public static Stream Synchronized(Stream stream)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		///
		/// </summary>
		public virtual IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback cback, object state)
		{
			throw new NotSupportedException("This stream does not support async");
		}

		/// <summary>
		///
		/// </summary>
		public virtual IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback cback, object state)
		{
			throw new NotSupportedException("This stream does not support async");
		}

		/// <summary>
		///
		/// </summary>
		public virtual int EndRead(IAsyncResult async_result)
		{
			throw new NotSupportedException("This stream does not support async");
		}

		/// <summary>
		///
		/// </summary>
		public virtual void EndWrite(IAsyncResult async_result)
		{
			throw new NotSupportedException("This stream does not support async");
		}

		/// <summary>
		///
		/// </summary>
		public void Dispose()
		{
			Close();
		}

		/// <summary>
		///
		/// </summary>
		protected virtual void Dispose(bool disposing)
		{
		}

		/// <summary>
		///
		/// </summary>
		public virtual void Close()
		{
			Dispose(true);
		}

		/// <summary>
		///
		/// </summary>
		void IDisposable.Dispose()
		{
			Close();
		}
	}

	/// <summary>
	///
	/// </summary>
	public class AsyncResult
	{
	}

	/// <summary>
	///
	/// </summary>
	public class IAsyncResult
	{
	}

	/// <summary>
	///
	/// </summary>
	public class AsyncCallback
	{
	}
}
