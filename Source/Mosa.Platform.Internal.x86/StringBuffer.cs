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
	public struct StringBuffer
	{
		[FieldOffset(0)]
		public uint Length;

		public const uint MaxLength = 100;
		public const uint EntrySize = 208;

		[FieldOffset(8)]
		unsafe public char* Chars;

		public void Append(string value)
		{
			for (var i = 0; i < value.Length; i++)
				Append(value[i]);
		}

		unsafe public void Append(char value)
		{
			if (Length + 1 >= MaxLength)
			{
				//TODO: Error
			}
			Length++;
			*(Chars + Length) = value;
		}

		public void Clear()
		{
			Length = 0;
		}

		public void Set(string value)
		{
			Clear();
			Append(value);
		}

	}
}