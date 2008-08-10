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
	/// Provides a access to the strings in the .NET provider string heap.
	/// </summary>
	public sealed class StringHeap : Heap {
		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="NetOS.Runtime.Metadata.StringHeap"/>.
		/// </summary>
		/// <param name="provider">The byte array, which holds the provider.</param>
		/// <param name="offset">The offset into the byte array, where the heap starts.</param>
		/// <param name="size">The size of the heap in bytes.</param>
		public StringHeap(byte[] metadata, int offset, int size)
			: base(metadata, offset, size)
		{
		}

		#endregion // Construction

        #region Methods

        public string ReadString(TokenTypes token)
        {
            Debug.Assert((TokenTypes.TableMask & token) == TokenTypes.String);
            if ((TokenTypes.TableMask & token) != TokenTypes.String)
                throw new ArgumentException(@"Invalid token value.", @"token");

            // Offset of the requested string
            int offset = (int)(token & TokenTypes.RowIndexMask);
            if (0 == offset)
                return String.Empty;

            // Validate the offset & calculate the real offset
            int realOffset = ValidateOffset(offset);
            byte[] buffer = this.Buffer;
            int endOffset = Array.IndexOf<byte>(buffer, 0, realOffset);
            return Encoding.UTF8.GetString(buffer, realOffset, endOffset - realOffset);
        }

        #endregion // Methods
    }
}
