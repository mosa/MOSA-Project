// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::SetCR4")]
	private static void SetCR4(Context context, MethodCompiler methodCompiler)
	{
		Operand operand1 = context.Operand1;

		Operand eax = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.U4, CPURegister.RAX);
		Operand cr = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.U4, CPURegister.CR4);

		context.SetInstruction(X64.Mov64, eax, operand1);
		context.AppendInstruction(X64.MovCRStore64, null, cr, eax);
	}
}
