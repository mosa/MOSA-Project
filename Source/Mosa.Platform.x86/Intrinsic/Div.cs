// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Platform.x86.Intrinsic::Div")]
	private static void Div(Context context, MethodCompiler methodCompiler)
	{
		var n = context.Operand1;
		var d = context.Operand2;
		var result = context.Result;
		var result2 = methodCompiler.VirtualRegisters.Allocate32();

		methodCompiler.SplitOperand(n, out Operand op0L, out Operand op0H);

		context.SetInstruction2(X86.Div32, result2, result, op0H, op0L, d);
	}
}
