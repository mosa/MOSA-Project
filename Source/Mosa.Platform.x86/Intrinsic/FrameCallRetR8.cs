// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::FrameCallRetR8")]
	private static void FrameCallRetR8(Context context, MethodCompiler methodCompiler)
	{
		var result = context.Result;
		var methodAddress = context.Operand1;

		var eax = Operand.CreateCPURegister32(CPURegister.EAX);
		var edx = Operand.CreateCPURegister32(CPURegister.EDX);
		var xmm0 = Operand.CreateCPURegister32(CPURegister.XMM0);

		methodCompiler.SplitOperand(result, out Operand op0L, out Operand op0H);

		context.SetInstruction(X86.Call, null, methodAddress);
		context.AppendInstruction(IRInstruction.Gen, xmm0);

		context.AppendInstruction(X86.Movdi32ss, eax, xmm0);    // CHECK
		context.AppendInstruction(X86.Pextrd32, edx, xmm0, Operand.Constant32_1);

		context.AppendInstruction(X86.Mov32, op0L, eax);
		context.AppendInstruction(X86.Mov32, op0H, edx);
	}
}
