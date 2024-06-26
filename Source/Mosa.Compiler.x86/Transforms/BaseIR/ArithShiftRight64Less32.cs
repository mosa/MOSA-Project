// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x86.Transforms.BaseIR;

/// <summary>
/// ArithShiftRight64Less32
/// </summary>
public sealed class ArithShiftRight64Less32 : BaseIRTransform
{
	public ArithShiftRight64Less32() : base(IR.ArithShiftRight64, TransformType.Manual | TransformType.Transform, 10)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		return context.Operand2.IsResolvedConstant && context.Operand2.ConstantUnsigned32 <= 31;
	}

	public override void Transform(Context context, Transform transform)
	{
		transform.SplitOperand(context.Result, out var resultLow, out var resultHigh);
		transform.SplitOperand(context.Operand1, out var op1L, out var op1H);

		var count = context.Operand2;

		context.SetInstruction(X86.Shrd32, resultLow, op1L, op1H, count);
		context.AppendInstruction(X86.Sar32, resultHigh, op1H, count);
	}
}
