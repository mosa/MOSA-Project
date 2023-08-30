// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.IR;

/// <summary>
/// ShiftLeft64
/// </summary>
[Transform("ARM32.IR")]
public sealed class ShiftLeft64 : BaseIRTransform
{
	public ShiftLeft64() : base(IRInstruction.ShiftLeft64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.SplitOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitOperand(context.Operand1, out var op1L, out var op1H);

		//shiftleft64(long long, int):
		//		r0 (op1l), r1 (op1h), r2 (operand2)

		//mov	v1, op1l
		//sub	v2, operand2, #32
		//rsb	v3, operand2, #32
		//lsl	v4, op1L, op1L

		//orr	v5, v4, v4, lsl v2
		//lsl	resultLow, v1, operand2
		//orr	resultHigh, v5, v1, lsr v3

		var v1 = transform.VirtualRegisters.Allocate32();
		var v2 = transform.VirtualRegisters.Allocate32();
		var v3 = transform.VirtualRegisters.Allocate32();
		var v4 = transform.VirtualRegisters.Allocate32();
		var v5 = transform.VirtualRegisters.Allocate32();

		var operand2 = MoveConstantToRegister(transform, context, context.Operand2);

		op1L = MoveConstantToRegisterOrImmediate(transform, context, op1L);
		op1H = MoveConstantToRegister(transform, context, op1H);

		context.SetInstruction(ARM32.Mov, v1, op1L);
		context.AppendInstruction(ARM32.Sub, v2, operand2, Operand.Constant32_32);
		context.AppendInstruction(ARM32.Rsb, v3, operand2, Operand.Constant32_32);
		context.AppendInstruction(ARM32.Lsl, v4, op1H, operand2);

		context.AppendInstruction(ARM32.OrrRegShift, v5, v4, v4, v2, Operand.Constant32_0 /* LSL */);
		context.AppendInstruction(ARM32.Lsl, resultLow, v1, operand2);
		context.AppendInstruction(ARM32.OrrRegShift, resultHigh, v5, v1, v3, Operand.Constant32_1 /* LSR */);
	}
}
