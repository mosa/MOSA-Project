// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// IntrinsicMethods
	/// </summary>
	static partial class IntrinsicMethods
	{
		[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::Div")]
		private static void Div(Context context, MethodCompiler methodCompiler)
		{
			Operand n = context.Operand1;
			Operand d = context.Operand2;
			Operand result = context.Result;
			Operand result2 = methodCompiler.CreateVirtualRegister(methodCompiler.TypeSystem.BuiltIn.U4);

			methodCompiler.SplitLongOperand(n, out Operand op0L, out Operand op0H);

			context.SetInstruction2(X86.Div32, result2, result, op0H, op0L, d);
		}
	}
}
