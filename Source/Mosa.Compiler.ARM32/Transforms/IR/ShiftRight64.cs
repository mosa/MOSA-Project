// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// ShiftRight64
/// </summary>
[Transform("ARM32.IR")]
public sealed class ShiftRight64 : BaseIRTransform
{
	public ShiftRight64() : base(IRInstruction.ShiftRight64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.SplitOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitOperand(context.Operand1, out var op1L, out var op1H);
		transform.SplitOperand(context.Operand2, out var op2L, out var op2H);

		//shiftright64(long long, int):
		//		r0 (op1l), r1 (op1h), r2 (operand2)

		//rsb	v1, operand2, #32
		//subs	v2, operand2, #32
		//lsr	v3, op1l, operand2

		//orr	v4, v3, op1h, lsl v1
		//orrpl	resultLow, v4, op1h, asr v2

		//asr	resultHigh, op1h, operand2

		var v1 = transform.VirtualRegisters.Allocate32();
		var v2 = transform.VirtualRegisters.Allocate32();
		var v3 = transform.VirtualRegisters.Allocate32();
		var v4 = transform.VirtualRegisters.Allocate32();

		op1L = MoveConstantToRegister(transform, context, op1L);
		op1H = MoveConstantToRegister(transform, context, op1H);
		op2L = MoveConstantToRegister(transform, context, op2L);

		context.SetInstruction(ARM32.Rsb, v1, op2L, Operand.Constant32_32);
		context.AppendInstruction(ARM32.Sub, StatusRegister.Set, v2, op2L, Operand.Constant32_32);
		context.AppendInstruction(ARM32.Lsr, v3, op1L, op2L);
		context.AppendInstruction(ARM32.OrrRegShift, v4, v3, op1H, v1, Operand.Constant32_0 /* LSL */);
		context.AppendInstruction(ARM32.OrrRegShift, ConditionCode.Zero, resultLow, v4, op1H, v2, Operand.Constant32_2 /* ASR */);
		context.AppendInstruction(ARM32.Asr, resultHigh, op1H, op2L);
	}
}
