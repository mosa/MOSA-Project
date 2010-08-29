/*
 * (c) 2009 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Text;
using Mosa.Runtime.Metadata;

namespace Mosa.Runtime.Loader.PE
{
	/// <summary>
	/// A stream header gibes the names, and the position and length of
	/// a particular table or heap. Note that the length of a StreamHeader
	/// structure is not fixed, but depends on the length of its name field
	/// *a variable length null-terminated string).
	/// </summary>
	public class StreamHeader
	{
		/// <summary>
		/// Memory offset to start of this stream from start of the metadata 
		/// root (according to ISO/IEC 23271:2006 (E), §24.2.1)
		/// </summary>
		private readonly int _offset;

		/// <summary>
		/// Size of this stream in bytes, shall be a multiple of 4.
		/// </summary>
		private readonly int _size;

		/// <summary>
		/// Name of the stream as null-terminated variable length
		/// array of ASCII characters, padded to the next -byte
		/// boundary with \0 characters. The name is limited to
		/// 32 characters.
		/// </summary>
		private readonly string _name;

		/// <summary>
		/// 
		/// </summary>
		private readonly HeapType _kind;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="metadata"></param>
		public StreamHeader(System.IO.BinaryReader reader, byte[] metadata)
		{
			_offset = reader.ReadInt32();
			_size = reader.ReadInt32();

			int position = (int)reader.BaseStream.Position;
			int length = Array.IndexOf<byte>(metadata, 0, position, 32);
			_name = Encoding.ASCII.GetString(metadata, position, length - position);

			if (_name.Equals("#Strings"))
			{
				_kind = HeapType.String;
			}
			else if (_name.Equals("#US"))
			{
				_kind = HeapType.UserString;
			}
			else if (_name.Equals("#Blob"))
			{
				_kind = HeapType.Blob;
			}
			else if (_name.Equals("#GUID"))
			{
				_kind = HeapType.Guid;
			}
			else if (_name.Equals("#~"))
			{
				_kind = HeapType.Tables;
			}
			else
			{
				throw new NotSupportedException("Offset: " + _offset + " \\ Size: " + _size + " \\ Name: " + _name.ToString());
			}
		}

		/// <summary>
		/// Memory offset to start of this stream from start of the metadata 
		/// root (according to ISO/IEC 23271:2006 (E), §24.2.1)
		/// </summary>
		public int Offset
		{
			get { return _offset; }
		}

		/// <summary>
		/// Size of this stream in bytes, shall be a multiple of 4.
		/// </summary>
		public int Size
		{
			get { return _size; }
		}

		/// <summary>
		/// 
		/// </summary>
		public HeapType Kind
		{
			get { return _kind; }
		}
	}
}
