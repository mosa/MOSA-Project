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
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		public static extern void InitializeArray(Array array, RuntimeFieldHandle fldHandle);
	}
}