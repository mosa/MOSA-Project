// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// AddCarryOut64
/// </summary>
[Transform("x86.IR")]
public sealed class AddCarryOut64 : BaseIRTransform
{
	public AddCarryOut64() : base(IRInstruction.AddCarryOut64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitLongOperand(context.Operand1, out var op1L, out var op1H);
		transform.SplitLongOperand(context.Operand2, out var op2L, out var op2H);
		var result2 = context.Result2;

		var v1 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(X86.Add32, resultLow, op1L, op2L);
		context.AppendInstruction(X86.Adc32, resultHigh, op1H, op2H);
		context.AppendInstruction(X86.Setcc, ConditionCode.Carry, v1);
		context.AppendInstruction(X86.Movzx8To32, result2, v1);
	}
}
