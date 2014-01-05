/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Diagnostics;
using System.Text;

namespace Mosa.Compiler.Metadata
{
	/// <summary>
	/// Provides a access to the strings in the .NET provider string heap.
	/// </summary>
	public sealed class StringHeap : Heap
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Mosa.Compiler.Metadata.StringHeap"/>.
		/// </summary>
		/// <param name="metadata">The byte array, which holds the provider.</param>
		/// <param name="offset">The offset into the byte array, where the heap starts.</param>
		/// <param name="size">The size of the heap in bytes.</param>
		public StringHeap(byte[] metadata, int offset, int size)
			: base(metadata, offset, size)
		{
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Reads the string.
		/// </summary>
		/// <param name="offset">The offset.</param>
		/// <returns></returns>
		public string ReadString(int offset, out int size)
		{
			// Validate the offset & calculate the real offset
			int realOffset = ValidateOffset(offset);
			size = Array.IndexOf<byte>(Metadata, 0, realOffset) - realOffset;
			return Encoding.UTF8.GetString(Metadata, realOffset, size);
		}

		/// <summary>
		/// Reads the string.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public string ReadString(HeapIndexToken token)
		{
			Debug.Assert((HeapIndexToken.TableMask & token) == HeapIndexToken.String);
			if ((HeapIndexToken.TableMask & token) != HeapIndexToken.String)
				throw new ArgumentException(@"Invalid token value.", @"token");

			int size = 0;

			// Offset of the requested string
			return ReadString((int)(token & HeapIndexToken.RowIndexMask), out size);
		}

		#endregion Methods
	}
}