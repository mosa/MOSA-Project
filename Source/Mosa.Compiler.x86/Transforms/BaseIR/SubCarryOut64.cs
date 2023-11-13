// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// SubCarryOut64
/// </summary>
[Transform("x86.BaseIR")]
public sealed class SubCarryOut64 : BaseIRTransform
{
	public SubCarryOut64() : base(Framework.IR.SubCarryOut64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.SplitOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitOperand(context.Operand1, out var op1L, out var op1H);
		transform.SplitOperand(context.Operand2, out var op2L, out var op2H);
		var result2 = context.Result2;

		var v1 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(X86.Sub32, resultLow, op1L, op2L);
		context.AppendInstruction(X86.Sbb32, resultHigh, op1H, op2H);
		context.AppendInstruction(X86.Setcc, ConditionCode.Carry, v1);
		context.AppendInstruction(X86.Movzx8To32, result2, v1);
	}
}
