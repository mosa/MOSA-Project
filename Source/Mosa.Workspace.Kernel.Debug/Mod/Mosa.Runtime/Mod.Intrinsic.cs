// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Workspace.Kernel.Internal;

namespace Mosa.Runtime
{
	/// <summary>
	/// Provides stub methods for selected native IR instructions.
	/// </summary>
	public static class Intrinsic
	{
		#region Intrinsic

		public static byte Load8(Pointer address)
		{
			return CPU.Read8((ulong)address.ToInt64());
		}

		public static byte Load8(Pointer address, int offset)
		{
			return CPU.Read8((ulong)(address.ToInt64() + offset));
		}

		public static byte Load8(Pointer address, uint offset)
		{
			return CPU.Read8((ulong)(address.ToInt64() + offset));
		}

		public static ushort Load16(Pointer address)
		{
			return CPU.Read16((ulong)address.ToInt64());
		}

		public static ushort Load16(Pointer address, int offset)
		{
			return CPU.Read16((ulong)(address.ToInt64() + offset));
		}

		public static ushort Load16(Pointer address, uint offset)
		{
			return CPU.Read16((ulong)(address.ToInt64() + offset));
		}

		public static uint Load32(Pointer address)
		{
			return CPU.Read32((ulong)address.ToInt64());
		}

		public static Pointer LoadPointer(Pointer address)
		{
			if (CPU.Is32Bit)
				return new Pointer(Load32(address));
			else
				return new Pointer((long)Load64(address));
		}

		public static Pointer LoadPointer(Pointer address, int offset)
		{
			if (CPU.Is32Bit)
				return new Pointer(Load32(address, offset));
			else
				return new Pointer((long)Load64(address, offset));
		}

		public static Pointer LoadPointer(Pointer address, uint offset)
		{
			if (CPU.Is32Bit)
				return new Pointer(Load32(address, offset));
			else
				return new Pointer((long)Load64(address, offset));
		}

		public static uint Load32(Pointer address, int offset)
		{
			return CPU.Read32((ulong)(address.ToInt64() + offset));
		}

		public static uint Load32(Pointer address, uint offset)
		{
			return CPU.Read32((ulong)(address.ToInt64() + offset));
		}

		public static ulong Load64(Pointer address)
		{
			return CPU.Read64((ulong)address.ToInt64());
		}

		public static ulong Load64(Pointer address, int offset)
		{
			return CPU.Read64((ulong)(address.ToInt64() + offset));
		}

		public static ulong Load64(Pointer address, uint offset)
		{
			return CPU.Read64((ulong)(address.ToInt64() + offset));
		}

		public static float LoadR4(Pointer address)
		{
			// TODO
			return 0;
		}

		public static float LoadR4(Pointer address, uint offset)
		{
			//TODO
			return 0;
		}

		public static double LoadR8(Pointer address)
		{
			//TODO
			return 0;
		}

		public static double LoadR8(Pointer address, uint offset)
		{
			//TODO
			return 0;
		}

		public static void Store8(Pointer address, byte value)
		{
			CPU.Write8((ulong)address.ToInt64(), value);
		}

		public static void Store8(Pointer address, int offset, byte value)
		{
			CPU.Write8((ulong)(address.ToInt64() + offset), value);
		}

		public static void Store8(Pointer address, uint offset, byte value)
		{
			CPU.Write8((ulong)(address.ToInt64() + offset), value);
		}

		public static void Store16(Pointer address, ushort value)
		{
			CPU.Write16((ulong)address.ToInt64(), value);
		}

		public static void Store16(Pointer address, int offset, ushort value)
		{
			CPU.Write16((ulong)(address.ToInt64() + offset), value);
		}

		public static void Store16(Pointer address, uint offset, ushort value)
		{
			CPU.Write16((ulong)(address.ToInt64() + offset), value);
		}

		public static void Store32(Pointer address, uint value)
		{
			CPU.Write32((ulong)address.ToInt64(), value);
		}

		public static void Store32(Pointer address, int value)
		{
			CPU.Write32((ulong)address.ToInt64(), (uint)value);
		}

		public static void Store32(Pointer address, int offset, uint value)
		{
			CPU.Write32((ulong)(address.ToInt64() + offset), value);
		}

		public static void Store32(Pointer address, int offset, int value)
		{
			CPU.Write32((ulong)(address.ToInt64() + offset), (uint)value);
		}

		public static void Store32(Pointer address, uint offset, uint value)
		{
			CPU.Write32((ulong)(address.ToInt64() + offset), value);
		}

		public static void Store32(Pointer address, uint offset, int value)
		{
			CPU.Write32((ulong)(address.ToInt64() + offset), (uint)value);
		}

		public static void Store64(Pointer address, ulong value)
		{
			CPU.Write64((ulong)address.ToInt64(), value);
		}

		public static void Store64(Pointer address, int offset, ulong value)
		{
			CPU.Write64((ulong)(address.ToInt64() + offset), value);
		}

		public static void Store64(Pointer address, uint offset, ulong value)
		{
			CPU.Write64((ulong)(address.ToInt64() + offset), value);
		}

		public static void Store64(Pointer address, int offset, long value)
		{
			CPU.Write64((ulong)(address.ToInt64() + offset), (ulong)value);
		}

		public static void Store64(Pointer address, uint offset, long value)
		{
			CPU.Write64((ulong)(address.ToInt64() + offset), (ulong)value);
		}

		public static void Store64(Pointer address, Pointer value)
		{
			if (CPU.Is32Bit)
				CPU.Write64((ulong)address.ToInt64(), (uint)value.ToInt64());
			else
				CPU.Write64((ulong)address.ToInt64(), (ulong)value.ToInt64());
		}

		public static void StoreR4(Pointer address, float value)
		{
			// TODO
		}

		public static void StoreR4(Pointer address, int offset, float value)
		{
			// TODO
		}

		public static void StoreR4(Pointer address, uint offset, float value)
		{
			// TODO
		}

		public static void StoreR8(Pointer address, double value)
		{
			// TODO
		}

		public static void StoreR8(Pointer address, int offset, double value)
		{
			// TODO
		}

		public static void StoreR8(Pointer address, uint offset, double value)
		{
			// TODO
		}

		public static void Store(Pointer address, byte value)
		{
			CPU.Write8((ulong)address.ToInt64(), value);
		}

		public static void Store(Pointer address, sbyte value)
		{
			CPU.Write8((ulong)address.ToInt64(), (byte)value);
		}

		public static void Store(Pointer address, ushort value)
		{
		}

		public static void Store(Pointer address, short value)
		{
			CPU.Write16((ulong)address.ToInt64(), (ushort)value);
		}

		public static void Store(Pointer address, uint value)
		{
			CPU.Write32((ulong)address.ToInt64(), value);
		}

		public static void Store(Pointer address, int value)
		{
			CPU.Write32((ulong)address.ToInt64(), (uint)value);
		}

		public static void Store(Pointer address, ulong value)
		{
			CPU.Write64((ulong)address.ToInt64(), value);
		}

		public static void Store(Pointer address, long value)
		{
			CPU.Write64((ulong)address.ToInt64(), (ulong)value);
		}

		public static void Store(Pointer address, int offset, uint value)
		{
			CPU.Write32((ulong)(address.ToInt64() + offset), value);
		}

		public static void Store(Pointer address, int offset, int value)
		{
			CPU.Write32((ulong)(address.ToInt64() + offset), (uint)value);
		}

		public static void Store(Pointer address, uint offset, uint value)
		{
			CPU.Write32((ulong)(address.ToInt64() + offset), value);
		}

		public static void Store(Pointer address, uint offset, int value)
		{
			CPU.Write32((ulong)(address.ToInt64() + offset), (uint)value);
		}

		public static void Store(Pointer address, int offset, ulong value)
		{
			CPU.Write64((ulong)(address.ToInt64() + offset), value);
		}

		public static void Store(Pointer address, int offset, long value)
		{
			CPU.Write64((ulong)(address.ToInt64() + offset), (ulong)value);
		}

		public static void Store(Pointer address, uint offset, ulong value)
		{
			CPU.Write64((ulong)(address.ToInt64() + offset), value);
		}

		public static void Store(Pointer address, uint offset, long value)
		{
			CPU.Write64((ulong)(address.ToInt64() + offset), (ulong)value);
		}

		public static void Store(Pointer address, uint offset, Pointer value)
		{
			if (CPU.Is32Bit)
				CPU.Write32((ulong)(address.ToInt64() + offset), (uint)value.ToInt64());
			else
				CPU.Write64((ulong)(address.ToInt64() + offset), (ulong)value.ToInt64());
		}

		public static void Store(Pointer address, int offset, Pointer value)
		{
			if (CPU.Is32Bit)
				CPU.Write32((ulong)(address.ToInt64() + offset), (uint)value.ToInt64());
			else
				CPU.Write64((ulong)(address.ToInt64() + offset), (ulong)value.ToInt64());
		}

		public static void Store(Pointer address, ulong offset, Pointer value)
		{
			if (CPU.Is32Bit)
				CPU.Write32((ulong)address.ToInt64() + offset, (uint)value.ToInt64());
			else
				CPU.Write64((ulong)address.ToInt64() + offset, (ulong)value.ToInt64());
		}

		public static void Store(Pointer address, long offset, Pointer value)
		{
			if (CPU.Is32Bit)
				CPU.Write32((ulong)(address.ToInt64() + offset), (uint)value.ToInt64());
			else
				CPU.Write64((ulong)(address.ToInt64() + offset), (ulong)value.ToInt64());
		}

		public static void StorePointer(Pointer address, Pointer value)
		{
			if (CPU.Is32Bit)
				CPU.Write32((ulong)address.ToInt64(), (uint)value.ToInt64());
			else
				CPU.Write64((ulong)address.ToInt64(), (ulong)value.ToInt64());
		}

		public static void StorePointer(Pointer address, uint offset, Pointer value)
		{
			if (CPU.Is32Bit)
				CPU.Write32((ulong)(address.ToInt64() + offset), (uint)value.ToInt64());
			else
				CPU.Write64((ulong)(address.ToInt64() + offset), (ulong)value.ToInt64());
		}

		public static void StorePointer(Pointer address, int offset, Pointer value)
		{
			if (CPU.Is32Bit)
				CPU.Write32((ulong)(address.ToInt64() + offset), (uint)value.ToInt64());
			else
				CPU.Write64((ulong)(address.ToInt64() + offset), (ulong)value.ToInt64());
		}

		public static void StorePointer(Pointer address, ulong offset, Pointer value)
		{
			if (CPU.Is32Bit)
				CPU.Write32((ulong)address.ToInt64() + offset, (uint)value.ToInt64());
			else
				CPU.Write64((ulong)address.ToInt64() + offset, (ulong)value.ToInt64());
		}

		public static void StorePointer(Pointer address, long offset, Pointer value)
		{
			if (CPU.Is32Bit)
				CPU.Write32((ulong)(address.ToInt64() + offset), (uint)value.ToInt64());
			else
				CPU.Write64((ulong)(address.ToInt64() + offset), (ulong)value.ToInt64());
		}

		#endregion Intrinsic
	}
}
