// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// ConvertI64ToR4
/// </summary>
public sealed class ConvertI64ToR4 : BaseIRTransform
{
	public ConvertI64ToR4() : base(IR.ConvertI64ToR4, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.SplitOperand(context.Result, out var op1Low, out _);

		context.SetInstruction(X86.Cvtsi2ss32, context.Result, op1Low);
	}
}
