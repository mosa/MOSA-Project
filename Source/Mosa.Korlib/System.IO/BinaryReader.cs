// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace System.IO
{
	public class BinaryReader : IDisposable
	{
		/// <summary>
		/// Initializes a new instance of the System.IO.BinaryReader class based on the supplied stream.
		/// </summary>
		/// <param name="input">A stream.</param>
		public BinaryReader(Stream input)
		{
			// Check if the stream is null
			if (input == null)
				throw new ArgumentException("The stream is null", "input");

			// Check if the stream can be read
			if (input.CanRead == false)
				throw new ArgumentException("The stream does not support reading", "input");

			// Set the base stream
			BaseStream = input;
		}

		/// <summary>
		/// Exposes access to the underlying stream of the System.IO.BinaryReader.
		/// </summary>
		public virtual Stream BaseStream
		{
			get;
			private set;
		}

		/// <summary>
		/// Closes the current reader and the underlying stream.
		/// </summary>
		public virtual void Close()
		{
			//TODO
			BaseStream.Close();
		}

		/// <summary>
		/// Releases unmanaged resources and optionally releases managed resources.
		/// </summary>
		/// <param name="disposing">Whether to release managed resources.</param>
		protected virtual void Dispose(bool disposing)
		{
			//TODO
			BaseStream.Dispose();
		}

		/// <summary>
		/// Returns the next available character and does not advance the byte or character position.
		/// </summary>
		/// <returns>The next available character, or -1 if no more characters are available or the stream does not support seeking.</returns>
		public virtual int PeekChar()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Reads a character from the underlying stream and advances the current position of the stream.
		/// </summary>
		/// <returns>The next character from the input stream, or -1 if no characters are currently available.</returns>
		public virtual int Read()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Reads the specified number of bytes from the stream with index as the starting point in the byte array.
		/// </summary>
		/// <param name="buffer">The buffer to read data into.</param>
		/// <param name="index">The starting point in the buffer at which to begin reading into the buffer.</param>
		/// <param name="count">The number of bytes to read.</param>
		/// <returns></returns>
		public virtual int Read(byte[] buffer, int index, int count)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Reads the specified number of characters from the stream with index as the starting point in the character array.
		/// </summary>
		/// <param name="buffer">The buffer to read data into.</param>
		/// <param name="index">The starting point in the buffer at which to begin reading into the buffer.</param>
		/// <param name="count">The number of characters to read.</param>
		/// <returns></returns>
		public virtual int Read(char[] buffer, int index, int count)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Reads in a 32-bit integer in compressed format.
		/// </summary>
		/// <returns>A 32-bit integer in compressed format.</returns>
		protected internal int Read7BitEncodedInt()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Reads a Boolean value from the current stream and advances the current position of the stream by one byte.
		/// </summary>
		/// <returns>True if the byte is non-zero, otherwise false.</returns>
		public virtual bool ReadBoolean()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Reads the next byte from the current stream and advances the current position of the stream by one byte.
		/// </summary>
		/// <returns>The next byte read from the current stream.</returns>
		public virtual byte ReadByte()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Reads the specified number of bytes from the current stream into a byte array and advances the current position by count bytes.
		/// </summary>
		/// <param name="count">The number of bytes to read.</param>
		/// <returns>A byte array containing data read from the underlying stream. This might be less than the number of bytes requested if the end of the stream is reached.</returns>
		public virtual byte[] ReadBytes(int count)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Reads the next character from the current stream and advances the current position of the stream.
		/// </summary>
		/// <returns>A character read from the current stream.</returns>
		public virtual char ReadChar()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Reads the specified number of characters from the current stream into a character array and advances the current position by count characters.
		/// </summary>
		/// <param name="count">The number of characters to read.</param>
		/// <returns>A character array containing data read from the underlying stream.
		/// This might be less than the number of characters requested if the end of the stream is reached.</returns>
		public virtual char[] ReadChars(int count)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Reads a decimal value from the current stream and advances the current position of the stream by sixteen bytes.
		/// </summary>
		/// <returns>A decimal value read from the current stream.</returns>
		public virtual decimal ReadDecimal()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Reads an 8-byte floating point value from the current stream and advances the current position of the stream by eight bytes.
		/// </summary>
		/// <returns>An 8-byte floating point value read from the current stream.</returns>
		public virtual double ReadDouble()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Reads a 2-byte signed integer from the current stream and advances the current position of the stream by two bytes.
		/// </summary>
		/// <returns>A 2-byte signed integer read from the current stream.</returns>
		public virtual short ReadInt16()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Reads a 4-byte signed integer from the current stream and advances the current position of the stream by four bytes.
		/// </summary>
		/// <returns>A 4-byte signed integer read from the current stream.</returns>
		public virtual int ReadInt32()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Reads an 8-byte signed integer from the current stream and advances the current position of the stream by eight bytes.
		/// </summary>
		/// <returns>An 8-byte signed integer read from the current stream.</returns>
		public virtual long ReadInt64()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Reads a signed byte from this stream and advances the current position of the stream by one byte.
		/// </summary>
		/// <returns>A signed byte read from the current stream.</returns>
		public virtual sbyte ReadSByte()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Reads a 4-byte floating point value from the current stream and advances the current position of the stream by four bytes.
		/// </summary>
		/// <returns>A 4-byte floating point value read from the current stream.</returns>
		public virtual float ReadSingle()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Reads a string from the current stream. The string is prefixed with the length, encoded as an integer seven bits at a time.
		/// </summary>
		/// <returns>The string being read.</returns>
		public virtual string ReadString()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Reads a 2-byte unsigned integer from the current stream and advances the position of the stream by two bytes.
		/// </summary>
		/// <returns>A 2-byte unsigned integer read from this stream.</returns>
		public virtual ushort ReadUInt16()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Reads a 4-byte unsigned integer from the current stream and advances the position of the stream by four bytes.
		/// </summary>
		/// <returns>A 4-byte unsigned integer read from this stream.</returns>
		public virtual uint ReadUInt32()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Reads an 8-byte unsigned integer from the current stream and advances the position of the stream by eight bytes.
		/// </summary>
		/// <returns>An 8-byte unsigned integer read from this stream.</returns>
		public virtual ulong ReadUInt64()
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
