// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.StrengthReduction;

/// <summary>
/// MulSigned32ByZero
/// </summary>
[Transform("IR.Optimizations.Auto.StrengthReduction")]
public sealed class MulSigned32ByZero : BaseTransform
{
	public MulSigned32ByZero() : base(IR.MulSigned32, TransformType.Auto | TransformType.Optimization)
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

		var e1 = Operand.CreateConstant(To32(0));

		context.SetInstruction(IR.Move32, result, e1);
	}
}

/// <summary>
/// MulSigned32ByZero_v1
/// </summary>
[Transform("IR.Optimizations.Auto.StrengthReduction")]
public sealed class MulSigned32ByZero_v1 : BaseTransform
{
	public MulSigned32ByZero_v1() : base(IR.MulSigned32, TransformType.Auto | TransformType.Optimization)
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

		var e1 = Operand.CreateConstant(To32(0));

		context.SetInstruction(IR.Move32, result, e1);
	}
}
