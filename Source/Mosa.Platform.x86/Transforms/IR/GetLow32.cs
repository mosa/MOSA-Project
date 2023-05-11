// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// GetLow32
/// </summary>
[Transform("x86.IR")]
public sealed class GetLow32 : BaseIRTransform
{
	public GetLow32() : base(IRInstruction.GetLow32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var op0 = transform.SplitOperand(context.Operand1);

		context.SetInstruction(X86.Mov32, context.Result, op0.Low);
	}
}
