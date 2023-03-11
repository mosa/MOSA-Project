// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// GetLow32
/// </summary>
public sealed class GetLow32 : BaseIRTransform
{
	public GetLow32() : base(IRInstruction.GetLow32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.SplitLongOperand(context.Operand1, out var op0L, out var _);

		context.SetInstruction(X86.Mov32, context.Result, op0L);
	}
}
