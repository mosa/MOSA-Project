/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Diagnostics;

namespace Mosa.Compiler.Metadata
{
	public sealed class SignatureReader
	{
		private byte[] buffer = null;

		public int Index { get; private set; }

		public int Length { get { return buffer.Length; } }

		public byte this[int index] { get { return buffer[index]; } }

		/// <summary>
		/// Initializes a new instance of the <see cref="SignatureReader"/> class.
		/// </summary>
		/// <param name="buffer">The buffer.</param>
		public SignatureReader(byte[] buffer)
		{
			if (buffer == null)
				throw new ArgumentNullException("buffer");

			this.buffer = buffer;
			Index = 0;
		}

		/// <summary>
		/// Reads the byte.
		/// </summary>
		/// <returns></returns>
		public byte ReadByte()
		{
			if (Index < 0 || Index >= buffer.Length)
				throw new ArgumentOutOfRangeException("index");

			return buffer[Index++];
		}

		/// <summary>
		/// Peeks the byte.
		/// </summary>
		/// <returns></returns>
		public byte PeekByte()
		{
			if (Index < 0 || Index >= buffer.Length)
				throw new ArgumentOutOfRangeException(@"index");

			return buffer[Index];
		}

		/// <summary>
		/// Skips the byte.
		/// </summary>
		public void SkipByte()
		{
			if (Index < 0 || Index >= buffer.Length)
				throw new ArgumentOutOfRangeException(@"index");

			Index++;
		}

		/// <summary>
		/// Reads the compressed int32.
		/// </summary>
		/// <returns></returns>
		public int ReadCompressedInt32()
		{
			if (Index < 0 || Index >= buffer.Length)
				throw new ArgumentOutOfRangeException(@"index");

			int result = 0;
			if (0xC0 == (0xE0 & buffer[Index]))
			{
				if (Index + 3 >= buffer.Length)
					throw new ArgumentOutOfRangeException(@"index");

				result = ((buffer[Index] & 0x1F) << 24) | (buffer[Index + 1] << 16) | (buffer[Index + 2] << 8) | (buffer[Index + 3]);
				Index += 4;
			}
			else if (0x80 == (0xC0 & buffer[Index]))
			{
				if (Index + 1 >= buffer.Length)
					throw new ArgumentOutOfRangeException(@"index");

				result = ((buffer[Index] & 0x3F) << 8) | (buffer[Index + 1]);
				Index += 2;
			}
			else
			{
				Debug.Assert(0x00 == (0x80 & buffer[Index]));
				result = buffer[Index++];
			}
			return result;
		}

		/// <summary>
		/// Reads an (optional) custom modifier
		/// </summary>
		/// <returns></returns>
		public bool ReadCustomMod()
		{
			bool result = (buffer[Index] == (byte)CilElementType.Required || buffer[Index] == (byte)CilElementType.Optional);
			if (result)
			{
				Index++;
				ReadEncodedTypeDefOrRef();
			}
			return result;
		}

		/// <summary>
		///
		/// </summary>
		private static readonly TableType[] typeDefOrRefEncodedTables2 = new TableType[] { TableType.TypeDef, TableType.TypeRef, TableType.TypeSpec };

		/// <summary>
		/// Reads the type def or ref encoded.
		/// </summary>
		/// <returns></returns>
		public Token ReadEncodedTypeDefOrRef()
		{
			int value = ReadCompressedInt32();
			Debug.Assert(0 != (value & 0xFFFFFFFC), @"Invalid TypeDefOrRefEncoded index value.");
			Token token = new Token(typeDefOrRefEncodedTables2[value & 0x03], value >> 2);
			return token;
		}

		/// <summary>
		/// Reads the encoded token.
		/// </summary>
		/// <returns></returns>
		public HeapIndexToken ReadEncodedToken()
		{
			return (HeapIndexToken)ReadCompressedInt32();
		}

		/// <summary>
		/// Reads an optional constraint modifier.
		/// </summary>
		/// <returns>
		/// True, if a constraint was found otherwise false.
		/// </returns>
		public bool ReadConstraint()
		{
			// FIXME: Influence the variable type somehow.
			bool result = (buffer[Index] == (byte)CilElementType.Pinned);
			if (result)
				Index++;
			return result;
		}
	}
}