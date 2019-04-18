// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using System;

namespace Mosa.Kernel.BareMetal.Extension
{
	public static class IntPtrExtension
	{
		public static ushort Load16(this IntPtr address, uint offset)
		{
			return Intrinsic.Load16(address, offset);
		}

		public static uint Load24(this IntPtr address, uint offset)
		{
			return Intrinsic.Load16(address, offset) | (uint)(Intrinsic.Load8(address, offset + 2) << 16);
		}

		public static uint Load32(this IntPtr address)
		{
			return Intrinsic.Load32(address);
		}

		public static uint Load32(this IntPtr address, uint offset)
		{
			return Intrinsic.Load32(address, offset);
		}

		public static uint Load32(this IntPtr address, int offset)
		{
			return Intrinsic.Load32(address, offset);
		}

		public static ulong Load64(this IntPtr address, int offset)
		{
			return Intrinsic.Load64(address, offset);
		}

		public static ulong Load64(this IntPtr address, uint offset)
		{
			return Intrinsic.Load64(address, offset);
		}

		public static byte Load8(this IntPtr address, uint offset)
		{
			return Intrinsic.Load8(address, offset);
		}

		public static byte Load8(this IntPtr address, int offset)
		{
			return Intrinsic.Load8(address, offset);
		}

		public static IntPtr LoadPointer(this IntPtr address)
		{
			return Intrinsic.LoadPointer(address);
		}

		public static IntPtr LoadPointer(this IntPtr address, uint offset)
		{
			return Intrinsic.LoadPointer(address, offset);
		}

		public static void Store16(this IntPtr address, uint offset, ushort value)
		{
			Intrinsic.Store16(address, offset, value);
		}

		public static void Store24(this IntPtr address, uint offset, uint value)
		{
			Intrinsic.Store16(address, offset, (ushort)(value & 0xFFFF));
			Intrinsic.Store8(address, offset + 2, (byte)((value >> 16) & 0xFF));
		}

		public static void Store32(this IntPtr address, uint offset, uint value)
		{
			Intrinsic.Store32(address, offset, value);
		}

		public static void Store32(this IntPtr address, int offset, uint value)
		{
			Intrinsic.Store32(address, offset, value);
		}

		public static void Store32(this IntPtr address, uint offset, int value)
		{
			Intrinsic.Store32(address, offset, value);
		}

		public static void Store32(this IntPtr address, int offset, int value)
		{
			Intrinsic.Store32(address, offset, value);
		}

		public static void Store32(this IntPtr address, uint value)
		{
			Intrinsic.Store32(address, value);
		}

		public static void Store64(this IntPtr address, uint offset, ulong value)
		{
			Intrinsic.Store64(address, offset, value);
		}

		public static void Store64(this IntPtr address, int offset, ulong value)
		{
			Intrinsic.Store64(address, offset, value);
		}

		public static void Store64(this IntPtr address, uint offset, long value)
		{
			Intrinsic.Store64(address, offset, value);
		}

		public static void Store8(this IntPtr address, uint offset, byte value)
		{
			Intrinsic.Store8(address, offset, value);
		}

		public static void Store8(this IntPtr address, int offset, byte value)
		{
			Intrinsic.Store8(address, offset, value);
		}

		public static void StorePointer(this IntPtr address, IntPtr value)
		{
			Intrinsic.StorePointer(address, 0, value);
		}

		public static void StorePointer(this IntPtr address, uint offset, IntPtr value)
		{
			Intrinsic.StorePointer(address, offset, value);
		}
	}
}
