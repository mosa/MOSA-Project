// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// Move64
/// </summary>
public sealed class Move64 : BaseIRTransform
{
	public Move64() : base(IR.Move64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.SplitOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitOperand(context.Operand1, out var op1L, out var op1H);

		context.SetInstruction(X86.Mov32, resultLow, op1L);
		context.AppendInstruction(X86.Mov32, resultHigh, op1H);
	}
}
