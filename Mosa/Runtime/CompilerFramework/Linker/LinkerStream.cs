/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.IO;

namespace Mosa.Runtime.CompilerFramework.Linker
{
    /// <summary>
    /// A linker stream object used to control symbol sizes.
    /// </summary>
    public sealed class LinkerStream : Stream
    {
        #region Data members

        /// <summary>
        /// Specifies the number of bytes available to the symbol.
        /// </summary>
        private long length;

        /// <summary>
        /// The start position in the wrapped stream.
        /// </summary>
        private long start;

        /// <summary>
        /// The stream wrapped by this instance.
        /// </summary>
        private Stream stream;

        /// <summary>
        /// The linker symbol represented by the stream.
        /// </summary>
        private LinkerSymbol symbol;

        #endregion // Data members

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkerStream"/> class.
        /// </summary>
        /// <param name="symbol">The linker symbol created by this stream.</param>
        /// <param name="stream">The stream provided by the actual linker instance.</param>
        /// <param name="length">The length of the symbol. Set to zero, if length is unknown.</param>
        public LinkerStream(LinkerSymbol symbol, Stream stream, long length)
        {
            if (null == symbol)
                throw new ArgumentNullException(@"symbol");
            if (null == stream)
                throw new ArgumentNullException(@"stream");

            this.length = length;
            this.start = stream.Position;
            this.symbol = symbol;
            this.stream = stream;
        }

        #endregion // Construction

        #region Stream Overrides

        /// <summary>
        /// Gets a reference to the underlying stream.
        /// </summary>
        /// <value>A stream object that represents the underlying stream.</value>
        public Stream BaseStream
        {
            get 
            {
                if (this.stream == null)
                    throw new ObjectDisposedException(@"LinkerStream");

                return this.stream; 
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports reading.
        /// </summary>
        /// <value></value>
        /// <returns>true if the stream supports reading; otherwise, false.</returns>
        public override bool CanRead
        {
            get
            {
                if (this.stream == null)
                    throw new ObjectDisposedException(@"LinkerStream");

                return this.stream.CanRead; 
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports seeking.
        /// </summary>
        /// <value></value>
        /// <returns>true if the stream supports seeking; otherwise, false.</returns>
        public override bool CanSeek
        {
            get
            {
                if (this.stream == null)
                    throw new ObjectDisposedException(@"LinkerStream");

                return this.stream.CanSeek; 
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports writing.
        /// </summary>
        /// <value></value>
        /// <returns>true if the stream supports writing; otherwise, false.</returns>
        public override bool CanWrite
        {
            get 
            {
                if (this.stream == null)
                    throw new ObjectDisposedException(@"LinkerStream");

                return this.stream.CanWrite; 
            }
        }

        /// <summary>
        /// Gets a value that determines whether the current stream can time out.
        /// </summary>
        /// <value></value>
        /// <returns>A value that determines whether the current stream can time out.</returns>
        public override bool CanTimeout
        {
            get
            {
                if (null == this.stream)
                    throw new ObjectDisposedException(@"LinkerStream");

                return this.stream.CanTimeout;
            }
        }

        /// <summary>
        /// Gets or sets a value, in miliseconds, that determines how long the stream will attempt to read before timing out.
        /// </summary>
        /// <value></value>
        /// <returns>A value, in miliseconds, that determines how long the stream will attempt to read before timing out.</returns>
        /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.IO.Stream.ReadTimeout"/> method always throws an <see cref="T:System.InvalidOperationException"/>. </exception>
        public override int ReadTimeout
        {
            get
            {
                if (null == this.stream)
                    throw new ObjectDisposedException(@"LinkerStream");

                return this.stream.ReadTimeout;
            }

            set
            {
                if (null == this.stream)
                    throw new ObjectDisposedException(@"LinkerStream");

                this.stream.ReadTimeout = value;
            }
        }

        /// <summary>
        /// Gets or sets a value, in miliseconds, that determines how long the stream will attempt to write before timing out.
        /// </summary>
        /// <value></value>
        /// <returns>A value, in miliseconds, that determines how long the stream will attempt to write before timing out.</returns>
        /// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.IO.Stream.WriteTimeout"/> method always throws an <see cref="T:System.InvalidOperationException"/>. </exception>
        public override int WriteTimeout
        {
            get
            {
                if (null == this.stream)
                    throw new ObjectDisposedException(@"LinkerStream");

                return this.stream.WriteTimeout;
            }
            set
            {
                if (null == this.stream)
                    throw new ObjectDisposedException(@"LinkerStream");

                this.stream.WriteTimeout = value;
            }
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.IO.Stream"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (this.stream != null && this.symbol != null)
            {
                // Fix the linker symbol size
                this.symbol.Length = this.Position;

                // Clear the stream & symbol
                this.stream = null;
                this.symbol = null;
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        public override void Flush()
        {
            if (this.stream == null)
                throw new ObjectDisposedException(@"LinkerStream");
            this.stream.Flush();
        }

        /// <summary>
        /// When overridden in a derived class, gets the length in bytes of the stream.
        /// </summary>
        /// <value></value>
        /// <returns>A long value representing the length of the stream in bytes.</returns>
        /// <exception cref="T:System.NotSupportedException">A class derived from Stream does not support seeking. </exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public override long Length
        {
            get 
            {
                if (this.stream == null)
                    throw new ObjectDisposedException(@"LinkerStream");
                if (0 == this.length)
                    throw new System.NotSupportedException();

                return this.length;
            }
        }

        /// <summary>
        /// When overridden in a derived class, gets or sets the position within the current stream.
        /// </summary>
        /// <value></value>
        /// <returns>The current position within the stream.</returns>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        /// <exception cref="T:System.NotSupportedException">The stream does not support seeking. </exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public override long Position
        {
            get 
            {
                if (this.stream == null)
                    throw new ObjectDisposedException(@"LinkerStream");

                return this.stream.Position - this.start; 
            }
            set
            {
                if (this.stream == null)
                    throw new ObjectDisposedException(@"LinkerStream");
                if (value < 0 || (0 != this.length && value > this.length))
                    throw new ArgumentOutOfRangeException(@"value", value, @"Value must be equal to or larger than zero and less than the length of the stream.");

                this.stream.Position = (this.start + value);
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
        /// <exception cref="T:System.ArgumentException">The sum of <paramref name="offset"/> and <paramref name="count"/> is larger than the buffer length. </exception>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="buffer"/> is null. </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="offset"/> or <paramref name="count"/> is negative. </exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        /// <exception cref="T:System.NotSupportedException">The stream does not support reading. </exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public override int Read(byte[] buffer, int offset, int count)
        {
            if (null == this.stream)
                throw new ObjectDisposedException(@"LinkerStream");
            if (buffer == null)
                throw new ArgumentNullException(@"buffer");
            if (offset < 0)
                throw new ArgumentOutOfRangeException(@"offset", offset, @"Offset can't be negative.");
            if (count < 0)
                throw new ArgumentOutOfRangeException(@"count", count, @"Count can't be negative.");
            if (offset + count > buffer.Length)
                throw new ArgumentException(@"Buffer too small.", @"buffer");

            // Guard for the symbol size
            if (this.length != 0 && this.Position + count > this.length)
                count = (int)(this.length - this.Position);

            if (count != 0)
                count = this.stream.Read(buffer, offset, count);

            return count;
        }

        /// <summary>
        /// Reads a byte from the stream and advances the position within the stream by one byte, or returns -1 if at the end of the stream.
        /// </summary>
        /// <returns>
        /// The unsigned byte cast to an Int32, or -1 if at the end of the stream.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The stream does not support reading. </exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public override int ReadByte()
        {
            if (null == this.stream)
                throw new ObjectDisposedException(@"LinkerStream");

            // Check that we're not writing past the end of our stream
            if (0 != this.length && this.Position + 1 < this.Length)
                return -1;

            return this.stream.ReadByte();
        }

        /// <summary>
        /// When overridden in a derived class, sets the position within the current stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the <paramref name="origin"/> parameter.</param>
        /// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin"/> indicating the reference point used to obtain the new position.</param>
        /// <returns>
        /// The new position within the current stream.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">Stream doesn't support the specified <paramref name="origin"/>.</exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        /// <exception cref="T:System.NotSupportedException">The stream does not support seeking, such as if the stream is constructed from a pipe or console output. </exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public override long Seek(long offset, SeekOrigin origin)
        {
            if (null == this.stream)
                throw new ObjectDisposedException(@"LinkerStream");

            // FIXME: Guard for the symbol size
            switch (origin)
            {
                case SeekOrigin.Begin:
                    offset += this.start;
                    break;

                case SeekOrigin.End:
                    if (0 == this.length)
                        throw new ArgumentException(@"Can't seek from end, symbol doesn't have a fixed size.", @"origin");
                    offset = (this.length - offset);
                    if (offset < 0)
                        offset = 0;
                    break;

                case SeekOrigin.Current:
                    offset += this.Position;
                    if (offset < 0)
                        offset = 0;
                    if (offset > this.length)
                        offset = this.length - 1;
                    break;
            }

            return this.stream.Seek(offset, SeekOrigin.Begin) - this.start;
        }

        /// <summary>
        /// When overridden in a derived class, sets the length of the current stream.
        /// </summary>
        /// <param name="value">The desired length of the current stream in bytes.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        /// <exception cref="T:System.NotSupportedException">The stream does not support both writing and seeking, such as if the stream is constructed from a pipe or console output. </exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public override void SetLength(long value)
        {
            if (null == this.stream)
                throw new ObjectDisposedException(@"LinkerStream");
            throw new NotSupportedException();
        }

        /// <summary>
        /// When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies <paramref name="count"/> bytes from <paramref name="buffer"/> to the current stream.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        /// <exception cref="T:System.ArgumentException">The sum of <paramref name="offset"/> and <paramref name="count"/> is greater than the buffer length. </exception>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="buffer"/> is null. </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        /// 	<paramref name="offset"/> or <paramref name="count"/> is negative. </exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        /// <exception cref="T:System.NotSupportedException">The stream does not support writing. </exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (null == this.stream)
                throw new ObjectDisposedException(@"LinkerStream");
            if (buffer == null)
                throw new ArgumentNullException(@"buffer");
            if (offset < 0)
                throw new ArgumentOutOfRangeException(@"offset", offset, @"Offset can't be negative.");
            if (count < 0)
                throw new ArgumentOutOfRangeException(@"count", count, @"Count can't be negative.");
            if (offset + count > buffer.Length)
                throw new ArgumentException(@"Buffer too small.", @"buffer");

            // Check that we're not writing past the end of our stream
            if (0 != this.length && count + this.Position < this.Length)
                throw new IOException(@"Can't fit buffer in the remaining space for the current symbol.");

            this.stream.Write(buffer, offset, count);
        }

        /// <summary>
        /// Writes a byte to the current position in the stream and advances the position within the stream by one byte.
        /// </summary>
        /// <param name="value">The byte to write to the stream.</param>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        /// <exception cref="T:System.NotSupportedException">The stream does not support writing, or the stream is already closed. </exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed. </exception>
        public override void WriteByte(byte value)
        {
            if (null == this.stream)
                throw new ObjectDisposedException(@"LinkerStream");

            // Check that we're not writing past the end of our stream
            if (0 != this.length && this.Position + 1 < this.Length)
                throw new IOException(@"Can't fit value in the remaining space for the current symbol.");

            this.stream.WriteByte(value);
        }

        #endregion // Stream Overrides
    }
}
