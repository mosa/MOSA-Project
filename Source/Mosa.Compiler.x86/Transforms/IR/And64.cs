// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.IR;

/// <summary>
/// And64
/// </summary>
[Transform("x86.IR")]
public sealed class And64 : BaseIRTransform
{
	public And64() : base(IRInstruction.And64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.SplitOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitOperand(context.Operand1, out var op1L, out var op1H);
		transform.SplitOperand(context.Operand2, out var op2L, out var op2H);

		context.SetInstruction(X86.And32, resultHigh, op1H, op2H);
		context.AppendInstruction(X86.And32, resultLow, op1L, op2L);
	}
}
