// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.StrengthReduction;

/// <summary>
/// RemUnsigned64ByPowerOfTwo
/// </summary>
[Transform("IR.Optimizations.Auto.StrengthReduction")]
public sealed class RemUnsigned64ByPowerOfTwo : BaseTransform
{
	public RemUnsigned64ByPowerOfTwo() : base(Framework.IR.RemUnsigned64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!IsResolvedConstant(context.Operand2))
			return false;

		if (!IsPowerOfTwo64(context.Operand2))
			return false;

		if (IsZero(context.Operand2))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;
		var t2 = context.Operand2;

		var e1 = Operand.CreateConstant(Sub64(ShiftLeft64(1, And64(GetPowerOfTwo(To32(t2)), Sub64(64, 1))), 1));

		context.SetInstruction(Framework.IR.And64, result, t1, e1);
	}
}
