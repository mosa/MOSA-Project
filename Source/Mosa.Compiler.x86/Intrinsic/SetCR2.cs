// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x86.Intrinsic::SetCR2")]
	private static void SetCR2(Context context, Transform transform)
	{
		Operand operand1 = context.Operand1;

		Operand eax = transform.PhysicalRegisters.Allocate32(CPURegister.EAX);
		Operand cr = transform.PhysicalRegisters.Allocate32(CPURegister.CR2);

		context.SetInstruction(X86.Mov32, eax, operand1);
		context.AppendInstruction(X86.MovCRStore32, null, cr, eax);
	}
}
