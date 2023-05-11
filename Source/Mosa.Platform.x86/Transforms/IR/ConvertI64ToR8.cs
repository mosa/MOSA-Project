// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// ConvertI64ToR8
/// </summary>
[Transform("x86.IR")]
public sealed class ConvertI64ToR8 : BaseIRTransform
{
	public ConvertI64ToR8() : base(IRInstruction.ConvertI64ToR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.SplitOperand(context.Operand1, out var op1Low, out _);

		context.SetInstruction(X86.Cvtsi2sd32, context.Result, op1Low);
	}
}
