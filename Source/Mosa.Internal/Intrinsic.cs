// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;

namespace Mosa.Internal
{
	/// <summary>
	/// Provides stub methods for selected native IR instructions.
	/// </summary>
	public static unsafe class Intrinsic
	{
		#region Intrinsic

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern uint Load32(uint address);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern uint Load32(uint address, int offset);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern uint Load32(uint address, uint offset);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern void* GetObjectAddress<T>(T obj) where T : class;

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern void* GetValueTypeAddress<T>(T obj) where T : struct;

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern object GetObjectFromAddress(void* address);

		#endregion Intrinsic
	}
}
