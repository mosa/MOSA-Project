// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.StrengthReduction;

/// <summary>
/// SubOverflowOut32ByZero
/// </summary>
public sealed class SubOverflowOut32ByZero : BaseTransform
{
	public SubOverflowOut32ByZero() : base(IR.SubOverflowOut32, TransformType.Manual | TransformType.Optimization, true)
	{
	}

	public override int Priority => 80;

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand2.IsResolvedConstant)
			return false;

		if (context.Operand2.ConstantUnsigned64 != 0)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var result2 = context.Result2;
		var operand1 = context.Operand1;

		context.SetInstruction(IR.Move32, result, operand1);
		context.AppendInstruction(IR.Move32, result2, Operand.Constant32_0);
	}
}

/// <summary>
/// SubOverflowOut32ByZero2
/// </summary>
public sealed class SubOverflowOut32ByZero2 : BaseTransform
{
	public SubOverflowOut32ByZero2() : base(IR.SubOverflowOut32, TransformType.Manual | TransformType.Optimization, true)
	{
	}

	public override int Priority => 80;

	public override bool Match(Context context, Transform transform)
	{
		if (!context.Operand1.IsResolvedConstant)
			return false;

		if (context.Operand1.ConstantUnsigned64 != 0)
			return false;

		return true;
	}

	public override void Transform(Context context, Transform transform)
	{
		var result = context.Result;
		var result2 = context.Result2;
		var operand2 = context.Operand2;

		context.SetInstruction(IR.Move32, result, operand2);
		context.AppendInstruction(IR.Move32, result2, Operand.Constant32_0);
	}
}
