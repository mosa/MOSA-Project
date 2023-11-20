// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// GetLow32
/// </summary>
public sealed class GetLow32 : BaseIRTransform
{
	public GetLow32() : base(IR.GetLow32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var op0 = transform.SplitOperand(context.Operand1);

		context.SetInstruction(X86.Mov32, context.Result, op0.Low);
	}
}
