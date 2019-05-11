// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Runtime.CompilerServices;

namespace Mosa.Runtime
{
	/// <summary>
	/// Provides stub methods for selected native IR instructions.
	/// </summary>
	public static unsafe class Intrinsic
	{
		#region Intrinsic

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern byte Load8(IntPtr address);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern byte Load8(IntPtr address, int offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern byte Load8(IntPtr address, uint offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ushort Load16(IntPtr address);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ushort Load16(IntPtr address, int offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ushort Load16(IntPtr address, uint offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern uint Load32(IntPtr address);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr LoadPointer(IntPtr address);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr LoadPointer(IntPtr address, int offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr LoadPointer(IntPtr address, uint offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern uint Load32(IntPtr address, int offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern uint Load32(IntPtr address, uint offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ulong Load64(IntPtr address);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ulong Load64(IntPtr address, int offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ulong Load64(IntPtr address, uint offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float LoadR4(IntPtr address);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float LoadR4(IntPtr address, uint offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double LoadR8(IntPtr address);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double LoadR8(IntPtr address, uint offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store8(IntPtr address, byte value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store8(IntPtr address, int offset, byte value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store8(IntPtr address, uint offset, byte value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store16(IntPtr address, ushort value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store16(IntPtr address, int offset, ushort value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store16(IntPtr address, uint offset, ushort value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store32(IntPtr address, uint value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store32(IntPtr address, int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store32(IntPtr address, int offset, uint value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store32(IntPtr address, int offset, int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store32(IntPtr address, uint offset, uint value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store32(IntPtr address, uint offset, int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store64(IntPtr address, ulong value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store64(IntPtr address, int offset, ulong value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store64(IntPtr address, uint offset, ulong value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store64(IntPtr address, int offset, long value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store64(IntPtr address, uint offset, long value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store64(IntPtr address, IntPtr value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void StoreR4(IntPtr address, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void StoreR4(IntPtr address, int offset, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void StoreR4(IntPtr address, uint offset, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void StoreR8(IntPtr address, double value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void StoreR8(IntPtr address, int offset, double value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void StoreR8(IntPtr address, uint offset, double value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(IntPtr address, byte value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(IntPtr address, sbyte value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(IntPtr address, ushort value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(IntPtr address, short value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(IntPtr address, uint value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(IntPtr address, int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(IntPtr address, ulong value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(IntPtr address, long value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(IntPtr address, int offset, uint value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(IntPtr address, int offset, int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(IntPtr address, uint offset, uint value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(IntPtr address, uint offset, int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(IntPtr address, int offset, ulong value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(IntPtr address, int offset, long value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(IntPtr address, uint offset, ulong value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(IntPtr address, uint offset, long value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(IntPtr address, uint offset, IntPtr value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(IntPtr address, int offset, IntPtr value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(IntPtr address, ulong offset, IntPtr value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(IntPtr address, long offset, IntPtr value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void StorePointer(IntPtr address, IntPtr value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void StorePointer(IntPtr address, uint offset, IntPtr value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void StorePointer(IntPtr address, int offset, IntPtr value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void StorePointer(IntPtr address, ulong offset, IntPtr value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void StorePointer(IntPtr address, long offset, IntPtr value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr GetObjectAddress<T>(T obj) where T : class;

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern IntPtr GetValueTypeAddress<T>(T obj) where T : struct;

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern object GetObjectFromAddress(IntPtr address);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern static object CreateInstanceSimple(IntPtr ctor, IntPtr thisObject);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern static IntPtr GetAssemblyListTable();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern static IntPtr GetDelegateMethodAddress(Delegate d);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern static IntPtr GetDelegateTargetAddress(Delegate d);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern static RuntimeTypeHandle GetStringType();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern static void SuppressStackFrame();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern static IntPtr GetMethodLookupTable();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern static IntPtr GetMethodExceptionLookupTable();

		#endregion Intrinsic
	}
}
