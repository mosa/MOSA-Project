/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the GNU GPL v3, with Classpath Linking Exception
 * Licensed under the terms of the New BSD License for exclusive use by the Ensemble OS Project
 *
 * Authors:
 *  Michael Ruck (<mailto:sharpos@michaelruck.de>)
 */

using System;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace Mosa.Runtime.Metadata {

	/// <summary>
	/// Provides access to binary blobs in the .NET provider blob heap.
	/// </summary>
	public sealed class BlobHeap : Heap {
		
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Mosa.Runtime.Metadata.BlobHeap"/>.
		/// </summary>
		/// <param name="provider">The provider buffer, which contains the blob heap.</param>
		/// <param name="offset">The offset into the buffer, where the heap starts.</param>
		/// <param name="size">The size of the heap.</param>
		public BlobHeap(byte[] metadata, int offset, int size)
			: base(metadata, offset, size)
		{
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Retrieves the blob at the specified offset.
		/// </summary>
		/// <param name="offset">The offset into the heap, where the blob begins.</param>
		/// <returns>A byte array, which represents the blob at the specified location.</returns>
		public byte[] ReadBlob(TokenTypes token)
		{
            Debug.Assert((TokenTypes.TableMask & token) == TokenTypes.Blob);
            if ((TokenTypes.TableMask & token) != TokenTypes.Blob)
                throw new ArgumentException(@"Invalid token value.", @"token");

            // Argument checks
            int offset = (int)(token & TokenTypes.RowIndexMask);
			if (0 == offset)
				return new byte[0];

			// Validate the offset & calculate the real offset
			int realOffset = ValidateOffset(offset);
			int length = CalculatePrefixLength(ref realOffset);
			byte[] result = new byte[length];
			Array.Copy(this.Buffer, realOffset, result, 0, length);
            return result;
		}

		#endregion // Methods
	}
}
