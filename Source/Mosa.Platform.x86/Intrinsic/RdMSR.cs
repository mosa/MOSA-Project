// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::RdMSR")]
	private static void RdMSR(Context context, TransformContext transformContext)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		//transformContext.SplitOperand(result, out Operand resultLow, out Operand resultHigh);

		var eax = Operand.CreateCPURegister32(CPURegister.EAX);
		var edx = Operand.CreateCPURegister32(CPURegister.EDX);
		var ecx = Operand.CreateCPURegister32(CPURegister.ECX);

		context.SetInstruction(X86.Mov32, ecx, operand1);
		context.AppendInstruction2(X86.RdMSR, eax, edx, ecx);
		context.AppendInstruction(IRInstruction.To64, result, edx, ecx);
	}
}
