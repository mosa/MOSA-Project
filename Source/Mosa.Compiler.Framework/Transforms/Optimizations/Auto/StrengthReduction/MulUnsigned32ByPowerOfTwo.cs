// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.StrengthReduction;

/// <summary>
/// MulUnsigned32ByPowerOfTwo
/// </summary>
[Transform("IR.Optimizations.Auto.StrengthReduction")]
public sealed class MulUnsigned32ByPowerOfTwo : BaseTransform
{
	public MulUnsigned32ByPowerOfTwo() : base(IRInstruction.MulUnsigned32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
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

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;
		var t2 = context.Operand2;

		var e1 = Operand.CreateConstant(GetPowerOfTwo(To32(t2)));

		context.SetInstruction(IRInstruction.ShiftLeft32, result, t1, e1);
	}
}

/// <summary>
/// MulUnsigned32ByPowerOfTwo_v1
/// </summary>
[Transform("IR.Optimizations.Auto.StrengthReduction")]
public sealed class MulUnsigned32ByPowerOfTwo_v1 : BaseTransform
{
	public MulUnsigned32ByPowerOfTwo_v1() : base(IRInstruction.MulUnsigned32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
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

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;
		var t2 = context.Operand2;

		var e1 = Operand.CreateConstant(GetPowerOfTwo(To32(t1)));

		context.SetInstruction(IRInstruction.ShiftLeft32, result, t2, e1);
	}
}
