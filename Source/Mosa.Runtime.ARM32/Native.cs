// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.InteropServices;

namespace Mosa.Runtime.ARM32;

/// <summary>
/// Provides stub methods for selected x86 native assembly instructions.
/// </summary>
public static class Native
{
	#region Intrinsic

	[DllImport("Mosa.Compiler.ARM32.Intrinsic::Nop")]
	public static extern void Nop();

	[DllImport("Mosa.Compiler.ARM32.Intrinsic::Mcr")]
	public static extern uint Mcr(uint op1, uint op2, uint op3, uint op4, uint op5);

	[DllImport("Mosa.Compiler.ARM32.Intrinsic::Mrc")]
	public static extern uint Mrc(uint op1, uint op2, uint op3, uint op4, uint op5);

	#endregion Intrinsic
}
