// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.StrengthReduction;

/// <summary>
/// MulCarryOut32ByZero
/// </summary>
public sealed class MulCarryOut32ByZero : BaseTransform
{
	public MulCarryOut32ByZero() : base(IRInstruction.MulCarryOut32, TransformType.Auto | TransformType.Optimization, true)
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
		var result2 = context.Result2;

		context.SetInstruction(IRInstruction.Move32, result, transform.Constant32_0);
		context.AppendInstruction(IRInstruction.Move32, result2, transform.Constant32_1);
	}
}

/// <summary>
/// MulCarryOut32ByZero_v1
/// </summary>
public sealed class MulCarryOut32ByZero_v1 : BaseTransform
{
	public MulCarryOut32ByZero_v1() : base(IRInstruction.MulCarryOut32, TransformType.Auto | TransformType.Optimization)
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
		var result2 = context.Result2;

		var e1 = transform.CreateConstant(To32(0));

		context.SetInstruction(IRInstruction.Move32, result, e1);
		context.AppendInstruction(IRInstruction.Move32, result2, transform.Constant32_1);
	}
}
