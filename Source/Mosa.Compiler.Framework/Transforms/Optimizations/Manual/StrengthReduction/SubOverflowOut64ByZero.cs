// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Manual.StrengthReduction;

/// <summary>
/// SubOverflowOut64ByZero
/// </summary>
public sealed class SubOverflowOut64ByZero : BaseTransform
{
	public SubOverflowOut64ByZero() : base(Framework.IR.SubOverflowOut64, TransformType.Manual | TransformType.Optimization, true)
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

		context.SetInstruction(Framework.IR.Move64, result, operand1);
		context.AppendInstruction(Framework.IR.Move64, result2, Operand.Constant64_0);
	}
}

/// <summary>
/// SubOverflowOut64ByZero2
/// </summary>
public sealed class SubOverflowOut64ByZero2 : BaseTransform
{
	public SubOverflowOut64ByZero2() : base(Framework.IR.SubOverflowOut64, TransformType.Manual | TransformType.Optimization, true)
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

		context.SetInstruction(Framework.IR.Move64, result, operand2);
		context.AppendInstruction(Framework.IR.Move64, result2, Operand.Constant64_0);
	}
}
