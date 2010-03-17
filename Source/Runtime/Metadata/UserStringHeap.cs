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
	/// Provides a access to the strings in the .NET provider user string heap.
	/// </summary>
	public sealed class UserStringHeap : Heap {
		#region Construction

		/// <summary>
		/// Initializes a new instance of the UserStringHeap.
		/// </summary>
        /// <param name="metadata">The provider buffer, which contains the heap.</param>
		/// <param name="offset">The offset into the buffer, where the heap starts.</param>
		/// <param name="size">The size of the heap in bytes.</param>
		public UserStringHeap(byte[] metadata, int offset, int size)
			: base(metadata, offset, size)
		{
		}

		#endregion // Construction

		#region Methods
        
        /// <summary>
		/// Retrieves the string at the requested offset.
		/// </summary>
        /// <param name="token">The offset into the heap, where the string starts.</param>
		/// <returns>The string at the given offset.</returns>
		public string ReadString(ref TokenTypes token)
		{
            Debug.Assert((TokenTypes.TableMask & token) == TokenTypes.UserString);
            if ((TokenTypes.TableMask & token) != TokenTypes.UserString)
                throw new ArgumentException(@"Invalid token value.", @"token");

            int offset = (int)(token & TokenTypes.RowIndexMask);
			// Argument checks
            if (0 == offset)
            {
                token += 1;
                return String.Empty;
            }

			// Validate the offset & calculate the real offset
			int realOffset = ValidateOffset(offset);
			int length = CalculatePrefixLength(ref realOffset);
			Debug.Assert(1 == (length & 1), @"Invalid string length read from metadata - corrupt string?");
            token = (TokenTypes)((int)TokenTypes.UserString | realOffset + length - this._offset);
            if (0 == length)
				return String.Empty;
			byte[] buffer = this.Buffer;
			return Encoding.Unicode.GetString(buffer, realOffset, length-1);
		}

		#endregion // Methods
	}
}
