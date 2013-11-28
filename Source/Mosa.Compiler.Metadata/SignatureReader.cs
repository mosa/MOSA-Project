﻿/*
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
		private int index = 0;

		public int Index { get { return index; } }

		public int Length { get { return buffer.Length; } }

		public byte this[int index] { get { return buffer[index]; } }

		/// <summary>
		/// Initializes a new instance of the <see cref="SignatureReader"/> class.
		/// </summary>
		/// <param name="buffer">The buffer.</param>
		public SignatureReader(byte[] buffer)
		{
			if (buffer == null)
				throw new ArgumentNullException(@"buffer");

			this.buffer = buffer;
			this.index = 0;
		}

		/// <summary>
		/// Reads the byte.
		/// </summary>
		/// <returns></returns>
		public byte ReadByte()
		{
			if (0 > index || index > buffer.Length)
				throw new ArgumentOutOfRangeException(@"index");

			return buffer[index++];
		}

		/// <summary>
		/// Peeks the byte.
		/// </summary>
		/// <returns></returns>
		public byte PeekByte()
		{
			if (0 > index || index > buffer.Length)
				throw new ArgumentOutOfRangeException(@"index");

			return buffer[index];
		}

		/// <summary>
		/// Skips the byte.
		/// </summary>
		public void SkipByte()
		{
			if (0 > index || index > buffer.Length)
				throw new ArgumentOutOfRangeException(@"index");

			index++;
		}

		/// <summary>
		/// Reads the compressed int32.
		/// </summary>
		/// <returns></returns>
		public int ReadCompressedInt32()
		{
			if (0 > index || index > buffer.Length)
				throw new ArgumentOutOfRangeException(@"index");

			int result = 0;
			if (0xC0 == (0xE0 & buffer[index]))
			{
				if (index + 3 >= buffer.Length)
					throw new ArgumentOutOfRangeException(@"index");

				result = ((buffer[index] & 0x1F) << 24) | (buffer[index + 1] << 16) | (buffer[index + 2] << 8) | (buffer[index + 3]);
				index += 4;
			}
			else if (0x80 == (0xC0 & buffer[index]))
			{
				if (index + 1 >= buffer.Length)
					throw new ArgumentOutOfRangeException(@"index");

				result = ((buffer[index] & 0x3F) << 8) | (buffer[index + 1]);
				index += 2;
			}
			else
			{
				Debug.Assert(0x00 == (0x80 & buffer[index]));
				result = buffer[index++];
			}
			return result;
		}

		/// <summary>
		/// Reads an (optional) custom modifier
		/// </summary>
		/// <returns></returns>
		public bool ReadCustomMod()
		{
			bool result = (buffer[index] == (byte)CilElementType.Required || buffer[index] == (byte)CilElementType.Optional);
			if (result)
			{
				index++;
				ReadEncodedTypeDefOrRef();
				//Debug.WriteLine("Skipping CilElementType.Required or CilElementType.Optional.");
			}
			return result;
		}

		/// <summary>
		///
		/// </summary>
		private static readonly TableType[] _typeDefOrRefEncodedTables2 = new TableType[] { TableType.TypeDef, TableType.TypeRef, TableType.TypeSpec };

		/// <summary>
		/// Reads the type def or ref encoded.
		/// </summary>
		/// <returns></returns>
		public Token ReadEncodedTypeDefOrRef()
		{
			int value = ReadCompressedInt32();
			Debug.Assert(0 != (value & 0xFFFFFFFC), @"Invalid TypeDefOrRefEncoded index value.");
			Token token = new Token(_typeDefOrRefEncodedTables2[value & 0x03], value >> 2);
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
			bool result = (buffer[index] == (byte)CilElementType.Pinned);
			if (result)
				index++;
			return result;
		}
	}
}