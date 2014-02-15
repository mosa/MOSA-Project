/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 */

using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	public static class RuntimeHelpers
	{
		[DllImport("Mosa.Compiler.Framework.Intrinsics.InternalInitializeArray, Mosa.Compiler.Framework")]
		public static extern void InitializeArray(Array array, RuntimeFieldHandle fldHandle);
	}
}