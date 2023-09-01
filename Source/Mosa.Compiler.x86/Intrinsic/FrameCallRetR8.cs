// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x86.Intrinsic::FrameCallRetR8")]
	private static void FrameCallRetR8(Context context, TransformContext transformContext)
	{
		var result = context.Result;
		var methodAddress = context.Operand1;

		var eax = Operand.CreateCPURegister32(CPURegister.EAX);
		var edx = Operand.CreateCPURegister32(CPURegister.EDX);
		var xmm0 = Operand.CreateCPURegister32(CPURegister.XMM0);

		context.SetInstruction(X86.Call, null, methodAddress);
		context.AppendInstruction(IRInstruction.Gen, xmm0);
		context.AppendInstruction(X86.Movdi32ss, eax, xmm0);    // CHECK
		context.AppendInstruction(X86.Pextrd32, edx, xmm0, Operand.Constant32_1);
		context.AppendInstruction(IRInstruction.To64, result, eax, edx);
	}
}
