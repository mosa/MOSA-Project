// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.StrengthReduction;

/// <summary>
/// AddOverflowOut32ByZero
/// </summary>
public sealed class AddOverflowOut32ByZero : BaseTransform
{
	public AddOverflowOut32ByZero() : base(IRInstruction.AddOverflowOut32, TransformType.Manual | TransformType.Optimization, true)
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
		var operand1 = context.Operand1;

		context.SetInstruction(IRInstruction.Move32, result, operand1);
		context.AppendInstruction(IRInstruction.Move32, result2, transform.Constant32_0);
	}
}

/// <summary>
/// AddOverflowOut32ByZero2
/// </summary>
public sealed class AddOverflowOut32ByZero2 : BaseTransform
{
	public AddOverflowOut32ByZero2() : base(IRInstruction.AddOverflowOut32, TransformType.Manual | TransformType.Optimization, true)
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
		var operand2 = context.Operand2;

		context.SetInstruction(IRInstruction.Move32, result, operand2);
		context.AppendInstruction(IRInstruction.Move32, result2, transform.Constant32_0);
	}
}
