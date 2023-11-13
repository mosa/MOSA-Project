// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// ArithShiftRight64
/// </summary>
[Transform("ARM32.BaseIR")]
public sealed class ArithShiftRight64 : BaseIRTransform
{
	public ArithShiftRight64() : base(IR.ArithShiftRight64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.SplitOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitOperand(context.Operand1, out var op1L, out var op1H);
		transform.SplitOperand(context.Operand2, out var op2L, out var op2H);

		op1L = MoveConstantToRegister(transform, context, op1L);
		op1H = MoveConstantToRegister(transform, context, op1H);
		op2L = MoveConstantToRegister(transform, context, op2L);

		var v1 = transform.VirtualRegisters.Allocate32();
		var v2 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(ARM32.Asr, resultHigh, op1H, op2L);
		context.AppendInstruction(ARM32.Sub, StatusRegister.Set, v1, op2L, Operand.Constant32_0);
		context.AppendInstruction(ARM32.Lsr, resultLow, op1L, op2L);
		context.AppendInstruction(ARM32.Rsb, v2, op2L, Operand.Constant32_0);
		context.AppendInstruction(ARM32.Asr, ConditionCode.Positive, resultHigh, op1H, Operand.Constant32_31);
		context.AppendInstruction(ARM32.OrrRegShift, resultLow, resultLow, op1H, v2, Operand.Constant32_0 /* LSL */);
		context.AppendInstruction(ARM32.Asr, ConditionCode.Positive, resultLow, op1H, v1);
	}
}
