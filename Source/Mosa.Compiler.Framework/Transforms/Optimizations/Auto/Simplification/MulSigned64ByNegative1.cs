// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.Simplification;

[Transform()]
public sealed class MulSigned64ByNegative1 : BaseTransform
{
	public MulSigned64ByNegative1() : base(IR.MulSigned64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand2.IsResolvedConstant)
			return false;

		if (context.Operand2.ConstantUnsigned64 != 18446744073709551615)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand1;

		var e1 = Operand.Constant64_0;

		context.SetInstruction(IR.Sub64, result, e1, t1);
	}
}

[Transform()]
public sealed class MulSigned64ByNegative1_v1 : BaseTransform
{
	public MulSigned64ByNegative1_v1() : base(IR.MulSigned64, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsResolvedConstant)
			return false;

		if (context.Operand1.ConstantUnsigned64 != 18446744073709551615)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;

		var t1 = context.Operand2;

		var e1 = Operand.Constant64_0;

		context.SetInstruction(IR.Sub64, result, e1, t1);
	}
}
