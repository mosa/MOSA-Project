// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Platform.x86.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	internal static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Compiler.Platform.x86.Intrinsic::FrameCallRetR8")]
		private static void FrameCallRetR8(Context context, MethodCompiler methodCompiler)
		{
			var result = context.Result;
			var methodAddress = context.Operand1;

			var eax = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, CPURegister.EAX);
			var edx = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, CPURegister.EDX);
			var xmm0 = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, CPURegister.XMM0);

			methodCompiler.SplitLongOperand(result, out Operand op0L, out Operand op0H);

			context.SetInstruction(X86.Call, null, methodAddress);
			context.AppendInstruction(IRInstruction.Gen, xmm0);

			context.AppendInstruction(X86.Movdi32ss, eax, xmm0);    // CHECK
			context.AppendInstruction(X86.Pextrd32, edx, xmm0, methodCompiler.CreateConstant((byte)1));

			context.AppendInstruction(X86.Mov32, op0L, eax);
			context.AppendInstruction(X86.Mov32, op0H, edx);
		}
	}
}
