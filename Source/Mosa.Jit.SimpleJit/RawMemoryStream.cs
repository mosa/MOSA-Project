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

namespace Mosa.Jit.SimpleJit
{
	/// <summary>
	/// Implements a raw memory stream on top of UnmanagedMemoryStream.
	/// </summary>
	sealed class RawMemoryStream : UnmanagedMemoryStream
	{
		#region Construction

		/// <summary>
		/// Initializes a new raw memory stream.
		/// </summary>
		/// <param name="memory">A pointer to the memory of the stream.</param>
		/// <param name="size">The number of bytes of memory available.</param>
		internal unsafe RawMemoryStream(IntPtr memory, long size)
		{
			base.Initialize((byte*)memory.ToPointer(), size, size, FileAccess.ReadWrite);
		}

		#endregion // Construction
	}
}
