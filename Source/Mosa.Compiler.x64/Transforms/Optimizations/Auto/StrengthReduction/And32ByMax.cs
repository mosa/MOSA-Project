// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.x64.Transforms.Optimizations.Auto.StrengthReduction;

public sealed class And32ByMax : BaseTransform
{
	public And32ByMax() : base(X64.And32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand2.IsResolvedConstant)
			return false;

		if (context.Operand2.ConstantUnsigned64 != 0xFFFFFFFF)
			return false;

		if (AreAnyStatusFlagsUsed(context, 10))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var e1 = Operand.Constant32_FFFFFFFF;

		context.SetInstruction(X64.Mov32, result, e1);
	}
}

public sealed class And32ByMax_v1 : BaseTransform
{
	public And32ByMax_v1() : base(X64.And32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsResolvedConstant)
			return false;

		if (context.Operand1.ConstantUnsigned64 != 0xFFFFFFFF)
			return false;

		if (AreAnyStatusFlagsUsed(context, 10))
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var e1 = Operand.Constant32_FFFFFFFF;

		context.SetInstruction(X64.Mov32, result, e1);
	}
}
