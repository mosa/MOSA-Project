/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;

namespace Mosa.Runtime.Metadata
{

	[Flags]
	public enum HeapIndexToken
	{
		
		/// <summary>
		/// Special constant to represent a user string heap token.
		/// </summary>
		UserString = 0x70000000,

		/// <summary>
		/// Special constant to represent a string heap token.
		/// </summary>
		String = 0x71000000,

		/// <summary>
		/// Constant to represent a blob heap token.
		/// </summary>
		Blob = 0x72000000,

		/// <summary>
		/// Constant to represent a guid heap token.
		/// </summary>
		Guid = 0x73000000,

		/// <summary>
		/// Table identification mask.
		/// </summary>
		TableMask = 0x7F000000,

		/// <summary>
		/// 
		/// </summary>
		RowIndexMask = 0x00FFFFFF
	}
}
