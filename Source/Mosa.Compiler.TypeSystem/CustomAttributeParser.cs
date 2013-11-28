﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Mosa.Compiler.Metadata;
using Mosa.Compiler.Metadata.Signatures;

namespace Mosa.Compiler.TypeSystem
{
	/// <summary>
	/// Parses and instantiates custom attributes in assembly metadata blobs.
	/// </summary>
	public static class CustomAttributeParser
	{
		#region Constants

		/// <summary>
		/// The prologue id for a custom attribute blob.
		/// </summary>
		private const ushort ATTRIBUTE_BLOB_PROLOGUE = 0x0001;

		/// <summary>
		/// Length of a null string in a custom attribute blob.
		/// </summary>
		private const byte ATTRIBUTE_NULL_STRING_LEN = 0xFF;

		/// <summary>
		/// Length of an empty string in a custom attribute blob.
		/// </summary>
		private const byte ATTRIBUTE_EMPTY_STRING_LEN = 0x00;

		#endregion Constants

		#region Methods

		/// <summary>
		/// Parses the specified attribute blob and instantiates the attribute.
		/// </summary>
		/// <param name="blob">The BLOB.</param>
		/// <param name="attributeCtor">The constructor of the attribute.</param>
		/// <returns>
		/// The fully instantiated and initialized attribute.
		/// </returns>
		public static object[] Parse(byte[] blob, RuntimeMethod attributeCtor)
		{
			if (blob == null)
				throw new ArgumentException(@"Invalid attribute blob token.", @"attributeBlob");

			if (blob.Length == 0)
				return null;

			// Create a binary reader for the blob
			using (var reader = new BinaryReader(new MemoryStream(blob), Encoding.UTF8))
			{
				var prologue = reader.ReadUInt16();
				Debug.Assert(prologue == ATTRIBUTE_BLOB_PROLOGUE, @"Attribute prologue doesn't match.");
				if (prologue != ATTRIBUTE_BLOB_PROLOGUE)
					throw new ArgumentException(@"Invalid custom attribute blob.", "attributeBlob");

				var parameters = attributeCtor.Parameters.Count;

				object[] args = new object[parameters];
				for (int idx = 0; idx < parameters; idx++)
					args[idx] = ParseFixedArg(reader, attributeCtor.SigParameters[idx]);

				// Create the attribute instance
				return args;
			}
		}

		#endregion Methods

		#region Internals

		/// <summary>
		/// Parses a fixed argument in an attribute blob definition.
		/// </summary>
		/// <param name="reader">The binary reader to read it From.</param>
		/// <param name="sigType">The signature type of the value to read.</param>
		/// <returns></returns>
		private static object ParseFixedArg(BinaryReader reader, SigType sigType)
		{
			// Return value
			object result = null;

			// A vector?
			var arraySigType = sigType as SZArraySigType;
			if (arraySigType != null)
				result = ParseSZArrayArg(reader, arraySigType);
			else
				result = ParseElem(reader, sigType);

			return result;
		}

		/// <summary>
		/// Parses an SZArray attribute value.
		/// </summary>
		/// <param name="reader">The binary reader used to read from the attribute blob.</param>
		/// <param name="sigType">Type of the SZArray.</param>
		/// <returns>
		/// An Array, which represents the SZArray definition.
		/// </returns>
		private static object ParseSZArrayArg(BinaryReader reader, SZArraySigType sigType)
		{
			// Return value
			Array result;

			// Determine the number of elements
			int numElements = reader.ReadInt32();
			if (-1 == numElements)
				return null;

			// Retrieve the array element type
			Type elementType = GetTypeFromSigType(sigType);
			Debug.Assert(null != elementType, @"Failed to get System.Type for SigType.");

			result = Array.CreateInstance(elementType, numElements);
			for (int idx = 0; idx < numElements; idx++)
			{
				object item = ParseElem(reader, sigType.ElementType);
				result.SetValue(item, idx);
			}

			return result;
		}

		/// <summary>
		/// Parses an elementary field, parameter or property definition.
		/// </summary>
		/// <param name="reader">The binary reader to read data From.</param>
		/// <param name="sigType">The signature type of the field, parameter or property to read.</param>
		/// <returns>
		/// An object, which represents the value read from the attribute blob.
		/// </returns>
		/// <exception cref="System.NotSupportedException"><paramref name="sigType"/> is not yet supported.</exception>
		private static object ParseElem(BinaryReader reader, SigType sigType)
		{
			object result;

			switch (sigType.Type)
			{
				case CilElementType.Boolean:
					result = (1 == reader.ReadByte());
					break;

				case CilElementType.Char:
					result = (char)reader.ReadUInt16();
					break;

				case CilElementType.I1:
					result = reader.ReadSByte();
					break;

				case CilElementType.I2:
					result = reader.ReadInt16();
					break;

				case CilElementType.I4:
					result = reader.ReadInt32();
					break;

				case CilElementType.I8:
					result = reader.ReadInt64();
					break;

				case CilElementType.U1:
					result = reader.ReadByte();
					break;

				case CilElementType.U2:
					result = reader.ReadUInt16();
					break;

				case CilElementType.U4:
					result = reader.ReadUInt32();
					break;

				case CilElementType.U8:
					result = reader.ReadUInt64();
					break;

				case CilElementType.R4:
					result = reader.ReadSingle();
					break;

				case CilElementType.R8:
					result = reader.ReadDouble();
					break;

				case CilElementType.String:
					result = ParseSerString(reader);
					break;

				case CilElementType.Type:
					throw new NotSupportedException();

				case CilElementType.Class:
					throw new NotSupportedException();

				case CilElementType.ValueType:
					throw new NotSupportedException();

				case CilElementType.Object:
					throw new NotSupportedException();

				default:
					throw new NotSupportedException();
			}

			return result;
		}

		/// <summary>
		/// Parses a string definition for an attribute field, parameter or property definition.
		/// </summary>
		/// <param name="reader">The binary reader to read From.</param>
		/// <returns>A string, which represents the value read.</returns>
		private static string ParseSerString(BinaryReader reader)
		{
			// Read the length
			int packedLen = DecodePackedLen(reader);
			if (ATTRIBUTE_NULL_STRING_LEN == packedLen)
				return null;
			if (ATTRIBUTE_EMPTY_STRING_LEN == packedLen)
				return String.Empty;

			// Read the string
			var buffer = reader.ReadChars(packedLen);
			return new String(buffer);
		}

		private static int DecodePackedLen(BinaryReader reader)
		{
			int result, offset;
			byte value = reader.ReadByte();

			if (0xC0 == (value & 0xC0))
			{
				// A 4 byte length...
				result = ((value & 0x1F) << 24);
				offset = 16;
			}
			else if (0x80 == (value & 0x80))
			{
				// A 2 byte length...
				result = ((value & 0x3F) << 8);
				offset = 0;
			}
			else
			{
				result = value & 0x7F;
				offset = -8;
			}

			while (offset != -8)
			{
				result |= (reader.ReadByte() << offset);
				offset -= 8;
			}

			return result;
		}

		/// <summary>
		/// Gets the type from the signature type.
		/// </summary>
		/// <param name="sigType">The signature type.</param>
		/// <returns>The System.Type represented by the signature type.</returns>
		private static Type GetTypeFromSigType(SigType sigType)
		{
			Type result = null;

			switch (sigType.Type)
			{
				case CilElementType.I: result = typeof(IntPtr); break;
				case CilElementType.I1: result = typeof(SByte); break;
				case CilElementType.I2: result = typeof(Int16); break;
				case CilElementType.I4: result = typeof(Int32); break;
				case CilElementType.I8: result = typeof(Int64); break;
				case CilElementType.U: result = typeof(UIntPtr); break;
				case CilElementType.U1: result = typeof(Byte); break;
				case CilElementType.U2: result = typeof(UInt16); break;
				case CilElementType.U4: result = typeof(UInt32); break;
				case CilElementType.U8: result = typeof(UInt64); break;
				case CilElementType.R4: result = typeof(Single); break;
				case CilElementType.R8: result = typeof(Double); break;
				case CilElementType.String: result = typeof(String); break;
				case CilElementType.Object: result = typeof(System.Object); break;
				default:
					throw new NotSupportedException();
			}

			return result;
		}

		#endregion Internals
	}
}