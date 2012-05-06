/*
 * (c) 2012 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Runtime.InteropServices;

namespace Mosa.Compiler.Common
{
	public static class Endian
	{
		public static readonly bool NativeIsLittleEndian = BitConverter.IsLittleEndian;

		public static ushort Swap(ushort value)
		{
			return (ushort)(((value & 0xFF00) >> 8) | ((value & 0x00FF) << 8));
		}

		public static uint Swap(uint value)
		{
			return ((value & 0xFF000000) >> 24) | ((value & 0x00FF0000) >> 8) | ((value & 0x0000FF00) << 8) | ((value & 0x000000FF) << 24);
		}

		public static ulong Swap(ulong value)
		{
			return ((value & 0xFF00000000000000) >> 56) | ((value & 0x00FF000000000000) >> 48) | ((value & 0x0000FF0000000000) >> 32) | ((value & 0x000000FF00000000) >> 16)
				 | ((value & 0x00000000FF000000) << 16) | ((value & 0x0000000000FF0000) << 32) | ((value & 0x000000000000FF00) << 48) | ((value & 0x00000000000000FF) << 56);
		}

		[StructLayout(LayoutKind.Explicit)]
		private struct Union4
		{
		
			[FieldOffset(0)]
			public float Single;
			[FieldOffset(0)]
			public int Int32;
			[FieldOffset(0)]
			public uint UInt32;
		}

		public static uint ConvertToUInt32(float value)
		{
			Union4 v = new Union4();
			v.Single = value;
			return v.UInt32;
		}
	}
}
