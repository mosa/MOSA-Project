/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
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

		#endregion Intrinsic
	}
}