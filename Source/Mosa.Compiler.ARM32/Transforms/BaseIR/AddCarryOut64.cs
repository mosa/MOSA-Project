// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// AddCarryOut64
/// </summary>
public sealed class AddCarryOut64 : BaseIRTransform
{
	public AddCarryOut64() : base(IR.AddCarryOut64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		var result2 = context.Result2;

		transform.SplitOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitOperand(context.Operand1, out var op1L, out var op1H);
		transform.SplitOperand(context.Operand2, out var op2L, out var op2H);

		context.SetInstruction(ARM32.Add, InstructionOption.SetFlags, resultLow, op1L, op2L);
		context.AppendInstruction(ARM32.Adc, InstructionOption.SetFlags, resultHigh, op1H, op2H);
		context.AppendInstruction(ARM32.Mov, ConditionCode.Carry, result2, Operand.Constant32_1);
		context.AppendInstruction(ARM32.Mov, ConditionCode.NoCarry, result2, Operand.Constant32_0);
	}
}
