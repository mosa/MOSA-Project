// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

using Mosa.Compiler.Framework;

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.StrengthReduction;

/// <summary>
/// MulSigned64ByZero
/// </summary>
[Transform("IR.Optimizations.Auto.StrengthReduction")]
public sealed class MulSigned64ByZero : BaseTransform
{
	public MulSigned64ByZero() : base(IRInstruction.MulSigned64, TransformType.Auto | TransformType.Optimization)
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

		var e1 = transform.CreateConstant(To64(0));

		context.SetInstruction(IRInstruction.Move64, result, e1);
	}
}

/// <summary>
/// MulSigned64ByZero_v1
/// </summary>
[Transform("IR.Optimizations.Auto.StrengthReduction")]
public sealed class MulSigned64ByZero_v1 : BaseTransform
{
	public MulSigned64ByZero_v1() : base(IRInstruction.MulSigned64, TransformType.Auto | TransformType.Optimization)
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

		var e1 = transform.CreateConstant(To64(0));

		context.SetInstruction(IRInstruction.Move64, result, e1);
	}
}
