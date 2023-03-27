// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// Or64
/// </summary>
[Transform("x86.IR")]
public sealed class Or64 : BaseIRTransform
{
	public Or64() : base(IRInstruction.Or64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitLongOperand(context.Operand1, out var op1L, out var op1H);
		transform.SplitLongOperand(context.Operand2, out var op2L, out var op2H);

		context.SetInstruction(X86.Or32, resultHigh, op1H, op2H);
		context.AppendInstruction(X86.Or32, resultLow, op1L, op2L);
	}
}
