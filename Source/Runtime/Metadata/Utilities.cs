/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Mosa.Runtime.Metadata
{
	/// <summary>
	/// 
	/// </summary>
	public static class Utilities
	{
		/// <summary>
		/// Reads the compressed int32.
		/// </summary>
		/// <param name="buffer">The buffer.</param>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public static int ReadCompressedInt32(byte[] buffer, ref int index)
		{
			if (null == buffer)
				throw new ArgumentNullException(@"buffer");
			if (0 > index || index >= buffer.Length)
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
		/// <param name="provider">The provider.</param>
		/// <param name="buffer">The buffer to read the modifier From.</param>
		/// <param name="index">The start index, where the modifier is expected.</param>
		/// <returns></returns>
		public static bool ReadCustomMod(IMetadataProvider provider, byte[] buffer, ref int index)
		{
			bool result = (buffer[index] == (byte)CilElementType.Required || buffer[index] == (byte)CilElementType.Optional);
			if (result)
			{
				index++;
				ReadTypeDefOrRefEncoded(provider, buffer, ref index);
				Debug.WriteLine("Skipping CilElementType.Required or CilElementType.Optional.");
			}
			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		private static readonly TokenTypes[] _typeDefOrRefEncodedTables = new TokenTypes[] { TokenTypes.TypeDef, TokenTypes.TypeRef, TokenTypes.TypeSpec };

		/// <summary>
		/// Reads the type def or ref encoded.
		/// </summary>
		/// <param name="provider">The provider.</param>
		/// <param name="buffer">The buffer.</param>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public static TokenTypes ReadTypeDefOrRefEncoded(IMetadataProvider provider, byte[] buffer, ref int index)
		{
			int value = Utilities.ReadCompressedInt32(buffer, ref index);
			Debug.Assert(0 != (value & 0xFFFFFFFC), @"Invalid TypeDefOrRefEncoded index value.");
			TokenTypes token = (TokenTypes)((value >> 2) | (int)_typeDefOrRefEncodedTables[value & 0x03]);
			return token;
		}

		/// <summary>
		/// Reads an optional constraint modifier.
		/// </summary>
		/// <param name="signature">The signature buffer, where the constraint is expected.</param>
		/// <param name="index">The position, where the constraint is expected.</param>
		/// <returns>True, if a constraint was found otherwise false.</returns>
		public static bool ReadConstraint(byte[] signature, ref int index)
		{
			// FIXME: Influence the variable type somehow.
			bool result = (signature[index] == (byte)CilElementType.Pinned);
			if (result)
				index++;
			return result;
		}
	}
}
