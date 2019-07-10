// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Workspace.Kernel.Internal;
using System;
using System.Runtime.CompilerServices;

namespace Mosa.Runtime
{
	/// <summary>
	/// Provides stub methods for selected native IR instructions.
	/// </summary>
	public static class Intrinsic
	{
		#region Intrinsic

		public static byte Load8(IntPtr address)
		{
			return CPU.Read8((ulong)address.ToInt64());
		}

		public static byte Load8(IntPtr address, int offset)
		{
			return CPU.Read8((ulong)(address.ToInt64() + offset));
		}

		public static byte Load8(IntPtr address, uint offset)
		{
			return CPU.Read8((ulong)(address.ToInt64() + offset));
		}

		public static ushort Load16(IntPtr address)
		{
			return CPU.Read16((ulong)address.ToInt64());
		}

		public static ushort Load16(IntPtr address, int offset)
		{
			return CPU.Read16((ulong)(address.ToInt64() + offset));
		}

		public static ushort Load16(IntPtr address, uint offset)
		{
			return CPU.Read16((ulong)(address.ToInt64() + offset));
		}

		public static uint Load32(IntPtr address)
		{
			return CPU.Read32((ulong)address.ToInt64());
		}

		public static IntPtr LoadPointer(IntPtr address)
		{
			if (CPU.Is32Bit)
				return new IntPtr(Load32(address));
			else
				return new IntPtr((long)Load64(address));
		}

		public static IntPtr LoadPointer(IntPtr address, int offset)
		{
			if (CPU.Is32Bit)
				return new IntPtr(Load32(address, offset));
			else
				return new IntPtr((long)Load64(address, offset));
		}

		public static IntPtr LoadPointer(IntPtr address, uint offset)
		{
			if (CPU.Is32Bit)
				return new IntPtr(Load32(address, offset));
			else
				return new IntPtr((long)Load64(address, offset));
		}

		public static uint Load32(IntPtr address, int offset)
		{
			return CPU.Read32((ulong)(address.ToInt64() + offset));
		}

		public static uint Load32(IntPtr address, uint offset)
		{
			return CPU.Read32((ulong)(address.ToInt64() + offset));
		}

		public static ulong Load64(IntPtr address)
		{
			return CPU.Read64((ulong)address.ToInt64());
		}

		public static ulong Load64(IntPtr address, int offset)
		{
			return CPU.Read64((ulong)(address.ToInt64() + offset));
		}

		public static ulong Load64(IntPtr address, uint offset)
		{
			return CPU.Read64((ulong)(address.ToInt64() + offset));
		}

		public static float LoadR4(IntPtr address)
		{
			// TODO
			return 0;
		}

		public static float LoadR4(IntPtr address, uint offset)
		{
			//TODO
			return 0;
		}

		public static double LoadR8(IntPtr address)
		{
			//TODO
			return 0;
		}

		public static double LoadR8(IntPtr address, uint offset)
		{
			//TODO
			return 0;
		}

		public static void Store8(IntPtr address, byte value)
		{
			CPU.Write8((ulong)address.ToInt64(), value);
		}

		public static void Store8(IntPtr address, int offset, byte value)
		{
			CPU.Write8((ulong)(address.ToInt64() + offset), value);
		}

		public static void Store8(IntPtr address, uint offset, byte value)
		{
			CPU.Write8((ulong)(address.ToInt64() + offset), value);
		}

		public static void Store16(IntPtr address, ushort value)
		{
			CPU.Write16((ulong)address.ToInt64(), value);
		}

		public static void Store16(IntPtr address, int offset, ushort value)
		{
			CPU.Write16((ulong)(address.ToInt64() + offset), value);
		}

		public static void Store16(IntPtr address, uint offset, ushort value)
		{
			CPU.Write16((ulong)(address.ToInt64() + offset), value);
		}

		public static void Store32(IntPtr address, uint value)
		{
			CPU.Write32((ulong)address.ToInt64(), value);
		}

		public static void Store32(IntPtr address, int value)
		{
			CPU.Write32((ulong)address.ToInt64(), (uint)value);
		}

		public static void Store32(IntPtr address, int offset, uint value)
		{
			CPU.Write32((ulong)(address.ToInt64() + offset), value);
		}

		public static void Store32(IntPtr address, int offset, int value)
		{
			CPU.Write32((ulong)(address.ToInt64() + offset), (uint)value);
		}

		public static void Store32(IntPtr address, uint offset, uint value)
		{
			CPU.Write32((ulong)(address.ToInt64() + offset), value);
		}

		public static void Store32(IntPtr address, uint offset, int value)
		{
			CPU.Write32((ulong)(address.ToInt64() + offset), (uint)value);
		}

		public static void Store64(IntPtr address, ulong value)
		{
			CPU.Write64((ulong)address.ToInt64(), value);
		}

		public static void Store64(IntPtr address, int offset, ulong value)
		{
			CPU.Write64((ulong)(address.ToInt64() + offset), value);
		}

		public static void Store64(IntPtr address, uint offset, ulong value)
		{
			CPU.Write64((ulong)(address.ToInt64() + offset), value);
		}

		public static void Store64(IntPtr address, int offset, long value)
		{
			CPU.Write64((ulong)(address.ToInt64() + offset), (ulong)value);
		}

		public static void Store64(IntPtr address, uint offset, long value)
		{
			CPU.Write64((ulong)(address.ToInt64() + offset), (ulong)value);
		}

		public static void Store64(IntPtr address, IntPtr value)
		{
			if (CPU.Is32Bit)
				CPU.Write64((ulong)address.ToInt64(), (uint)value.ToInt32());
			else
				CPU.Write64((ulong)address.ToInt64(), (ulong)value.ToInt64());
		}

		public static void StoreR4(IntPtr address, float value)
		{
			// TODO
		}

		public static void StoreR4(IntPtr address, int offset, float value)
		{
			// TODO
		}

		public static void StoreR4(IntPtr address, uint offset, float value)
		{
			// TODO
		}

		public static void StoreR8(IntPtr address, double value)
		{
			// TODO
		}

		public static void StoreR8(IntPtr address, int offset, double value)
		{
			// TODO
		}

		public static void StoreR8(IntPtr address, uint offset, double value)
		{
			// TODO
		}

		public static void Store(IntPtr address, byte value)
		{
			CPU.Write8((ulong)address.ToInt64(), value);
		}

		public static void Store(IntPtr address, sbyte value)
		{
			CPU.Write8((ulong)address.ToInt64(), (byte)value);
		}

		public static void Store(IntPtr address, ushort value)
		{
		}

		public static void Store(IntPtr address, short value)
		{
			CPU.Write16((ulong)address.ToInt64(), (ushort)value);
		}

		public static void Store(IntPtr address, uint value)
		{
			CPU.Write32((ulong)address.ToInt64(), value);
		}

		public static void Store(IntPtr address, int value)
		{
			CPU.Write32((ulong)address.ToInt64(), (uint)value);
		}

		public static void Store(IntPtr address, ulong value)
		{
			CPU.Write64((ulong)address.ToInt64(), value);
		}

		public static void Store(IntPtr address, long value)
		{
			CPU.Write64((ulong)address.ToInt64(), (ulong)value);
		}

		public static void Store(IntPtr address, int offset, uint value)
		{
			CPU.Write32((ulong)(address.ToInt64() + offset), value);
		}

		public static void Store(IntPtr address, int offset, int value)
		{
			CPU.Write32((ulong)(address.ToInt64() + offset), (uint)value);
		}

		public static void Store(IntPtr address, uint offset, uint value)
		{
			CPU.Write32((ulong)(address.ToInt64() + offset), value);
		}

		public static void Store(IntPtr address, uint offset, int value)
		{
			CPU.Write32((ulong)(address.ToInt64() + offset), (uint)value);
		}

		public static void Store(IntPtr address, int offset, ulong value)
		{
			CPU.Write64((ulong)(address.ToInt64() + offset), value);
		}

		public static void Store(IntPtr address, int offset, long value)
		{
			CPU.Write64((ulong)(address.ToInt64() + offset), (ulong)value);
		}

		public static void Store(IntPtr address, uint offset, ulong value)
		{
			CPU.Write64((ulong)(address.ToInt64() + offset), value);
		}

		public static void Store(IntPtr address, uint offset, long value)
		{
			CPU.Write64((ulong)(address.ToInt64() + offset), (ulong)value);
		}

		public static void Store(IntPtr address, uint offset, IntPtr value)
		{
			if (CPU.Is32Bit)
				CPU.Write32((ulong)(address.ToInt64() + offset), (uint)value.ToInt32());
			else
				CPU.Write64((ulong)(address.ToInt64() + offset), (ulong)value.ToInt64());
		}

		public static void Store(IntPtr address, int offset, IntPtr value)
		{
			if (CPU.Is32Bit)
				CPU.Write32((ulong)(address.ToInt64() + offset), (uint)value.ToInt32());
			else
				CPU.Write64((ulong)(address.ToInt64() + offset), (ulong)value.ToInt64());
		}

		public static void Store(IntPtr address, ulong offset, IntPtr value)
		{
			if (CPU.Is32Bit)
				CPU.Write32((ulong)address.ToInt64() + offset, (uint)value.ToInt32());
			else
				CPU.Write64((ulong)address.ToInt64() + offset, (ulong)value.ToInt64());
		}

		public static void Store(IntPtr address, long offset, IntPtr value)
		{
			if (CPU.Is32Bit)
				CPU.Write32((ulong)(address.ToInt64() + offset), (uint)value.ToInt32());
			else
				CPU.Write64((ulong)(address.ToInt64() + offset), (ulong)value.ToInt64());
		}

		public static void StorePointer(IntPtr address, IntPtr value)
		{
			if (CPU.Is32Bit)
				CPU.Write32((ulong)address.ToInt64(), (uint)value.ToInt32());
			else
				CPU.Write64((ulong)address.ToInt64(), (ulong)value.ToInt64());
		}

		public static void StorePointer(IntPtr address, uint offset, IntPtr value)
		{
			if (CPU.Is32Bit)
				CPU.Write32((ulong)(address.ToInt64() + offset), (uint)value.ToInt32());
			else
				CPU.Write64((ulong)(address.ToInt64() + offset), (ulong)value.ToInt64());
		}

		public static void StorePointer(IntPtr address, int offset, IntPtr value)
		{
			if (CPU.Is32Bit)
				CPU.Write32((ulong)(address.ToInt64() + offset), (uint)value.ToInt32());
			else
				CPU.Write64((ulong)(address.ToInt64() + offset), (ulong)value.ToInt64());
		}

		public static void StorePointer(IntPtr address, ulong offset, IntPtr value)
		{
			if (CPU.Is32Bit)
				CPU.Write32((ulong)address.ToInt64() + offset, (uint)value.ToInt32());
			else
				CPU.Write64((ulong)address.ToInt64() + offset, (ulong)value.ToInt64());
		}

		public static void StorePointer(IntPtr address, long offset, IntPtr value)
		{
			if (CPU.Is32Bit)
				CPU.Write32((ulong)(address.ToInt64() + offset), (uint)value.ToInt32());
			else
				CPU.Write64((ulong)(address.ToInt64() + offset), (ulong)value.ToInt64());
		}

		#endregion Intrinsic
	}
}
