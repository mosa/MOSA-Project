/*
 * (c) 2009 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Bruce Markham (illuminus) <illuminus86@gmail.com>
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace System.IO
{
    /// <summary>
    /// An implementation of BinaryReader that uses Endianness-aware data reading
    /// </summary>
    /// <remarks>Not all read operations can be Endiannness-aware, but were implemented for completeness anyway.</remarks>
    public class EndianAwareBinaryReader
        : BinaryReader
    {
        #region Private Fields
        private System.DataConverter _DataConverter;
        
        private System.Text.Encoding _ExplicitEncoding;
        private System.Text.Encoding _AssumedEffectiveEncoding;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes this instance of EndianAwareBinaryReader with the given input stream
        /// and text encoding, and assuming native Endianness
        /// </summary>
        /// <param name="output">The stream to read input From</param>
        /// <param name="encoding">The encoding to read text with</param>
        public EndianAwareBinaryReader(System.IO.Stream output, System.Text.Encoding encoding)
            : this(output,encoding,System.DataConverter.Native)
        { } // Nothing to do

        /// <summary>
        /// Initializes this instance of EndianAwareBinaryReader with the given input stream,
        /// assuming the default text encoding and native Endianness
        /// </summary>
        /// <param name="output">The stream to read input From</param>
        public EndianAwareBinaryReader(System.IO.Stream output)
            : this(output, System.DataConverter.Native)
        { } // Nothing to do
        
        /// <summary>
        /// Initializes this instance of EndianAwareBinaryReader with the given input stream,
        /// the given text encoding, and the Endianness of the given DataConverter
        /// </summary>
        /// <param name="output">The stream to read input From</param>
        /// <param name="encoding">The encoding to read text with</param>
        /// <param name="DataConverter">The DataConverter with the Endianness to use</param>
        public EndianAwareBinaryReader(System.IO.Stream output, System.Text.Encoding encoding, System.DataConverter DataConverter)
            : base(output,encoding)
        {
            this._DataConverter = DataConverter;
            this._ExplicitEncoding = this._AssumedEffectiveEncoding = encoding;
        }
        
        /// <summary>
        /// Initializes this instance of EndianAwareBinaryReader with the given input stream,
        /// assuming a default text encoding, and using the Endianness of the given DataConverter
        /// </summary>
        /// <param name="output">The stream to read input From</param>
        /// <param name="DataConverter">The DataConverter with the Endianness to use</param>
        public EndianAwareBinaryReader(System.IO.Stream output, System.DataConverter DataConverter)
            : base(output)
        {
            this._DataConverter = DataConverter;
        }

        #endregion

        #region Support Properties
        /// <summary>
        /// Gets the DataConverter instance used to read with Endian sensitivity
        /// </summary>
        public DataConverter DataConverter
        {
            get
            {
                return this._DataConverter;
            }
        }

        /// <summary>
        /// The encoding that this sub-class will use if one wasn't passed through the constructor
        /// </summary>
        /// <remarks>For some reason, the base class' encoding isn't exposed</remarks>
        protected System.Text.Encoding AssumedEffectiveEncoding
        {
            get
            {
                if (_AssumedEffectiveEncoding == null)
                {
                    this._AssumedEffectiveEncoding = (this._ExplicitEncoding == null ? System.Text.Encoding.Default : this._ExplicitEncoding);
                }
                return _AssumedEffectiveEncoding;
            }
        }

        #endregion

        #region Read Methods

        /// <summary>
        /// Returns the next available character without advancing the underlying stream
        /// </summary>
        /// <returns>The next available character or -1 if no characters are available</returns>
        public override int PeekChar()
        {
            return base.PeekChar();
        }

        /// <summary>
        /// Reads the next available character from the stream and advances the stream appropriately
        /// </summary>
        /// <returns>The next available character, or -1 if none are available</returns>
        public override int Read()
        {
            return base.Read();
        }

        /// <summary>
        /// Reads the given number of bytes from the underlying stream into the given buffer
        /// at the given index
        /// </summary>
        /// <param name="buffer">The buffer to read data into</param>
        /// <param name="index">The index within the buffer to begin writing into</param>
        /// <param name="count">The number of bytes to read from the underlying stream</param>
        /// <returns>The actual number of bytes written into the buffer</returns>
        public override int Read(byte[] buffer, int index, int count)
        {
            return base.Read(buffer, index, count);
        }

        /// <summary>
        /// Reads the given number of characters from the underlying stream into the given buffer
        /// at the given index
        /// </summary>
        /// <param name="buffer">The buffer to read data into</param>
        /// <param name="index">The index within the buffer to begin writing into</param>
        /// <param name="count">The number of characters to read from the underlying stream</param>
        /// <returns>The actual number of characters written into the buffer</returns>
        public override int Read(char[] buffer, int index, int count)
        {
            return base.Read(buffer, index, count);
        }

        /// <summary>
        /// Reads a boolean from the stream in the form of a single byte
        /// </summary>
        /// <returns>The boolean value read from the stream</returns>
        public override bool ReadBoolean()
        {
            return base.ReadBoolean();
        }

        /// <summary>
        /// Reads a single byte from the stream
        /// </summary>
        /// <returns>The byte that was read</returns>
        public override byte ReadByte()
        {
            return base.ReadByte();
        }

        /// <summary>
        /// Reads the given number of bytes from the stream
        /// </summary>
        /// <param name="count">The number of bytes to read from the stream</param>
        /// <returns>A byte array containing the bytes read from the stream</returns>
        public override byte[] ReadBytes(int count)
        {
            return base.ReadBytes(count);
        }

        /// <summary>
        /// Reads a single character from the stream
        /// </summary>
        /// <returns>The character read from the stream</returns>
        public override char ReadChar()
        {
            return base.ReadChar();
        }

        /// <summary>
        /// Reads the given number of characters from the stream
        /// </summary>
        /// <param name="count">The number of characters to read from the stream</param>
        /// <returns>An array of characters read from the stream</returns>
        public override char[] ReadChars(int count)
        {
            return base.ReadChars(count);
        }

        /// <summary>
        /// Reads a decimal value from the stream
        /// </summary>
        /// <returns>The decimal value read from the stream</returns>
        public override decimal ReadDecimal()
        {
            // NOTE: I couldn't find a reason this needs to be effected by Endianness (BMarkham 1/13/09)
            return base.ReadDecimal();
        }

        /// <summary>
        /// Reads a double-precision floating point value from the stream
        /// </summary>
        /// <returns>The System.Double that was read from the stream</returns>
        public override double ReadDouble()
        {
            const int bytesInDouble = 8;
            return _DataConverter.GetDouble(base.ReadBytes(bytesInDouble),0);
        }

        /// <summary>
        /// Reads a System.Int16 from the stream
        /// </summary>
        /// <returns>The System.Int16 read from the stream</returns>
        public override short ReadInt16()
        {
            return _DataConverter.GetInt16(base.ReadBytes(2), 0);
        }

        /// <summary>
        /// Reads a System.Int32 from the stream
        /// </summary>
        /// <returns>The System.Int32 read from the stream</returns>
        public override int ReadInt32()
        {
            return _DataConverter.GetInt32(base.ReadBytes(4), 0);
        }

        /// <summary>
        /// Reads a System.Int64 from the stream
        /// </summary>
        /// <returns>The System.Int64 read from the stream</returns>
        public override long ReadInt64()
        {
            return _DataConverter.GetInt64(base.ReadBytes(8), 0);
        }

        /// <summary>
        /// Reads a System.SByte from the stream
        /// </summary>
        /// <returns>The System.SByte read from the stream</returns>
        public override sbyte ReadSByte()
        {
            return base.ReadSByte();
        }

        /// <summary>
        /// Reads a System.Single from the stream
        /// </summary>
        /// <returns>The System.Single read from the stream</returns>
        public override float ReadSingle()
        {
            const int bytesInFloat = 4;
            return _DataConverter.GetFloat(base.ReadBytes(bytesInFloat), 0);
        }

        /// <summary>
        /// Reads a length-prefixed string from the stream
        /// </summary>
        /// <returns>The string read from the stream</returns>
        public override string ReadString()
        {
            return base.ReadString();
        }

        /// <summary>
        /// Reads a System.UInt16 from the stream
        /// </summary>
        /// <returns>The System.UInt16 read from the stream</returns>
        public override ushort ReadUInt16()
        {
            return _DataConverter.GetUInt16(base.ReadBytes(2), 0);
        }

        /// <summary>
        /// Reads a System.UInt32 from the stream
        /// </summary>
        /// <returns>The System.UInt32 read from the stream</returns>
        public override uint ReadUInt32()
        {
            return _DataConverter.GetUInt32(base.ReadBytes(4), 0);
        }

        /// <summary>
        /// Reads a System.UInt64 from the stream
        /// </summary>
        /// <returns>The System.UInt64 read from the stream</returns>
        public override ulong ReadUInt64()
        {
            return _DataConverter.GetUInt64(base.ReadBytes(8), 0);
        }
        #endregion
    }
}
