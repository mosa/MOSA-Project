/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.IO;
using System.Text;
using System.Diagnostics;

namespace Mosa.Runtime.Metadata {

	/// <summary>
	/// Represents the GUID heap in the .NET provider.
	/// </summary>
	public sealed class GuidHeap : Heap {

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Mosa.Runtime.Metadata.GuidHeap"/>.
		/// </summary>
		/// <param name="metadata">The provider buffer, where the guid heap is located.</param>
		/// <param name="offset">The offset into the buffer, where the GUID heap starts.</param>
		/// <param name="size">The size of the GUID heap in bytes.</param>
		public GuidHeap(byte[] metadata, int offset, int size)
			: base(metadata, offset, size)
		{
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Retrieves the GUID at the requested position in the heap.
		/// </summary>
		/// <param name="token">The Guid token, of the guid to retrieve.</param>
		/// <returns>The GUID at the specified location.</returns>
		public Guid ReadGuid(ref TokenTypes token)
		{
            Debug.Assert((TokenTypes.TableMask & token) == TokenTypes.Guid);
            if ((TokenTypes.TableMask & token) != TokenTypes.Guid)
                throw new ArgumentException(@"Invalid token value.", @"token");

            int index = (int)(token & TokenTypes.RowIndexMask);
			if (0 >= index--)
				return Guid.Empty;

			// Validate the offset & calculate the real offset
			int realOffset = ValidateOffset(index*16);
			byte[] buffer = this.Buffer;
            token = (TokenTypes)((int)TokenTypes.Guid | index + 1);
			return new Guid(BitConverter.ToInt32(buffer, realOffset), BitConverter.ToInt16(buffer, realOffset + 4), BitConverter.ToInt16(buffer, realOffset + 6), buffer[realOffset + 8], buffer[realOffset + 9], buffer[realOffset + 10], buffer[realOffset + 11], buffer[realOffset + 12], buffer[realOffset + 13], buffer[realOffset + 14], buffer[realOffset + 8]);
		}

		#endregion // Methods
	}
}
