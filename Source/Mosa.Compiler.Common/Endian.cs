// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Common
{
	public static class Endian
	{
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
			return ((value & 0xFF00000000000000) >> 56) | ((value & 0x00FF000000000000) >> 40) | ((value & 0x0000FF0000000000) >> 24) | ((value & 0x000000FF00000000) >> 8)
				 | ((value & 0x00000000FF000000) << 8) | ((value & 0x0000000000FF0000) << 24) | ((value & 0x000000000000FF00) << 40) | ((value & 0x00000000000000FF) << 56);
		}
	}
}
