// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x64.Intrinsic::AllocateStackSpace")]
	private static void AllocateStackSpace(Context context, Transform transform)
	{
		Operand result = context.Result;
		Operand size = context.Operand1;

		Operand esp = transform.PhysicalRegisters.Allocate64(CPURegister.RSP);

		context.SetInstruction(X64.Sub64, esp, esp, size);
		context.AppendInstruction(X64.Mov64, result, esp);
	}
}
