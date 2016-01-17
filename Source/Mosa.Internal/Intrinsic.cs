// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Mosa.Internal
{
	/// <summary>
	/// Provides stub methods for selected native IR instructions.
	/// </summary>
	public static unsafe class Intrinsic
	{
		#region Intrinsic

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern uint Load32(uint address);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern uint Load32(uint address, int offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern uint Load32(uint address, uint offset);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void* GetObjectAddress<T>(T obj) where T : class;

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void* GetValueTypeAddress<T>(T obj) where T : struct;

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern object GetObjectFromAddress(void* address);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern static object CreateInstanceSimple(void* ctor, void* thisObject);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern static uint* GetAssemblyListTable();

		#endregion Intrinsic
	}
}
