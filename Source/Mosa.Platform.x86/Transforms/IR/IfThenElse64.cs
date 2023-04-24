// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// IfThenElse64
/// </summary>
[Transform("x86.IR")]
public sealed class IfThenElse64 : BaseIRTransform
{
	public IfThenElse64() : base(IRInstruction.IfThenElse64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.SplitOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitOperand(context.Operand1, out var op1L, out var op1H);
		transform.SplitOperand(context.Operand2, out var op2L, out var op2H);
		transform.SplitOperand(context.Operand3, out var op3L, out var op3H);

		var v1 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(X86.Or32, v1, op1L, op1H);
		context.AppendInstruction(X86.Cmp32, null, v1, Operand.Constant32_0);
		context.AppendInstruction(X86.CMov32, ConditionCode.NotEqual, resultLow, resultLow, op2L);     // true
		context.AppendInstruction(X86.CMov32, ConditionCode.NotEqual, resultHigh, resultHigh, op2H);   // true
		context.AppendInstruction(X86.CMov32, ConditionCode.Equal, resultLow, resultLow, op3L);        // false
		context.AppendInstruction(X86.CMov32, ConditionCode.Equal, resultHigh, resultHigh, op3H);      // false
	}
}
