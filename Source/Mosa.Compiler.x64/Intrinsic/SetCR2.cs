// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x64.Intrinsic::SetCR2")]
	private static void SetCR2(Context context, Transform transform)
	{
		var operand1 = context.Operand1;

		var rax = transform.PhysicalRegisters.Allocate64(CPURegister.RAX);
		var cr2 = transform.PhysicalRegisters.Allocate64(CPURegister.CR2);

		context.SetInstruction(X64.Mov64, rax, operand1);
		context.AppendInstruction(X64.MovCRStore64, null, cr2, rax);
	}
}
