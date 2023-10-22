// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.IR;

/// <summary>
/// Not64
/// </summary>
[Transform("x86.IR")]
public sealed class Not64 : BaseIRTransform
{
	public Not64() : base(IRInstruction.Not64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.SplitOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitOperand(context.Operand1, out var op1L, out var op1H);

		context.SetInstruction(X86.Not32, resultHigh, op1H);
		context.AppendInstruction(X86.Not32, resultLow, op1L);
	}
}
