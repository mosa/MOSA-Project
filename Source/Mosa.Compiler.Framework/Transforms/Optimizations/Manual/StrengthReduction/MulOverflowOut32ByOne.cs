// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.StrengthReduction;

/// <summary>
/// MulOverflowOut32ByOne
/// </summary>
public sealed class MulOverflowOut32ByOne : BaseTransform
{
	public MulOverflowOut32ByOne() : base(IRInstruction.MulOverflowOut32, TransformType.Auto | TransformType.Optimization, true)
	{
	}

	public override int Priority => 80;

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand2.IsResolvedConstant)
			return false;

		if (context.Operand2.ConstantUnsigned64 != 1)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;
		var result2 = context.Result2;

		var t1 = context.Operand1;

		context.SetInstruction(IRInstruction.Move32, result, t1);
		context.AppendInstruction(IRInstruction.Move32, result2, transform.Constant32_1);
	}
}

/// <summary>
/// MulOverflowOut32ByOne_v1
/// </summary>
public sealed class MulOverflowOut32ByOne_v1 : BaseTransform
{
	public MulOverflowOut32ByOne_v1() : base(IRInstruction.MulOverflowOut32, TransformType.Auto | TransformType.Optimization)
	{
	}

	public override int Priority => 80;

	public override bool Match(Context context, TransformContext transform)
	{
		if (!context.Operand1.IsResolvedConstant)
			return false;

		if (context.Operand1.ConstantUnsigned64 != 1)
			return false;

		return true;
	}

	public override void Transform(Context context, TransformContext transform)
	{
		var result = context.Result;
		var result2 = context.Result2;

		var t1 = context.Operand2;

		context.SetInstruction(IRInstruction.Move32, result, t1);
		context.AppendInstruction(IRInstruction.Move32, result2, transform.Constant32_1);
	}
}
