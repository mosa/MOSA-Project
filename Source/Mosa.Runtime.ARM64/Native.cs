// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.InteropServices;

namespace Mosa.Runtime.ARM64;

/// <summary>
/// Provides stub methods for selected x86 native assembly instructions.
/// </summary>
public static class Native
{
	#region Intrinsic

	[DllImport("Mosa.Compiler.ARM64.Intrinsic::Nop")]
	public static extern void Nop();

	#endregion Intrinsic
}
