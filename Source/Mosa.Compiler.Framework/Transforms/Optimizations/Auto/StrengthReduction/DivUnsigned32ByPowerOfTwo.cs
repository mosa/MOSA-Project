// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.StrengthReduction;

/// <summary>
/// DivUnsigned32ByPowerOfTwo
/// </summary>
[Transform("IR.Optimizations.Auto.StrengthReduction")]
public sealed class DivUnsigned32ByPowerOfTwo : BaseTransform
{
	public DivUnsigned32ByPowerOfTwo() : base(IRInstruction.DivUnsigned32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!IsResolvedConstant(context.Operand2))
			return false;

		if (!IsPowerOfTwo32(context.Operand2))
			return false;

		if (IsZero(context.Operand2))
			return false;

		if (IsOne(context.Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;
		var t2 = context.Operand2;

		var e1 = Operand.CreateConstant(GetPowerOfTwo(To32(t2)));

		context.SetInstruction(IRInstruction.ShiftRight32, result, t1, e1);
	}
}
