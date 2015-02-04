/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Sebastian Loncar (Arakis) <sebastian.loncar@gmail.com>
 */

using System.Runtime.InteropServices;

namespace Mosa.Platform.Internal.x86
{
	[StructLayout(LayoutKind.Explicit, Size = 208)]
	public struct LinkedStringBuffer
	{
		[FieldOffset(0)]
		public uint Length;

		/// <summary>
		/// The next StringBuffer. Do NOT use it when the StringBuffer is on the stack.
		/// </summary>
		[FieldOffset(4)]
		public unsafe LinkedStringBuffer* Next;

		public const uint MaxLength = 100;
		public const uint EntrySize = 208;

		[FieldOffset(8)]
		unsafe public char* Chars;

		unsafe public bool HasNext
		{
			get { return (uint)Next != 0; }
		}

	}
}