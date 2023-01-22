// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Platform.x64.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	internal static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Compiler.Platform.x64.Intrinsic::RdMSR")]
		private static void RdMSR(Context context, MethodCompiler methodCompiler)
		{
			var result = context.Result;
			var operand1 = context.Operand1;

			methodCompiler.SplitLongOperand(result, out Operand resultLow, out Operand resultHigh);

			var EAX = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I8, CPURegister.RAX);
			var EDX = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I8, CPURegister.RDX);
			var ECX = Operand.CreateCPURegister(methodCompiler.TypeSystem.BuiltIn.I8, CPURegister.RAX);

			context.SetInstruction(X64.Mov64, ECX, operand1);
			context.AppendInstruction2(X64.RdMSR, EAX, EDX, ECX);
			context.AppendInstruction(X64.Mov64, resultLow, EAX);
			context.AppendInstruction(X64.Mov64, resultHigh, EDX);
		}
	}
}
