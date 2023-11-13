// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.IR;

/// <summary>
/// GetHigh32
/// </summary>
[Transform("x86.IR")]
public sealed class GetHigh32 : BaseIRTransform
{
	public GetHigh32() : base(Framework.IR.GetHigh32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.SplitOperand(context.Operand1, out var _, out var op0H);

		context.SetInstruction(X86.Mov32, context.Result, op0H);
	}
}
