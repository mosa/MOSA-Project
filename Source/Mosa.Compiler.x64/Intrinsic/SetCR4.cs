// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Intrinsic;

/// <summary>
/// Intrinsic Methods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x64.Intrinsic::SetCR4")]
	private static void SetCR4(Context context, Transform transform)
	{
		var operand1 = context.Operand1;

		var rax = transform.PhysicalRegisters.Allocate64(CPURegister.RAX);
		var cr4 = transform.PhysicalRegisters.Allocate64(CPURegister.CR4);

		context.SetInstruction(X64.Mov64, rax, operand1);
		context.AppendInstruction(X64.MovCRStore64, null, cr4, rax);
	}
}
