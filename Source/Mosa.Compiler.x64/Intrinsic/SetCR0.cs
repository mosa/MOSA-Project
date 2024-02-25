// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Intrinsic;

/// <summary>
/// Intrinsic Methods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x64.Intrinsic::SetCR0")]
	private static void SetCR0(Context context, Transform transform)
	{
		var operand1 = context.Operand1;

		var rax = transform.PhysicalRegisters.Allocate64(CPURegister.RAX);
		var cr0 = transform.PhysicalRegisters.Allocate64(CPURegister.CR0);

		context.SetInstruction(X64.Mov64, rax, operand1);
		context.AppendInstruction(X64.MovCRStore64, null, cr0, rax);
	}
}
