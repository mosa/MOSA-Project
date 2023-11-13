// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// Move64
/// </summary>
[Transform("ARM32.BaseIR")]
public sealed class Move64 : BaseIRTransform
{
	public Move64() : base(Framework.IR.Move64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.SplitOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitOperand(context.Operand1, out var op1L, out var op1H);

		op1L = MoveConstantToRegisterOrImmediate(transform, context, op1L);
		op1H = MoveConstantToRegisterOrImmediate(transform, context, op1H);

		context.SetInstruction(ARM32.Mov, resultLow, op1L);
		context.AppendInstruction(ARM32.Mov, resultHigh, op1H);
	}
}
