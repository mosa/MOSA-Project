/*
 * (c) 2009 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Bruce Markham (illuminus) <illuminus86@gmail.com>
 */

using System.IO;

namespace Mosa.Compiler.Common
{
	/// <summary>
	/// An implementation of BinaryWriter that uses Endianness-aware data writing
	/// </summary>
	/// <remarks>Not all write operations can be Endiannness-aware, but were implemented for completeness anyway.</remarks>
	public class EndianAwareBinaryWriter
		: BinaryWriter
	{
		#region Private Fields

		private DataConverter _DataConverter;

		private System.Text.Encoding _ExplicitEncoding;
		private System.Text.Encoding _AssumedEffectiveEncoding;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes this instance of EndianAwareBinaryWriter with the given output stream
		/// and text encoding, and assuming native Endianness
		/// </summary>
		/// <param name="output">The stream to write output to</param>
		/// <param name="encoding">The encoding to write text with</param>
		public EndianAwareBinaryWriter(System.IO.Stream output, System.Text.Encoding encoding)
			: this(output, encoding, DataConverter.Native)
		{ } // Nothing to do

		/// <summary>
		/// Initializes this instance of EndianAwareBinaryWriter with the given output stream,
		/// assuming the default text encoding and native Endianness
		/// </summary>
		/// <param name="output">The stream to write output to</param>
		public EndianAwareBinaryWriter(System.IO.Stream output)
			: this(output, DataConverter.Native)
		{ } // Nothing to do

		/// <summary>
		/// Initializes this instance of EndianAwareBinaryWriter with the given output stream,
		/// the given text encoding, and the Endianness of the given DataConverter
		/// </summary>
		/// <param name="output">The stream to write output to</param>
		/// <param name="encoding">The encoding to write text with</param>
		/// <param name="DataConverter">The DataConverter with the Endianness to use</param>
		public EndianAwareBinaryWriter(System.IO.Stream output, System.Text.Encoding encoding, DataConverter DataConverter)
			: base(output, encoding)
		{
			this._DataConverter = DataConverter;
			this._ExplicitEncoding = this._AssumedEffectiveEncoding = encoding;
		}

		/// <summary>
		/// Initializes this instance of EndianAwareBinaryWriter with the given output stream,
		/// assuming a default text encoding, and using the Endianness of the given DataConverter
		/// </summary>
		/// <param name="output">The stream to write output to</param>
		/// <param name="DataConverter">The DataConverter with the Endianness to use</param>
		public EndianAwareBinaryWriter(System.IO.Stream output, DataConverter DataConverter)
			: base(output)
		{
			this._DataConverter = DataConverter;
		}

		#endregion

		#region Support Properties

		/// <summary>
		/// Gets the DataConverter instance used to write with Endian sensitivity
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

		#region Write Methods

		/// <summary>
		/// Writes the given boolean value to the underlying stream, represented as a single byte
		/// </summary>
		/// <param name="value">The boolean value to write</param>
		public override void Write(bool value)
		{
			base.Write(value);
		}

		/// <summary>
		/// Writes the given byte to the underlying stream
		/// </summary>
		/// <param name="value">The byte to write</param>
		public override void Write(byte value)
		{
			base.Write(value);
		}

		/// <summary>
		/// Writes the given bytes to the underlying stream
		/// </summary>
		/// <param name="buffer">The byte array to be written</param>
		public override void Write(byte[] buffer)
		{
			base.Write(buffer);
		}

		/// <summary>
		/// Writes bytes from the given byte array to the underlying stream,
		/// using the given source index and length for the source array
		/// </summary>
		/// <param name="buffer">An array of bytes containing the bytes to be written</param>
		/// <param name="index">The index within the source byte array to begin reading From</param>
		/// <param name="count">The number of bytes within the source byte array to use</param>
		public override void Write(byte[] buffer, int index, int count)
		{
			base.Write(buffer, index, count);
		}

		/// <summary>
		/// Writes the given character to the stream in its binary format
		/// </summary>
		/// <param name="ch">The character to write</param>
		public override void Write(char ch)
		{
			base.Write(this.AssumedEffectiveEncoding.GetBytes(new char[] { ch }));
		}

		/// <summary>
		/// Writes the given characters to the stream in their binary format
		/// </summary>
		/// <param name="chars">The array of characters to write</param>
		public override void Write(char[] chars)
		{
			base.Write(this.AssumedEffectiveEncoding.GetBytes(chars));
		}

		/// <summary>
		/// Writes characters from the given character array to the stream, in their
		/// binary format
		/// </summary>
		/// <param name="chars">The character array containing the characters to write</param>
		/// <param name="index">The starting index of the characters to write</param>
		/// <param name="count">The number of characters from the source array to write</param>
		public override void Write(char[] chars, int index, int count)
		{
			base.Write(this.AssumedEffectiveEncoding.GetBytes(chars, index, count));
		}

		/// <summary>
		/// Writes the given System.Decimal value to the stream
		/// </summary>
		/// <param name="value">The value to write</param>
		/// <remarks>Because this is a CLI-specific type, it is always read and written the same way.</remarks>
		// NOTE: I couldn't find a reason this needs to be effected by Endianness (BMarkham 1/13/09)
		public override void Write(decimal value)
		{
			base.Write(value);
		}

		/// <summary>
		/// Writes the given System.Double value to the stream
		/// </summary>
		/// <param name="value">The value to write</param>
		public override void Write(double value)
		{
			base.Write(this._DataConverter.GetBytes(value));
		}

		/// <summary>
		/// Writes the given System.Single value to the stream
		/// </summary>
		/// <param name="value">The value to write</param>
		public override void Write(float value)
		{
			base.Write(this._DataConverter.GetBytes(value));
		}

		/// <summary>
		/// Writes the given System.Int32 value to the stream
		/// </summary>
		/// <param name="value">The value to write</param>
		public override void Write(int value)
		{
			base.Write(this._DataConverter.GetBytes(value));
		}

		/// <summary>
		/// Writes the given System.Int64 value to the stream
		/// </summary>
		/// <param name="value">The value to write</param>
		public override void Write(long value)
		{
			base.Write(this._DataConverter.GetBytes(value));
		}

		/// <summary>
		/// Writes the given System.SByte value to the stream
		/// </summary>
		/// <param name="value">The value to write</param>
		public override void Write(sbyte value)
		{
			base.Write(this._DataConverter.GetBytes(value));
		}

		/// <summary>
		/// Writes the given System.Int16 value to the stream
		/// </summary>
		/// <param name="value">The value to write</param>
		public override void Write(short value)
		{
			base.Write(this._DataConverter.GetBytes(value));
		}

		/// <summary>
		/// Writes a length-prefixed string to the stream
		/// </summary>
		/// <param name="value">The string to write</param>
		public override void Write(string value)
		{
			base.Write(value);
		}

		/// <summary>
		/// Writes the given System.UInt32 value to the stream
		/// </summary>
		/// <param name="value">The value to write</param>
		public override void Write(uint value)
		{
			base.Write(this._DataConverter.GetBytes(value));
		}

		/// <summary>
		/// Writes the given System.UInt64 value to the stream
		/// </summary>
		/// <param name="value">The value to write</param>
		public override void Write(ulong value)
		{
			base.Write(this._DataConverter.GetBytes(value));
		}

		/// <summary>
		/// Writes the given System.UInt16 value to the stream
		/// </summary>
		/// <param name="value">The value to write</param>
		public override void Write(ushort value)
		{
			base.Write(this._DataConverter.GetBytes(value));
		}

		#endregion
	}
}
