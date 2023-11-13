// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.ARM32.Transforms.BaseIR;

/// <summary>
/// IfThenElse64
/// </summary>
[Transform("ARM32.BaseIR")]
public sealed class IfThenElse64 : BaseIRTransform
{
	public IfThenElse64() : base(IR.IfThenElse64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.SplitOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitOperand(context.Operand1, out var op1L, out var op1H);
		transform.SplitOperand(context.Operand2, out var op2L, out var op2H);
		transform.SplitOperand(context.Operand3, out var op3L, out var op3H);

		var v1 = transform.VirtualRegisters.Allocate32();

		op1L = MoveConstantToRegister(transform, context, op1L);
		op1H = MoveConstantToRegisterOrImmediate(transform, context, op1H);
		op2L = MoveConstantToRegisterOrImmediate(transform, context, op2L);
		op2H = MoveConstantToRegisterOrImmediate(transform, context, op2H);
		op3L = MoveConstantToRegisterOrImmediate(transform, context, op3L);
		op3H = MoveConstantToRegisterOrImmediate(transform, context, op3H);

		context.SetInstruction(ARM32.Orr, v1, op1L, op1H);
		context.AppendInstruction(ARM32.Cmp, StatusRegister.Set, null, v1, Operand.Constant32_0);
		context.AppendInstruction(ARM32.Mov, ConditionCode.NotEqual, resultLow, resultLow, op2L);
		context.AppendInstruction(ARM32.Mov, ConditionCode.NotEqual, resultHigh, resultHigh, op2H);
		context.AppendInstruction(ARM32.Mov, ConditionCode.Equal, resultLow, resultLow, op3L);
		context.AppendInstruction(ARM32.Mov, ConditionCode.Equal, resultHigh, resultHigh, op3H);
	}
}
