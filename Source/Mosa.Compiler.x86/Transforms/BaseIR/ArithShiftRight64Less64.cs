// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// ArithShiftRight64Less64
/// </summary>
[Transform]
public sealed class ArithShiftRight64Less64 : BaseIRTransform
{
	public ArithShiftRight64Less64() : base(IR.ArithShiftRight64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => 10;

	public override bool Match(Context context, Transform transform)
	{
		return context.Operand2.IsResolvedConstant && context.Operand2.ConstantUnsigned32 >= 32 && context.Operand2.ConstantUnsigned32 <= 63;
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.SplitOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitOperand(context.Operand1, out var op1L, out var op1H);

		var count = context.Operand2;

		var v1_count = transform.VirtualRegisters.Allocate32();
		var v2 = transform.VirtualRegisters.Allocate32();
		var v3 = transform.VirtualRegisters.Allocate32();

		context.SetInstruction(X86.Mov32, v1_count, count);
		context.AppendInstruction(X86.Mov32, v2, op1H);
		context.AppendInstruction(X86.Sar32, resultHigh, op1H, Operand.Constant32_31);
		context.AppendInstruction(X86.And32, v3, v1_count, Operand.Constant32_31);
		context.AppendInstruction(X86.Sar32, resultLow, v2, v3);
	}
}
