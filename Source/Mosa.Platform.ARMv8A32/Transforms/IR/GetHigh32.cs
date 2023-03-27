// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.ARMv8A32.Transforms.IR;

/// <summary>
/// GetHigh32
/// </summary>
[Transform("ARMv8A32.IR")]
public sealed class GetHigh32 : BaseIRTransform
{
	public GetHigh32() : base(IRInstruction.GetHigh32, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.SplitLongOperand(context.Result, out var resultLow, out _);
		transform.SplitLongOperand(context.Operand1, out _, out var op1H);

		op1H = MoveConstantToRegisterOrImmediate(transform, context, op1H);

		context.SetInstruction(ARMv8A32.Mov, resultLow, op1H);
	}
}
