// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Platform.x86.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Compiler.Platform.x86.Intrinsic::RdMSR")]
		private static void RdMSR(Context context, MethodCompiler methodCompiler)
		{
			var result = context.Result;
			var operand1 = context.Operand1;

			methodCompiler.SplitLongOperand(result, out Operand resultLow, out Operand resultHigh);

			var EAX = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, CPURegister.EAX);
			var EDX = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, CPURegister.EDX);
			var ECX = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I4, CPURegister.ECX);

			context.SetInstruction(X86.Mov32, ECX, operand1);
			context.AppendInstruction2(X86.RdMSR, EAX, EDX, ECX);
			context.AppendInstruction(X86.Mov32, resultLow, EAX);
			context.AppendInstruction(X86.Mov32, resultHigh, EDX);
		}
	}
}
