/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.IO;

namespace Mosa.Compiler.Metadata.Loader
{
	/// <summary>
	///
	/// </summary>
	public sealed class InstructionStream : Stream
	{
		#region Types

		/// <summary>
		///
		/// </summary>
		[Flags]
		private enum MethodFlags : ushort
		{
			/// <summary>
			///
			/// </summary>
			TinyFormat = 0x02,

			/// <summary>
			///
			/// </summary>
			FatFormat = 0x03,

			/// <summary>
			///
			/// </summary>
			MoreSections = 0x08,

			/// <summary>
			///
			/// </summary>
			InitLocals = 0x10,

			/// <summary>
			///
			/// </summary>
			CodeSizeMask = 0xF000,

			/// <summary>
			///
			/// </summary>
			HeaderMask = 0x0003
		}

		/// <summary>
		///
		/// </summary>
		[Flags]
		private enum MethodDataSectionType
		{
			/// <summary>
			///
			/// </summary>
			EHTable = 0x01,

			/// <summary>
			///
			/// </summary>
			OptIL = 0x02,

			/// <summary>
			///
			/// </summary>
			FatFormat = 0x40,

			/// <summary>
			///
			/// </summary>
			MoreSections = 0x80
		}

		#endregion Types

		#region Data members

		/// <summary>
		/// The CIL stream offset.
		/// </summary>
		private long _startOffset;

		/// <summary>
		/// Stream, which holds the il code to decode.
		/// </summary>
		private Stream _stream;

		#endregion Data members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="InstructionStream"/> class.
		/// </summary>
		/// <param name="assemblyStream">The stream, which represents the IL assembly.</param>
		/// <param name="offset">The offset, where the IL stream starts.</param>
		public InstructionStream(Stream assemblyStream, long offset)
		{
			// Check preconditions
			if (null == assemblyStream)
				throw new ArgumentNullException(@"assembly");

			// Store the arguments
			_stream = assemblyStream;
			_startOffset = offset;
			_stream.Position = offset;
		}

		#endregion Construction

		#region Stream Overrides

		/// <summary>
		/// Returns true if the current stream is able to read
		/// </summary>
		/// <value></value>
		/// <returns>
		/// True if the current stream is able to read, false otherwise
		/// </returns>
		public override bool CanRead
		{
			get { return true; }
		}

		/// <summary>
		/// Returns true if the current stream is able to seek
		/// </summary>
		/// <value></value>
		/// <returns>
		/// Returns true if the current stream is able to seek
		/// </returns>
		public override bool CanSeek
		{
			get { return true; }
		}

		/// <summary>
		/// When overridden in a derived class, gets a value indicating whether the current stream supports writing.
		/// </summary>
		/// <value></value>
		/// <returns>true if the stream supports writing; otherwise, false.
		/// </returns>
		public override bool CanWrite
		{
			get { return false; }
		}

		/// <summary>
		/// When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to the underlying device.
		/// </summary>
		/// <exception cref="T:System.IO.IOException">
		/// An I/O error occurs.
		/// </exception>
		public override void Flush()
		{
			// Do nothing. We can not flush.
		}

		/// <summary>
		/// Gets the stream's length in bytes.
		/// </summary>
		/// <value></value>
		/// <returns>
		/// The stream's length in bytes.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">
		/// A class derived from Stream does not support searching.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// Methods have been called after the stream has been closed.
		/// </exception>
		public override long Length
		{
			get { return _stream.Length; }
		}

		/// <summary>
		/// Gets the stream's current position or sets it.
		/// </summary>
		/// <value></value>
		/// <returns>
		/// The current position in the stream.
		/// </returns>
		/// <exception cref="T:System.IO.IOException">
		/// IOException
		/// </exception>
		/// <exception cref="T:System.NotSupportedException">
		/// The stream does not support searching.
		/// </exception>
		/// <exception cref="T:System.ObjectDisposedException">
		/// Methods have been called after the stream has been closed.
		/// </exception>
		public override long Position
		{
			get
			{
				return _stream.Position;
			}
			set
			{
				if (0 > value)
					throw new ArgumentOutOfRangeException(@"value");

				_stream.Position = value;
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
			// Check preconditions
			if (null == buffer)
				throw new ArgumentNullException(@"buffer");

			return _stream.Read(buffer, offset, count);
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
			// FIXME: Fix the seeking...
			return _stream.Seek(offset, origin);
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
			throw new NotSupportedException();
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
			throw new NotSupportedException();
		}

		#endregion Stream Overrides
	}
}