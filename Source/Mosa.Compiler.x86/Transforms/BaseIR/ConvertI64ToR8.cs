// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// ConvertI64ToR8
/// </summary>
public sealed class ConvertI64ToR8 : BaseIRTransform
{
	public ConvertI64ToR8() : base(IR.ConvertI64ToR8, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.SplitOperand(context.Operand1, out var op1Low, out _);

		context.SetInstruction(X86.Cvtsi2sd32, context.Result, op1Low);
	}
}
