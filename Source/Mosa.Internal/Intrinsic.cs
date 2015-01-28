/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 *  Scott Balmos <sbalmos@fastmail.fm>
*/

using System.Runtime.InteropServices;
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
		public static extern void* GetObjectAddress(object obj);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern void* GetValueTypeAddress<T>(T obj) where T : struct;

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern object GetObjectFromAddress(void* address);

		#endregion Intrinsic
	}
}
