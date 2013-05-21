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
		/// <param name="token">The token.</param>
		/// <returns></returns>
		public string ReadString(HeapIndexToken token)
		{
			Debug.Assert((HeapIndexToken.TableMask & token) == HeapIndexToken.String);
			if ((HeapIndexToken.TableMask & token) != HeapIndexToken.String)
				throw new ArgumentException(@"Invalid token value.", @"token");

			// Offset of the requested string
			int offset = (int)(token & HeapIndexToken.RowIndexMask);
			if (offset == 0)
				return String.Empty;

			// Validate the offset & calculate the real offset
			int realOffset = ValidateOffset(offset);
			byte[] buffer = this.Buffer;
			int endOffset = Array.IndexOf<byte>(buffer, 0, realOffset);
			return Encoding.UTF8.GetString(buffer, realOffset, endOffset - realOffset);
		}

		#endregion Methods
	}
}