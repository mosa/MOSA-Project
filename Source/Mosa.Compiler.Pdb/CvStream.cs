// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.IO;

namespace Mosa.Compiler.Pdb
{
	/// <summary>
	/// Wraps a stream to prevent it from being disposed by a reader/writer.
	/// </summary>
	internal class CvStream : Stream
	{
		#region Data Members

		/// <summary>
		/// Holds the wrapped stream of this CvStream instance.
		/// </summary>
		private Stream stream;

		#endregion Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CvStream"/> class.
		/// </summary>
		/// <param name="stream">The stream to wrap.</param>
		public CvStream(Stream stream)
		{
			if (stream == null)
				throw new ArgumentNullException(@"stream");

			this.stream = stream;
		}

		#endregion Construction

		#region Stream Overrides

		/// <summary>
		/// When overridden in a derived class, gets a value indicating whether the current stream supports reading.
		/// </summary>
		/// <value></value>
		/// <returns>true if the stream supports reading; otherwise, false.
		/// </returns>
		public override bool CanRead
		{
			get
			{
				ThrowIfDisposed();
				return this.stream.CanRead;
			}
		}

		/// <summary>
		/// When overridden in a derived class, gets a value indicating whether the current stream supports seeking.
		/// </summary>
		/// <value></value>
		/// <returns>true if the stream supports seeking; otherwise, false.
		/// </returns>
		public override bool CanSeek
		{
			get
			{
				ThrowIfDisposed();
				return this.stream.CanSeek;
			}
		}

		/// <summary>
		/// When overridden in a derived class, gets a value indicating whether the current stream supports writing.
		/// </summary>
		/// <value></value>
		/// <returns>true if the stream supports writing; otherwise, false.
		/// </returns>
		public override bool CanWrite
		{
			get
			{
				ThrowIfDisposed();
				return this.stream.CanWrite;
			}
		}

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="T:System.IO.Stream"/> and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing == true)
			{
				this.stream = null;
			}

			base.Dispose(disposing);
		}

		/// <summary>
		/// When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to the underlying device.
		/// </summary>
		/// <exception cref="T:System.IO.IOException">
		/// An I/O error occurs.
		/// </exception>
		public override void Flush()
		{
			ThrowIfDisposed();
			this.stream.Flush();
		}

		/// <summary>
		/// When overridden in a derived class, gets the length in bytes of the stream.
		/// </summary>
		/// <value></value>
		/// <returns>
		/// A long value representing the length of the stream in bytes.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">
		/// A class derived from Stream does not support seeking.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// Methods were called after the stream was closed.
		/// </exception>
		public override long Length
		{
			get
			{
				ThrowIfDisposed();
				return this.stream.Length;
			}
		}

		/// <summary>
		/// When overridden in a derived class, gets or sets the position within the current stream.
		/// </summary>
		/// <value></value>
		/// <returns>
		/// The current position within the stream.
		/// </returns>
		/// <exception cref="T:System.IO.IOException">
		/// An I/O error occurs.
		/// </exception>
		/// <exception cref="T:System.NotSupportedException">
		/// The stream does not support seeking.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// Methods were called after the stream was closed.
		/// </exception>
		public override long Position
		{
			get
			{
				ThrowIfDisposed();
				return this.stream.Position;
			}
			set
			{
				ThrowIfDisposed();
				this.stream.Position = value;
			}
		}

		/// <summary>
		/// When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
		/// </summary>
		/// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between <paramref name="offset"/> and (<paramref name="offset"/> + <paramref name="count"/> - 1) replaced by the bytes read from the current source.</param>
		/// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin storing the data read from the current stream.</param>
		/// <param name="count">The maximum number of bytes to be read from the current stream.</param>
		/// <returns>
		/// The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.
		/// </returns>
		/// <exception cref="T:System.ArgumentException">
		/// The sum of <paramref name="offset"/> and <paramref name="count"/> is larger than the buffer length.
		/// </exception>
		/// <exception cref="T:System.ArgumentNullException">
		/// 	<paramref name="buffer"/> is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// 	<paramref name="offset"/> or <paramref name="count"/> is negative.
		/// </exception>
		/// <exception cref="T:System.IO.IOException">
		/// An I/O error occurs.
		/// </exception>
		/// <exception cref="T:System.NotSupportedException">
		/// The stream does not support reading.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// Methods were called after the stream was closed.
		/// </exception>
		public override int Read(byte[] buffer, int offset, int count)
		{
			ThrowIfDisposed();
			return this.stream.Read(buffer, offset, count);
		}

		/// <summary>
		/// When overridden in a derived class, sets the position within the current stream.
		/// </summary>
		/// <param name="offset">A byte offset relative to the <paramref name="origin"/> parameter.</param>
		/// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin"/> indicating the reference point used to obtain the new position.</param>
		/// <returns>
		/// The new position within the current stream.
		/// </returns>
		/// <exception cref="T:System.IO.IOException">
		/// An I/O error occurs.
		/// </exception>
		/// <exception cref="T:System.NotSupportedException">
		/// The stream does not support seeking, such as if the stream is constructed from a pipe or console output.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// Methods were called after the stream was closed.
		/// </exception>
		public override long Seek(long offset, SeekOrigin origin)
		{
			ThrowIfDisposed();
			return this.stream.Seek(offset, origin);
		}

		/// <summary>
		/// When overridden in a derived class, sets the length of the current stream.
		/// </summary>
		/// <param name="value">The desired length of the current stream in bytes.</param>
		/// <exception cref="T:System.IO.IOException">
		/// An I/O error occurs.
		/// </exception>
		/// <exception cref="T:System.NotSupportedException">
		/// The stream does not support both writing and seeking, such as if the stream is constructed from a pipe or console output.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// Methods were called after the stream was closed.
		/// </exception>
		public override void SetLength(long value)
		{
			ThrowIfDisposed();
			this.stream.SetLength(value);
		}

		/// <summary>
		/// When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
		/// </summary>
		/// <param name="buffer">An array of bytes. This method copies <paramref name="count"/> bytes from <paramref name="buffer"/> to the current stream.</param>
		/// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin copying bytes to the current stream.</param>
		/// <param name="count">The number of bytes to be written to the current stream.</param>
		/// <exception cref="T:System.ArgumentException">
		/// The sum of <paramref name="offset"/> and <paramref name="count"/> is greater than the buffer length.
		/// </exception>
		/// <exception cref="T:System.ArgumentNullException">
		/// 	<paramref name="buffer"/> is null.
		/// </exception>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		/// 	<paramref name="offset"/> or <paramref name="count"/> is negative.
		/// </exception>
		/// <exception cref="T:System.IO.IOException">
		/// An I/O error occurs.
		/// </exception>
		/// <exception cref="T:System.NotSupportedException">
		/// The stream does not support writing.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// Methods were called after the stream was closed.
		/// </exception>
		public override void Write(byte[] buffer, int offset, int count)
		{
			ThrowIfDisposed();
			this.stream.Write(buffer, offset, count);
		}

		#endregion Stream Overrides

		#region Internals

		/// <summary>
		/// Throws a <see cref="System.ObjectDisposedException"/> if the CvStream is disposed.
		/// </summary>
		private void ThrowIfDisposed()
		{
			if (this.stream == null)
				throw new ObjectDisposedException(@"CvStream");
		}

		#endregion Internals
	}
}
