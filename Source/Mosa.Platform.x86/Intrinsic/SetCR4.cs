// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::SetCR4")]
	private static void SetCR4(Context context, TransformContext transformContext)
	{
		Operand operand1 = context.Operand1;

		Operand eax = Operand.CreateCPURegister32(CPURegister.EAX);
		Operand cr = Operand.CreateCPURegister32(CPURegister.CR4);

		context.SetInstruction(X86.Mov32, eax, operand1);
		context.AppendInstruction(X86.MovCRStore32, null, cr, eax);
	}
}
