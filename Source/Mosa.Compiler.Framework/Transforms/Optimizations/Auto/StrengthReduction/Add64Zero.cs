// Copyright (c) MOSA Project. Licensed under the New BSD License.

// This code was generated by an automated template.

namespace Mosa.Compiler.Framework.Transforms.Optimizations.Auto.StrengthReduction;

/// <summary>
/// Add64Zero
/// </summary>
[Transform("IR.Optimizations.Auto.StrengthReduction")]
public sealed class Add64Zero : BaseTransform
{
	public Add64Zero() : base(Framework.IR.Add64, TransformType.Auto | TransformType.Optimization)
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

		var t1 = context.Operand1;

		context.SetInstruction(Framework.IR.Move64, result, t1);
	}
}

/// <summary>
/// Add64Zero_v1
/// </summary>
[Transform("IR.Optimizations.Auto.StrengthReduction")]
public sealed class Add64Zero_v1 : BaseTransform
{
	public Add64Zero_v1() : base(Framework.IR.Add64, TransformType.Auto | TransformType.Optimization)
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

		var t1 = context.Operand2;

		context.SetInstruction(Framework.IR.Move64, result, t1);
	}
}
