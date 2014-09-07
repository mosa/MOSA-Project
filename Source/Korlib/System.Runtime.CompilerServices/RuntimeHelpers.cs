/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

namespace System.Runtime.CompilerServices
{
	public static class RuntimeHelpers
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void InitializeArray(Array array, RuntimeFieldHandle fldHandle);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetHashCode(Object o);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public new static extern bool Equals(Object o1, Object o2);
	}
}