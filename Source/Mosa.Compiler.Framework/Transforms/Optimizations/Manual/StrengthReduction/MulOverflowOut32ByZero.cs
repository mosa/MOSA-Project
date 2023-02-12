// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.StrengthReduction;

/// <summary>
/// MulOverflowOut32ByZero
/// </summary>
public sealed class MulOverflowOut32ByZero : BaseTransform
{
	public MulOverflowOut32ByZero() : base(IRInstruction.MulOverflowOut32, TransformType.Auto | TransformType.Optimization, true)
	{
	}

	public override int Priority => 80;

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand2.IsResolvedConstant)
			return false;

		if (context.Operand2.ConstantUnsigned64 != 0)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var e1 = transform.CreateConstant(To32(0));

		context.SetInstruction(IRInstruction.Move32, result, e1);
	}
}

/// <summary>
/// MulSigned32ByZero_v1
/// </summary>
public sealed class MulSigned32ByZero_v1 : BaseTransform
{
	public MulSigned32ByZero_v1() : base(IRInstruction.MulSigned32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 80;

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand1.IsResolvedConstant)
			return false;

		if (context.Operand1.ConstantUnsigned64 != 0)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;

		var e1 = transform.CreateConstant(To32(0));

		context.SetInstruction(IRInstruction.Move32, result, e1);
	}
}
