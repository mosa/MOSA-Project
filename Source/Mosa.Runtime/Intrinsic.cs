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
		public extern static object CreateInstanceSimple(Pointer ctor, Pointer thisObject);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern static Pointer GetAssemblyListTable();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern static Pointer GetDelegateMethodAddress(Delegate d);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern static Pointer GetDelegateTargetAddress(Delegate d);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern static Pointer GetMethodExceptionLookupTable();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern static Pointer GetMethodLookupTable();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Pointer GetObjectAddress<T>(T obj) where T : class;

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern object GetObjectFromAddress(Pointer address);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern static RuntimeTypeHandle GetStringType();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Pointer GetValueTypeAddress<T>(T obj) where T : struct;

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ushort Load16(Pointer address);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ushort Load16(Pointer address, int offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ushort Load16(Pointer address, uint offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern uint Load32(Pointer address);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern uint Load32(Pointer address, int offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern uint Load32(Pointer address, uint offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ulong Load64(Pointer address);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ulong Load64(Pointer address, int offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ulong Load64(Pointer address, uint offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern byte Load8(Pointer address);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern byte Load8(Pointer address, int offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern byte Load8(Pointer address, uint offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Pointer LoadPointer(Pointer address);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Pointer LoadPointer(Pointer address, int offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Pointer LoadPointer(Pointer address, uint offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float LoadR4(Pointer address);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float LoadR4(Pointer address, uint offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double LoadR8(Pointer address);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern double LoadR8(Pointer address, uint offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(Pointer address, byte value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(Pointer address, sbyte value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(Pointer address, ushort value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(Pointer address, short value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(Pointer address, uint value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(Pointer address, int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(Pointer address, ulong value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(Pointer address, long value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(Pointer address, int offset, uint value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(Pointer address, int offset, int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(Pointer address, uint offset, uint value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(Pointer address, uint offset, int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(Pointer address, int offset, ulong value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(Pointer address, int offset, long value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(Pointer address, uint offset, ulong value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(Pointer address, uint offset, long value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(Pointer address, uint offset, Pointer value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(Pointer address, int offset, Pointer value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(Pointer address, ulong offset, Pointer value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(Pointer address, long offset, Pointer value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store16(Pointer address, ushort value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store16(Pointer address, int offset, ushort value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store16(Pointer address, uint offset, ushort value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store32(Pointer address, uint value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store32(Pointer address, int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store32(Pointer address, int offset, uint value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store32(Pointer address, int offset, int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store32(Pointer address, uint offset, uint value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store32(Pointer address, uint offset, int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store64(Pointer address, ulong value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store64(Pointer address, int offset, ulong value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store64(Pointer address, uint offset, ulong value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store64(Pointer address, int offset, long value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store64(Pointer address, uint offset, long value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store64(Pointer address, Pointer value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store8(Pointer address, byte value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store8(Pointer address, int offset, byte value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store8(Pointer address, uint offset, byte value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void StorePointer(Pointer address, Pointer value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void StorePointer(Pointer address, uint offset, Pointer value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void StorePointer(Pointer address, int offset, Pointer value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void StorePointer(Pointer address, ulong offset, Pointer value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void StorePointer(Pointer address, long offset, Pointer value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void StoreR4(Pointer address, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void StoreR4(Pointer address, int offset, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void StoreR4(Pointer address, uint offset, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void StoreR8(Pointer address, double value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void StoreR8(Pointer address, int offset, double value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void StoreR8(Pointer address, uint offset, double value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern static void SuppressStackFrame();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern static Pointer GetStackFrame();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern static Pointer GetExceptionRegister();

		#endregion Intrinsic
	}
}
