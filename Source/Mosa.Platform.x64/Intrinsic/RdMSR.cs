// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x64.Intrinsic::RdMSR")]
	private static void RdMSR(Context context, TransformContext transformContext)
	{
		var result = context.Result;
		var operand1 = context.Operand1;

		transformContext.SplitOperand(result, out Operand resultLow, out Operand resultHigh);

		var EAX = Operand.CreateCPURegister64(CPURegister.RAX);
		var EDX = Operand.CreateCPURegister64(CPURegister.RDX);
		var ECX = Operand.CreateCPURegister64(CPURegister.RAX);

		context.SetInstruction(X64.Mov64, ECX, operand1);
		context.AppendInstruction2(X64.RdMSR, EAX, EDX, ECX);
		context.AppendInstruction(X64.Mov64, resultLow, EAX);
		context.AppendInstruction(X64.Mov64, resultHigh, EDX);
	}
}
