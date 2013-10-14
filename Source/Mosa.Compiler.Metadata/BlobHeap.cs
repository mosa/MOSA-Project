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

namespace Mosa.Compiler.Metadata
{
	/// <summary>
	/// Provides access to binary blobs in the .NET provider blob heap.
	/// </summary>
	public sealed class BlobHeap : Heap
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Mosa.Compiler.Metadata.BlobHeap"/>.
		/// </summary>
		/// <param name="metadata">The provider buffer, which contains the blob heap.</param>
		/// <param name="offset">The offset into the buffer, where the heap starts.</param>
		/// <param name="size">The size of the heap.</param>
		public BlobHeap(byte[] metadata, int offset, int size)
			: base(metadata, offset, size)
		{
		}

		#endregion Construction

		#region Methods

		/// <summary>
		/// Retrieves the blob at the specified offset.
		/// </summary>
		/// <param name="token">The token where the blob begins.</param>
		/// <returns>A byte array, which represents the blob at the specified location.</returns>
		public byte[] ReadBlob(HeapIndexToken token)
		{
			Debug.Assert((HeapIndexToken.TableMask & token) == HeapIndexToken.Blob);
			if ((HeapIndexToken.TableMask & token) != HeapIndexToken.Blob)
				throw new ArgumentException(@"Invalid token value.", @"token");

			// Argument checks
			int offset = (int)(token & HeapIndexToken.RowIndexMask);
			if (0 == offset)
			{
				token += 1;
				return new byte[0];
			}

			// Validate the offset & calculate the real offset
			int realOffset = ValidateOffset(offset);
			int length = CalculatePrefixLength(ref realOffset);
			byte[] result = new byte[length];
			Array.Copy(this.Buffer, realOffset, result, 0, length);
			return result;
		}

		#endregion Methods
	}
}