// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.StrengthReduction;

/// <summary>
/// MulSigned32ByPowerOfTwo
/// </summary>
public sealed class MulSigned32ByPowerOfTwo : BaseTransform
{
	public MulSigned32ByPowerOfTwo() : base(IRInstruction.MulSigned32, TransformType.Auto | TransformType.Optimization)
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

		var e1 = transform.CreateConstant(GetPowerOfTwo(To32(t2)));

		context.SetInstruction(IRInstruction.ShiftLeft32, result, t1, e1);
	}
}

/// <summary>
/// MulSigned32ByPowerOfTwo_v1
/// </summary>
public sealed class MulSigned32ByPowerOfTwo_v1 : BaseTransform
{
	public MulSigned32ByPowerOfTwo_v1() : base(IRInstruction.MulSigned32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, TransformContext transform)
	{
		if (!IsResolvedConstant(context.Operand1))
			return false;

		if (!IsPowerOfTwo32(context.Operand1))
			return false;

		if (IsZero(context.Operand1))
			return false;

		if (IsOne(context.Operand1))
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;
		var t2 = context.Operand2;

		var e1 = transform.CreateConstant(GetPowerOfTwo(To32(t1)));

		context.SetInstruction(IRInstruction.ShiftLeft32, result, t2, e1);
	}
}
