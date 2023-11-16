// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// Sub64
/// </summary>
[Transform]
public sealed class Sub64 : BaseIRTransform
{
	public Sub64() : base(IR.Sub64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.SplitOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitOperand(context.Operand1, out var op1L, out var op1H);
		transform.SplitOperand(context.Operand2, out var op2L, out var op2H);

		context.SetInstruction(X86.Sub32, resultLow, op1L, op2L);
		context.AppendInstruction(X86.Sbb32, resultHigh, op1H, op2H);
	}
}
