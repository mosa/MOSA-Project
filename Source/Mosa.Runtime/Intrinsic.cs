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
		public static extern byte Load8(uint address);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern byte Load8(uint address, int offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern byte Load8(uint address, uint offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern byte Load8(UIntPtr address);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern byte Load8(UIntPtr address, int offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern byte Load8(UIntPtr address, uint offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ushort Load16(uint address);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ushort Load16(uint address, int offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ushort Load16(uint address, uint offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ushort Load16(UIntPtr address);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ushort Load16(UIntPtr address, int offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ushort Load16(UIntPtr address, uint offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern uint Load32(UIntPtr address);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern uint Load32(uint address);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern uint Load32(uint address, int offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern uint Load32(uint address, uint offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern UIntPtr LoadPointer(UIntPtr address);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern UIntPtr LoadPointer(UIntPtr address, int offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern UIntPtr LoadPointer(UIntPtr address, uint offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern uint Load32(UIntPtr address, int offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern uint Load32(UIntPtr address, uint offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ulong Load64(uint address);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ulong Load64(uint address, int offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ulong Load64(uint address, uint offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ulong Load64(UIntPtr address);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ulong Load64(UIntPtr address, int offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern ulong Load64(UIntPtr address, uint offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store8(uint address, byte value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store8(uint address, int offset, byte value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store8(uint address, uint offset, byte value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store8(UIntPtr address, byte value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store8(UIntPtr address, int offset, byte value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store8(UIntPtr address, uint offset, byte value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store16(uint address, ushort value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store16(uint address, int offset, ushort value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store16(uint address, uint offset, ushort value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store16(UIntPtr address, ushort value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store16(UIntPtr address, int offset, ushort value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store16(UIntPtr address, uint offset, ushort value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store32(uint address, uint value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store32(uint address, int offset, uint value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store32(uint address, uint offset, uint value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store32(UIntPtr address, uint value);


		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store32(UIntPtr address, int offset, uint value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store32(UIntPtr address, int offset, int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store32(UIntPtr address, uint offset, uint value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store32(UIntPtr address, uint offset, int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store64(uint address, int offset, ulong value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store64(uint address, uint offset, ulong value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store64(uint address, ulong value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store64(UIntPtr address, ulong value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store64(UIntPtr address, int offset, ulong value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store64(UIntPtr address, uint offset, ulong value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store64(UIntPtr address, UIntPtr value);

		//[MethodImpl(MethodImplOptions.InternalCall)]
		//public static extern void Store64(UIntPtr address, int offset, UIntPtr value);

		//[MethodImpl(MethodImplOptions.InternalCall)]
		//public static extern void Store64(UIntPtr address, uint offset, UIntPtr value);

		//[MethodImpl(MethodImplOptions.InternalCall)]
		//public static extern void Store64(UIntPtr address, IntPtr value);

		//[MethodImpl(MethodImplOptions.InternalCall)]
		//public static extern void Store64(UIntPtr address, int offset, IntPtr value);

		//[MethodImpl(MethodImplOptions.InternalCall)]
		//public static extern void Store64(UIntPtr address, uint offset, IntPtr value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(UIntPtr address, byte value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(UIntPtr address, sbyte value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(UIntPtr address, ushort value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(UIntPtr address, short value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(UIntPtr address, uint value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(UIntPtr address, int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(UIntPtr address, ulong value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(UIntPtr address, long value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(UIntPtr address, int offset, uint value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(UIntPtr address, int offset, int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(UIntPtr address, uint offset, uint value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(UIntPtr address, uint offset, int value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(UIntPtr address, int offset, ulong value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(UIntPtr address, int offset, long value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(UIntPtr address, uint offset, ulong value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(UIntPtr address, uint offset, long value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(UIntPtr address, uint offset, UIntPtr value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(UIntPtr address, int offset, UIntPtr value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(UIntPtr address, ulong offset, UIntPtr value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(UIntPtr address, long offset, UIntPtr value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(UIntPtr address, long offset, IntPtr value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(UIntPtr address, uint offset, IntPtr value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(UIntPtr address, int offset, IntPtr value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Store(UIntPtr address, ulong offset, IntPtr value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void* GetObjectAddress<T>(T obj) where T : class;

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void* GetValueTypeAddress<T>(T obj) where T : struct;

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern object GetObjectFromAddress(void* address);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern object GetObjectFromAddress(UIntPtr address);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern static object CreateInstanceSimple(void* ctor, void* thisObject);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern static object CreateInstanceSimple(UIntPtr ctor, UIntPtr thisObject);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern static UIntPtr GetAssemblyListTable();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern static UIntPtr GetDelegateMethodAddress(Delegate d);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern static UIntPtr GetDelegateTargetAddress(Delegate d);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern static RuntimeTypeHandle GetStringType();

		#endregion Intrinsic
	}
}
