// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// GetHigh32
/// </summary>
[Transform("ARM32.BaseIR")]
public sealed class GetHigh32 : BaseIRTransform
{
	public GetHigh32() : base(Framework.IR.GetHigh32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.SplitOperand(context.Result, out var resultLow, out _);
		transform.SplitOperand(context.Operand1, out _, out var op1H);

		op1H = MoveConstantToRegisterOrImmediate(transform, context, op1H);

		context.SetInstruction(ARM32.Mov, resultLow, op1H);
	}
}
