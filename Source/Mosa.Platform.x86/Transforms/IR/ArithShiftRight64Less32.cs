// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Platform.x86.Transforms.IR;

/// <summary>
/// ArithShiftRight64Less32
/// </summary>
public sealed class ArithShiftRight64Less32 : BaseIRTransform
{
	public ArithShiftRight64Less32() : base(IRInstruction.ArithShiftRight64, TransformType.Manual | TransformType.Transform)
	{
	}

	public override int Priority => 10;

	public override bool Match(Context context, TransformContext transform)
	{
		return context.Operand2.IsResolvedConstant && context.Operand2.ConstantUnsigned32 <= 31;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		transform.SplitLongOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitLongOperand(context.Operand1, out var op1L, out var op1H);

		var count = context.Operand2;

		context.SetInstruction(X86.Shrd32, resultLow, op1L, op1H, count);
		context.AppendInstruction(X86.Sar32, resultHigh, op1H, count);
	}
}
