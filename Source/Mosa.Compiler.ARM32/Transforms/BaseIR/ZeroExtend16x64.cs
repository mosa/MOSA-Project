// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// ZeroExtend16x64
/// </summary>
[Transform("ARM32.BaseIR")]
public sealed class ZeroExtend16x64 : BaseIRTransform
{
	public ZeroExtend16x64() : base(IR.ZeroExtend16x64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.SplitOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitOperand(context.Operand1, out var op1L, out _);

		op1L = MoveConstantToRegisterOrImmediate(transform, context, op1L);

		var operand1 = MoveConstantToRegister(transform, context, Operand.Constant32_FFFF);

		context.SetInstruction(ARM32.Mov, resultLow, op1L);
		context.AppendInstruction(ARM32.And, resultLow, resultLow, operand1);
		context.AppendInstruction(ARM32.Mov, resultHigh, Operand.Constant32_0);
	}
}
