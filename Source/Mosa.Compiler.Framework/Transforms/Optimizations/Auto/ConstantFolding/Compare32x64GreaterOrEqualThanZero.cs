// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.ConstantFolding;

[Transform("IR.Optimizations.Auto.ConstantFolding")]
public sealed class Compare32x64GreaterOrEqualThanZero : BaseTransform
{
	public Compare32x64GreaterOrEqualThanZero() : base(IR.Compare32x64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 100;

	public override bool Match(Context context, Transform transform)
	{
		if (context.ConditionCode != ConditionCode.UnsignedGreaterOrEqual)
			return false;

		if (!context.Operand2.IsResolvedConstant)
			return false;

		if (context.Operand2.ConstantUnsigned64 != 0)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var c1 = Operand.CreateConstant(1);

		context.SetInstruction(IR.Move64, result, c1);
	}
}

[Transform("IR.Optimizations.Auto.ConstantFolding")]
public sealed class Compare32x64GreaterOrEqualThanZero_v1 : BaseTransform
{
	public Compare32x64GreaterOrEqualThanZero_v1() : base(IR.Compare32x64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 100;

	public override bool Match(Context context, Transform transform)
	{
		if (context.ConditionCode != ConditionCode.UnsignedLessOrEqual)
			return false;

		if (!context.Operand1.IsResolvedConstant)
			return false;

		if (context.Operand1.ConstantUnsigned64 != 0)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var c1 = Operand.CreateConstant(1);

		context.SetInstruction(IR.Move64, result, c1);
	}
}
