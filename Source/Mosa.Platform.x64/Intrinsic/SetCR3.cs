// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::SetCR3")]
	private static void SetCR3(Context context, TransformContext transformContext)
	{
		Operand operand1 = context.Operand1;

		Operand eax = Operand.CreateCPURegister32(CPURegister.RAX);
		Operand cr = Operand.CreateCPURegister32(CPURegister.CR3);

		context.SetInstruction(X64.Mov64, eax, operand1);
		context.AppendInstruction(X64.MovCRStore64, null, cr, eax);
	}
}
