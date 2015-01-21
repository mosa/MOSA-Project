/*
 * (c) 2014 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Stefan Andres Charsley (charsleysa) <charsleysa@gmail.com>
*/

using System.Runtime.CompilerServices;

namespace Mosa.Internal
{
	/// <summary>
	/// Provides stub methods for selected x86 native assembly instructions.
	/// </summary>
	public static unsafe class Native
	{
		#region Intrinsic

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern uint Load32(uint address);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern uint Load32(uint address, int offset);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern uint Load32(uint address, uint offset);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern void* GetObjectAddress(object obj);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern void* GetValueTypeAddress<T>(T obj) where T : struct;

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern object GetObjectFromAddress(void* address);

		#endregion Intrinsic
	}
}