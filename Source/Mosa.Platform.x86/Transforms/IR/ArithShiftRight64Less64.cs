// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// ArithShiftRight64Less64
/// </summary>
public sealed class ArithShiftRight64Less64 : BaseIRTransform
{
	public ArithShiftRight64Less64() : base(IRInstruction.ArithShiftRight64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => 10;

	public override bool Match(Context context, TransformContext transform)
	{
		return context.Operand2.IsResolvedConstant && context.Operand2.ConstantUnsigned32 >= 32 && context.Operand2.ConstantUnsigned32 <= 63;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitLongOperand(context.Operand1, out var op1L, out var op1H);

		var count = context.Operand2;

		var v1_count = transform.AllocateVirtualRegister32();
		var v2 = transform.AllocateVirtualRegister32();
		var v3 = transform.AllocateVirtualRegister32();

		context.SetInstruction(X86.Mov32, v1_count, count);
		context.AppendInstruction(X86.Mov32, v2, op1H);
		context.AppendInstruction(X86.Sar32, resultHigh, op1H, transform.Constant32_31);
		context.AppendInstruction(X86.And32, v3, v1_count, transform.Constant32_31);
		context.AppendInstruction(X86.Sar32, resultLow, v2, v3);
	}
}
