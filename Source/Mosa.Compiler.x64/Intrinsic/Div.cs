// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Intrinsic;

/// <summary>
/// IntrinsicMethods
/// </summary>
internal static partial class IntrinsicMethods
{
	[IntrinsicMethod("Mosa.Compiler.x64.Intrinsic::Div")]
	private static void Div(Context context, TransformContext transformContext)
	{
		var n = context.Operand1;
		var d = context.Operand2;
		var result = context.Result;
		var result2 = transformContext.VirtualRegisters.Allocate64();

		transformContext.SplitOperand(n, out Operand op0L, out Operand op0H);

		context.SetInstruction2(X64.Div64, result2, result, op0H, op0L, d);
	}
}
