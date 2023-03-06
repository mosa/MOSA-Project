// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// Not64
/// </summary>
public sealed class Not64 : BaseIRTransform
{
	public Not64() : base(IRInstruction.Not64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitLongOperand(context.Operand1, out var op1L, out var op1H);

		context.SetInstruction(X86.Not32, resultHigh, op1H);
		context.AppendInstruction(X86.Not32, resultLow, op1L);
	}
}
